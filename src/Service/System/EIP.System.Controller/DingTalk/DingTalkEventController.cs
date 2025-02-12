﻿using System.Collections;
using System.IO;
using System.Security.Cryptography;
using SystemSecurity = System.Security;
using SystemText = System.Text;

namespace EIP.System.Controller
{
    /// <summary>
    /// 钉钉事件回调
    /// </summary>
    public class DingTalkEventController : BaseSystemController
    {
        private readonly ISystemConfigLogic _configLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configLogic"></param>
        public DingTalkEventController(ISystemConfigLogic configLogic)
        {
            _configLogic = configLogic;
        }

        /// <summary>
        /// 事件回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-事件回调", RemarkFrom.System)]
        [Route("/dingtalk/event")]
        [AllowAnonymous]
        public async Task<Dictionary<String, String>> Event(string msg_signature, string timestamp, string nonce)
        {
            LogWriter.Debug("signature:" + msg_signature + " timestamp:" + timestamp + " nonce:" + nonce);
            try
            {
                var token = await _configLogic.FindByKey(EnumConfigKey.dingTalkToken.ToString());
                var encodingAesKey = await _configLogic.FindByKey(EnumConfigKey.dingTalkEncodingAesKey.ToString());
                var corpId = await _configLogic.FindByKey(EnumConfigKey.dingTalkCorpId.ToString());
                //获取三个参数
                DingTalkUtil ding = new DingTalkUtil(token, encodingAesKey, corpId);
                //接收encrypt参数
                string encryptStr = GetPostParam();
                ////解密内容
                string plainText = ding.getDecryptMsg(msg_signature, timestamp, nonce, encryptStr);
                Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
                //获取事件
                string eventType = tb["EventType"].ToString();
                //下面是根据事件名称进行相应的业务处理

                //返回success
                var msg = ding.getEncryptedMap("success");
                return msg;
            }
            catch (Exception ex)
            {
                LogWriter.Debug(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        private string GetPostParam()
        {
            Stream stream = HttpContext.Request.Body;
            byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
            stream.ReadAsync(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            LogWriter.Debug("content:" + content);
            return content.ToString().Replace("{\"encrypt\":\"", "").Replace("\"}", "");
        }
    }

    /**
    * 钉钉开放平台加解密方法
    */
    public class DingTalkUtil
    {
        //private static readonly Charset CHARSET = Charset.forName("utf-8");
        //private static readonly Base64         base64  = new Base64();
        private byte[] aesKey;
        private String token;
        private String corpId;
        /**ask getPaddingBytes key固定长度**/
        private static readonly int AES_ENCODE_KEY_LENGTH = 43;
        /**加密随机字符串字节长度**/
        private static readonly int RANDOM_LENGTH = 16;

        /**
         * 构造函数
         * @param token            钉钉开放平台上，开发者设置的token
         * @param encodingAesKey   钉钉开放台上，开发者设置的EncodingAESKey
         * @param corpId           企业自建应用-事件订阅, 使用appKey
         *                         企业自建应用-注册回调地址, 使用corpId
         *                         第三方企业应用, 使用suiteKey
         *
         * @throws DingTalkEncryptException 执行失败，请查看该异常的错误码和具体的错误信息
         */
        public DingTalkUtil(String token, String encodingAesKey, String corpId)
        {
            if (null == encodingAesKey || encodingAesKey.Length != AES_ENCODE_KEY_LENGTH)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.AES_KEY_ILLEGAL);
            }
            this.token = token;
            this.corpId = corpId;
            aesKey = Convert.FromBase64String(encodingAesKey + "=");
        }

        /**
         * 将和钉钉开放平台同步的消息体加密,返回加密Map
         */
        public Dictionary<String, String> getEncryptedMap(String plaintext)
        {

            var time = DateTime.Now.Millisecond;
            return getEncryptedMap(plaintext, time);
        }

        /**
         * 将和钉钉开放平台同步的消息体加密,返回加密Map
         * @param plaintext     传递的消息体明文
         * @param timeStamp      时间戳
         * @param nonce           随机字符串
         * @return
         * @throws DingTalkEncryptException
         */
        public Dictionary<String, String> getEncryptedMap(String plaintext, long timeStamp)
        {
            if (null == plaintext)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.ENCRYPTION_PLAINTEXT_ILLEGAL);
            }
            var nonce = Utils.getRandomStr(RANDOM_LENGTH);
            if (null == nonce)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.ENCRYPTION_NONCE_ILLEGAL);
            }

