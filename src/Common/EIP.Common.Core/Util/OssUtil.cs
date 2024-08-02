using Aliyun.OSS;
using Aliyun.OSS.Util;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace EIP.Common.Core.Util
{
    public class OssUtil
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFile(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            string url = "";
            if (options.StorageType == FilesStorageOptionsType.LocalStorage.ToString())
            {
                url = await UpLoadFileForLocalStorage(options, fileExt, file);
            }
            else if (options.StorageType == FilesStorageOptionsType.AliYunOSS.ToString())
            {
                url = await UpLoadFileForAliYunOSS(options, fileExt, file);
            }
            else if (options.StorageType == FilesStorageOptionsType.QCloudOSS.ToString())
            {
                url = await UpLoadFileForQCloudOSS(options, fileExt, file);
            }
            else if (options.StorageType == FilesStorageOptionsType.QiNiuKoDo.ToString())
            {
                url = await UpLoadFileForQiNiuKoDo(options, fileExt, file);
            }
            return url;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UpLoadFileCheck(FilesStorageOptions options, IFormFile file)
        {
            var maxSize = 1024 * 1024 * options.MaxSize; //上传大小5M
            var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
            //检查大小
            if (file.Length > maxSize)
            {
                return "上传文件大小超过限制，最大允许上传" + options.MaxSize + "M";
            }
            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) ||
               System.Array.IndexOf(options.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + options.FileTypes;
            }
            return "";
        }
        #region 本地上传方法(File)
        /// <summary>
        /// 本地上传方法(File)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <param name="filesStorageLocation"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForLocalStorage(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");
            var saveUrl = options.Path + today + "/";
            if (!Directory.Exists(saveUrl)) Directory.CreateDirectory(saveUrl);
            var filePath = saveUrl + newFileName;
            string bucketBindDomain = options.BucketBindUrl;
            await using (var fs = File.Create(filePath))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            return bucketBindDomain + today + "/" + newFileName;
        }
        #endregion

        #region 阿里云上传方法（File）
        /// <summary>
        /// 阿里云上传方法（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForAliYunOSS(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            //上传到阿里云
            await using var fileStream = file.OpenReadStream();
            var md5 = OssUtils.ComputeContentMd5(fileStream, file.Length);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            //初始化阿里云配置--外网Endpoint、访问ID、访问password
            var aliYun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
            //将文件md5值赋值给meat头信息，服务器验证文件MD5
            var objectMeta = new ObjectMetadata
            {
                ContentMd5 = md5
            };
            //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
            aliYun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
            //返回给UEditor的插入编辑器的图片的src

            return options.BucketBindUrl + filePath;
        }

        #endregion

        #region 腾讯云存储上传方法（File）
        /// <summary>
        /// 腾讯云存储上传方法（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForQCloudOSS(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径

            //上传到腾讯云OSS
            //初始化 CosXmlConfig
            string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
            string region = options.CosRegion; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
                //.SetAppid(appid)
                .IsHttps(true)  //设置默认 HTTPS 请求
                .SetRegion(region)  //设置一个默认的存储桶地域
                .SetDebugLog(true)  //显示日志
                .Build();  //创建 CosXmlConfig 对象

            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);


            byte[] bytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);
            cosXml.PutObject(putObjectRequest);

            return options.BucketBindUrl + filePath;
        }
        #endregion

        #region 七牛云存储上传（File）
        /// <summary>
        /// 七牛云存储上传（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForQiNiuKoDo(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_fffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            Mac mac = new Mac(options.AccessKeyId, options.AccessKeySecret);
            byte[] bytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }
            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = options.QiNiuBucketName;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传token
            string token = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Config config = new Config();
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;

            UploadManager um = new UploadManager(config);
            var outData = um.UploadData(bytes, newFileName, token, null);

            return options.BucketBindUrl + newFileName;
        }
        #endregion

        #region 阿里云上传方法（Base64）
        /// <summary>
        /// 阿里云上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="memStream"></param>
        /// <returns></returns>
        public async Task<string> UpLoadBase64ForAliYunOSS(FilesStorageOptions options, MemoryStream memStream)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");

            // 设置当前流的位置为流的开始
            memStream.Seek(0, SeekOrigin.Begin);

            await using var fileStream = memStream;
            var md5 = OssUtils.ComputeContentMd5(fileStream, memStream.Length);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            //初始化阿里云配置--外网Endpoint、访问ID、访问password
            var aliyun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
            //将文件md5值赋值给meat头信息，服务器验证文件MD5
            var objectMeta = new ObjectMetadata
            {
                ContentMd5 = md5
            };
            //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
            aliyun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
            //返回给UEditor的插入编辑器的图片的src

            return options.BucketBindUrl + filePath;

        }

        #endregion

        #region 腾讯云存储上传方法（Base64）

        /// <summary>
        /// 腾讯云存储上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string UpLoadBase64ForQCloudOSS(FilesStorageOptions options, byte[] bytes)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");

            //初始化 CosXmlConfig
            string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
            string region = options.CosRegion; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
                //.SetAppid(appid)
                .IsHttps(true)  //设置默认 HTTPS 请求
                .SetRegion(region)  //设置一个默认的存储桶地域
                .SetDebugLog(true)  //显示日志
                .Build();  //创建 CosXmlConfig 对象

            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);

            var cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);

            cosXml.PutObject(putObjectRequest);

            return options.BucketBindUrl + filePath;
        }
        #endregion

        #region 牛云上传方法（Base64）

        /// <summary>
        /// 七牛云上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string UpLoadBase64ForQiNiuKoDo(FilesStorageOptions options, byte[] bytes)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_fffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";

            Mac mac = new Mac(options.AccessKeyId, options.AccessKeySecret);

            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = options.QiNiuBucketName;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传token
            string token = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Config config = new Config();
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;

            UploadManager um = new UploadManager(config);
            var outData = um.UploadData(bytes, newFileName, token, null);

            return options.BucketBindUrl + newFileName;
        }
        #endregion
    }

    /// <summary>
    ///     存储配置转换对象
    /// </summary>
    public class FilesStorageOptions
    {
        /// <summary>
        ///     存储方式（'LocalStorage','AliYunOSS','QCloudOSS'）
        /// </summary>
        public string StorageType { get; set; } = "LocalStorage";
        /// <summary>
        ///     存储目录
        /// </summary>
        public string Path { get; set; } = "Upload";
        /// <summary>
        ///     账户标识（腾讯云）
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        ///     存储桶地域（腾讯云）
        /// </summary>
        public string CosRegion { get; set; }

        /// <summary>
        ///     存储桶名称（腾讯云）
        /// </summary>
        public string TencentBucketName { get; set; }

        /// <summary>
        ///     存储桶名称（七牛云）
        /// </summary>
        public string QiNiuBucketName { get; set; }

        /// <summary>
        ///     授权账户
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        ///     授权密钥
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        ///     节点
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        ///     桶名称
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        ///     桶绑定域名
        /// </summary>
        public string BucketBindUrl { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        public string FileTypes { get; set; } = "gif,jpg,jpeg,png,bmp,xls,xlsx,doc,pdf,mp4,WebM,Ogv";

        /// <summary>
        ///     最大允许上传单个文件大小（M）
        /// </summary>
        public int MaxSize { get; set; } = 20;
    }

    /// <summary>
    /// 附件存储支持类型
    /// </summary>
    public enum FilesStorageOptionsType
    {
        [Description("本地存储")]
        LocalStorage = 1,
        [Description("阿里云OSS")]
        AliYunOSS = 2,
        [Description("腾讯云COS")]
        QCloudOSS = 3,
        [Description("七牛云KoDo")]
        QiNiuKoDo = 4,
    }
}
