/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EIP.Common.Core.Util
{
    public static class DEncryptUtil
    {
        #region 加密解密法一
        //默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string Encrypt(string encryptString, string encryptKey = "Eip963Ace#321Key")
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dcsp = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message + encryptString;
            }
        }
        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string Decrypt(string decryptString, string decryptKey = "Eip963Ace#321Key")
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                var dcsp = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message + decryptString;
            }
        }
        #endregion

        #region Md5

        public static string Md5(this string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        #endregion

        #region 3DES 加密解密

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key">必须16位</param>
        /// <returns></returns>
        public static string Des3Encrypt(this string data, string key = "Eip963Ace#321Key")
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(data);
            var tripleDes = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDes.Key = allKey;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Des3Decrypt(this string data, string key = "Eip963Ace#321Key")
        {
            byte[] inputArray = Convert.FromBase64String(data);
            var tripleDes = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDes.Key = allKey;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region Base64位加密解密
        /// <summary>
        /// 将字符串转换成base64格式,使用UTF8字符集
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <returns></returns>
        public static string Base64Encode(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 将base64格式，转换utf8
        /// </summary>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public static string Base64Decode(string content)
        {
            byte[] bytes = Convert.FromBase64String(content);
            return Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region HMACSHA256
        /// <summary>
        /// Base64 SHA256
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string EncryptWithSHA256(string data, string secret)
        {
            secret = secret ?? "";
            var encoding = Encoding.UTF8;
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] dataBytes = encoding.GetBytes(data);
            using (var hmac256 = new HMACSHA256(keyByte))
            {
                byte[] hashData = hmac256.ComputeHash(dataBytes);
                return Convert.ToBase64String(hashData);
            }
        }

        /// <summary>
        /// 原始64位 SHA256
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string EncryptWithSHA256Original(string data, string secret)
        {
            secret = secret ?? "";
            var encoding = Encoding.UTF8;
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] dataBytes = encoding.GetBytes(data);
            using (var hmac256 = new HMACSHA256(keyByte))
            {
                byte[] hashData = hmac256.ComputeHash(dataBytes);
                return BitConverter.ToString(hashData).Replace("-", "").ToLower();
            }
        }
        #endregion

        #region AES 加密

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="aesKey">密钥</param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string aesKey)
        {
            string data = string.Empty;
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(aesKey))
            {
                byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
                using (Aes aes = Aes.Create())
                {
                    var iv = new byte[16];
                    for (int i = 0; i < iv.Length; i++)
                        iv[i] = 0;
                    aes.IV = iv;
                    aes.Key = Encoding.UTF8.GetBytes(aesKey);
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.BlockSize = 128;
                    var cryptoTransform = aes.CreateEncryptor();
                    var resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    data = Convert.ToBase64String(resultArray, 0, resultArray.Length);

                }
            }
            return data;
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="aesKey">密钥</param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string aesKey)
        {
            string data = string.Empty;
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(aesKey))
            {
                byte[] toEncryptArray = Convert.FromBase64String(str);
                using (Aes aes = Aes.Create())
                {
                    var iv = new byte[16];
                    for (int i = 0; i < iv.Length; i++)
                        iv[i] = 0;
                    aes.IV = iv;
                    aes.Key = Encoding.UTF8.GetBytes(aesKey);
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.BlockSize = 128;
                    var cryptoTransform = aes.CreateDecryptor();
                    byte[] resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    data = Encoding.UTF8.GetString(resultArray);
                }

            }
            return data;
        }

        #endregion
    }
}