            String encrypt = this.encrypt(nonce, plaintext);
            String signature = getSignature(token, timeStamp.ToString(), nonce, encrypt);
            Dictionary<String, String> resultMap = new Dictionary<String, String>();
            resultMap["msg_signature"] = signature;
            resultMap["encrypt"] = encrypt;
            resultMap["timeStamp"] = timeStamp.ToString();
            resultMap["nonce"] = nonce;
            return resultMap;
        }

        /**
         * 密文解密
         * @param msgSignature     签名串
         * @param timeStamp        时间戳
         * @param nonce             随机串
         * @param encryptMsg       密文
         * @return                  解密后的原文
         * @throws DingTalkEncryptException
         */
        public String getDecryptMsg(String msgSignature, String timeStamp, String nonce, String encryptMsg)
        {
            //校验签名
            String signature = getSignature(token, timeStamp, nonce, encryptMsg);
            if (!signature.Equals(msgSignature))
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_SIGNATURE_ERROR);
            }
            // 解密
            String result = decrypt(encryptMsg);
            return result;
        }


        /*
         * 对明文加密.
         * @param text 需要加密的明文
         * @return 加密后base64编码的字符串
         */
        private String encrypt(String random, String plaintext)
        {
            try
            {
                byte[] randomBytes = SystemText.Encoding.UTF8.GetBytes(random);// random.getBytes(CHARSET);
                byte[] plainTextBytes = SystemText.Encoding.UTF8.GetBytes(plaintext);// plaintext.getBytes(CHARSET);
                byte[] lengthByte = Utils.int2Bytes(plainTextBytes.Length);
                byte[] corpidBytes = SystemText.Encoding.UTF8.GetBytes(corpId);// corpId.getBytes(CHARSET);
                //MemoryStream byteStream = new MemoryStream();
                var bytestmp = new List<byte>();
                bytestmp.AddRange(randomBytes);
                bytestmp.AddRange(lengthByte);
                bytestmp.AddRange(plainTextBytes);
                bytestmp.AddRange(corpidBytes);
                byte[] padBytes = PKCS7Padding.getPaddingBytes(bytestmp.Count);
                bytestmp.AddRange(padBytes);
                byte[] unencrypted = bytestmp.ToArray();

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.Zeros;
                rDel.Key = aesKey;
                rDel.IV = aesKey.ToList().Take(16).ToArray();
                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(unencrypted, 0, unencrypted.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);


                //Cipher cipher = Cipher.getInstance("AES/CBC/NoPadding");
                //SecretKeySpec keySpec = new SecretKeySpec(aesKey, "AES");
                //IvParameterSpec iv = new IvParameterSpec(aesKey, 0, 16);
                //cipher.init(Cipher.ENCRYPT_MODE, keySpec, iv);
                //byte[] encrypted = cipher.doFinal(unencrypted);
                //String result = base64.encodeToString(encrypted);
                //return result;
            }
            catch (Exception e)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_ENCRYPT_TEXT_ERROR);
            }
        }

        /*
         * 对密文进行解密.
         * @param text 需要解密的密文
         * @return 解密得到的明文
         */
        private String decrypt(String text)
        {
            byte[] originalArr;
            try
            {
                byte[] toEncryptArray = Convert.FromBase64String(text);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.Zeros;
                rDel.Key = aesKey;
                rDel.IV = aesKey.ToList().Take(16).ToArray();
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                originalArr = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                //return System.Text.UTF8Encoding.UTF8.GetString(resultArray);

                //// 设置解密模式为AES的CBC模式
                //Cipher cipher = Cipher.getInstance("AES/CBC/NoPadding");
                //SecretKeySpec keySpec = new SecretKeySpec(aesKey, "AES");
                //IvParameterSpec iv = new IvParameterSpec(Arrays.copyOfRange(aesKey, 0, 16));
                //cipher.init(Cipher.DECRYPT_MODE, keySpec, iv);
                //// 使用BASE64对密文进行解码
                //byte[] encrypted = Base64.decodeBase64(text);
                //// 解密
                //originalArr = cipher.doFinal(encrypted);
            }
            catch (Exception e)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_DECRYPT_TEXT_ERROR);
            }

            String plainText;
            String fromCorpid;
            try
            {
                // 去除补位字符
                byte[] bytes = PKCS7Padding.removePaddingBytes(originalArr);
                Console.Out.WriteLine("bytes size:" + bytes.Length);

                // 分离16位随机字符串,网络字节序和corpId
                byte[] networkOrder = bytes.Skip(16).Take(4).ToArray();// Arrays.copyOfRange(bytes, 16, 20);
                for (int i = 0; i < 4; i++)
                {
                    Console.Out.WriteLine("networkOrder size:" + (int)networkOrder[i]);
                }

                int plainTextLegth = Utils.bytes2int(networkOrder);

                plainText = SystemText.UTF8Encoding.UTF8.GetString(bytes.Skip(20).Take(plainTextLegth).ToArray()); // new String(Arrays.copyOfRange(bytes, 20, 20 + plainTextLegth), CHARSET);
                fromCorpid = SystemText.UTF8Encoding.UTF8.GetString(bytes.Skip(20 + plainTextLegth).ToArray()); //new String(Arrays.copyOfRange(bytes, 20 + plainTextLegth, bytes.length), CHARSET);

            }
            catch (Exception e)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_DECRYPT_TEXT_LENGTH_ERROR);
            }
            Console.Out.WriteLine(fromCorpid + "=====" + corpId);


            // corpid不相同的情况
            if (!fromCorpid.Equals(corpId))
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_DECRYPT_TEXT_CORPID_ERROR);
            }
            return plainText;
        }

        /**
         * 数字签名
         * @param token         isv token
         * @param timestamp     时间戳
         * @param nonce          随机串
         * @param encrypt       加密文本
         * @return
         * @throws DingTalkEncryptException
         */
        public String getSignature(String token, String timestamp, String nonce, String encrypt)
        {
            try
            {
                Console.Out.WriteLine(encrypt);

                String[] array = new String[] { token, timestamp, nonce, encrypt };
                Array.Sort(array, StringComparer.Ordinal);
                //var tmparray = array.ToList();
                //tmparray.Sort(new JavaStringComper());
                //array = tmparray.ToArray();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    sb.Append(array[i]);
                }
                String str = sb.ToString();
                Console.Out.WriteLine(str);
                //MessageDigest md = MessageDigest.getInstance("SHA-1");
                //md.update(str.getBytes());
                //byte[] digest = md.digest();
                SystemSecurity.Cryptography.SHA1 hash = SystemSecurity.Cryptography.SHA1.Create();
                SystemText.Encoding encoder = SystemText.Encoding.ASCII;
                byte[] combined = encoder.GetBytes(str);
                ////byte 转换
                //sbyte[] myByte = new sbyte[]
                //byte[] mySByte = new byte[myByte.Length];



                //for (int i = 0; i < myByte.Length; i++)

                //{

                //    if (myByte[i] > 127)

                //        mySByte[i] = (sbyte)(myByte[i] - 256);

                //    else

                //        mySByte[i] = (sbyte)myByte[i];

                //}

                byte[] digest = hash.ComputeHash(combined);
                StringBuilder hexstr = new StringBuilder();
                String shaHex = "";
                for (int i = 0; i < digest.Length; i++)
                {
                    shaHex = ((int)digest[i]).ToString("x");// Integer.toHexString(digest[i] & 0xFF);
                    if (shaHex.Length < 2)
                    {
                        hexstr.Append(0);
                    }
                    hexstr.Append(shaHex);
                }
                return hexstr.ToString();
            }
            catch (Exception e)
            {
                throw new DingTalkEncryptException(DingTalkEncryptException.COMPUTE_SIGNATURE_ERROR);
            }
        }
    }


    /**
 * 钉钉开放平台加解密异常类
 */
    public class DingTalkEncryptException : Exception
    {
        /**成功**/
        public static readonly int SUCCESS = 0;
        /**加密明文文本非法**/
        public readonly static int ENCRYPTION_PLAINTEXT_ILLEGAL = 900001;
        /**加密时间戳参数非法**/
        public readonly static int ENCRYPTION_TIMESTAMP_ILLEGAL = 900002;
        /**加密随机字符串参数非法**/
        public readonly static int ENCRYPTION_NONCE_ILLEGAL = 900003;
        /**不合法的aeskey**/
        public readonly static int AES_KEY_ILLEGAL = 900004;
        /**签名不匹配**/
        public readonly static int SIGNATURE_NOT_MATCH = 900005;
        /**计算签名错误**/
        public readonly static int COMPUTE_SIGNATURE_ERROR = 900006;
        /**计算加密文字错误**/
        public readonly static int COMPUTE_ENCRYPT_TEXT_ERROR = 900007;
        /**计算解密文字错误**/
        public readonly static int COMPUTE_DECRYPT_TEXT_ERROR = 900008;
        /**计算解密文字长度不匹配**/
        public readonly static int COMPUTE_DECRYPT_TEXT_LENGTH_ERROR = 900009;
        /**计算解密文字corpid不匹配**/
        public readonly static int COMPUTE_DECRYPT_TEXT_CORPID_ERROR = 900010;

        private static Dictionary<int, String> msgMap = new Dictionary<int, String>();
        static DingTalkEncryptException()
        {
            msgMap[SUCCESS] = "成功";
            msgMap[ENCRYPTION_PLAINTEXT_ILLEGAL] = "加密明文文本非法";
            msgMap[ENCRYPTION_TIMESTAMP_ILLEGAL] = "加密时间戳参数非法";
            msgMap[ENCRYPTION_NONCE_ILLEGAL] = "加密随机字符串参数非法";
            msgMap[SIGNATURE_NOT_MATCH] = "签名不匹配";
            msgMap[COMPUTE_SIGNATURE_ERROR] = "签名计算失败";
            msgMap[AES_KEY_ILLEGAL] = "不合法的aes key";
            msgMap[COMPUTE_ENCRYPT_TEXT_ERROR] = "计算加密文字错误";
            msgMap[COMPUTE_DECRYPT_TEXT_ERROR] = "计算解密文字错误";
            msgMap[COMPUTE_DECRYPT_TEXT_LENGTH_ERROR] = "计算解密文字长度不匹配";
            msgMap[COMPUTE_DECRYPT_TEXT_CORPID_ERROR] = "计算解密文字corpid不匹配";
        }

        private int code;
        public DingTalkEncryptException(int exceptionCode) : base(msgMap[exceptionCode])
        {
            this.code = exceptionCode;
        }
    }

    /*
     * PKCS7算法的加密填充
     */
    public class PKCS7Padding
    {
        //private readonly static Charset CHARSET = Charset.forName("utf-8");
        private readonly static int BLOCK_SIZE = 32;

        /**
         * 填充mode字节
         * @param count
         * @return
         */
        public static byte[] getPaddingBytes(int count)
        {
            int amountToPad = BLOCK_SIZE - (count % BLOCK_SIZE);
            if (amountToPad == 0)
            {
                amountToPad = BLOCK_SIZE;
            }
            char padChr = chr(amountToPad);
            String tmp = string.Empty; ;
            for (int index = 0; index < amountToPad; index++)
            {
                tmp += padChr;
            }
            return SystemText.Encoding.UTF8.GetBytes(tmp);
        }

        /**
         * 移除mode填充字节
         * @param decrypted
         * @return
         */
        public static byte[] removePaddingBytes(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > BLOCK_SIZE)
            {
                pad = 0;
            }
            //Array.Copy()
            var output = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, output, decrypted.Length - pad);
            return output;
        }

        private static char chr(int a)
        {
            byte target = (byte)(a & 0xFF);
            return (char)target;
        }

    }

    /**
 * 加解密工具类
 */
    public class Utils
    {
        /**
     *
     * @return
     */
        public static String getRandomStr(int count)
        {
            String baset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                int number = random.Next(baset.Length);
                sb.Append(baset[number]);
            }
            return sb.ToString();
        }


        /*
         * int转byte数组,高位在前
         */
        public static byte[] int2Bytes(int count)
        {
            byte[] byteArr = new byte[4];
            byteArr[3] = (byte)(count & 0xFF);
            byteArr[2] = (byte)(count >> 8 & 0xFF);
            byteArr[1] = (byte)(count >> 16 & 0xFF);
            byteArr[0] = (byte)(count >> 24 & 0xFF);
            return byteArr;
        }

        /**
         * 高位在前bytes数组转int
         * @param byteArr
         * @return
         */
        public static int bytes2int(byte[] byteArr)
        {
            int count = 0;
            for (int i = 0; i < 4; ++i)
            {
                count <<= 8;
                count |= byteArr[i] & 255;
            }
            return count;
        }
    }

    public class JavaStringComper : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return String.Compare(x, y);
        }
    }


    // 测试加解密的正确性
    public class Program
    {
        public static void Main(string[] args)
        {

            String[] a = new String[] { "1", "W", "t" };

            var ding = new DingTalkUtil("tokenxxxx", "o1w0aum42yaptlz8alnhwikjd3jenzt9cb9wmzptgus", "dingxxxxxx");
            var msg = ding.getEncryptedMap("success");
            Console.Out.WriteLine(msg);
            //msg_signature, $data->timeStamp, $data->nonce, $data->encrypt
            var text = ding.getDecryptMsg(msg["msg_signature"], msg["timeStamp"], msg["nonce"], msg["encrypt"]);
            Console.Out.WriteLine(text);
            // "msg_signature":"c01beb7b06384cf416e04930aed794684aae98c1","encrypt":"","timeStamp":,"nonce":""
            //{"timeStamp":"1605695694141","msg_signature":"702c953056613f5c7568b79ed134a27bd2dcd8d0",
            //"encrypt":"","nonce":"WelUQl6bCqcBa2fMc6eI"}
            text = ding.getDecryptMsg("f36f4ba5337d426c7d4bca0dbcb06b3ddc1388fc", "1605695694141", "WelUQl6bCqcBa2fM", "X1VSe9cTJUMZu60d3kyLYTrBq5578ZRJtteU94wG0Q4Uk6E/wQYeJRIC0/UFW5Wkya1Ihz9oXAdLlyC9TRaqsQ==");
            Console.Out.WriteLine(text);
        }
    }
}
