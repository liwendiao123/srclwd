using Microsoft.Extensions.Options;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using Senparc.CO2NET;
using Senparc.Core.Oss;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Util
{
   public class QiniuClient
    {
        private static SenparcQiniuSetting senparcQiniuSetting = SenparcDI.GetService<IOptions<SenparcQiniuSetting>>().Value;
        private static Mac mac = new Mac(senparcQiniuSetting.QiniuAccessKeyId, senparcQiniuSetting.QiniuOSSAccessKeySecret);

        public static void UploadFile(string filename, string filepath)
        {
            
            // 上传文件名
            string key = filename;
            // 本地文件路径
            string filePath = filepath;
            // 存储空间名
            string Bucket = senparcQiniuSetting.QiniuOSSPictureBucket;
            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            //putPolicy.SetExpires(3600);
            //putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Qiniu.Common.Config.AutoZone(senparcQiniuSetting.QiniuAccessKeyId, Bucket, false);


            // // 设置上传区域
            // config.Zone = Zone.ZONE_CN_South;
            // // 设置 http 或者 https 上传
            // config.UseHttps = true;
            // config.UseCdnDomains = true;
            // config.ChunkSize = ChunkUnit.U512K;
            // 表单上传
            FormUploader target = new FormUploader(true);
          

            HttpResult result = target.UploadFile(filePath, key, token, null);
            Console.WriteLine("form upload result: " + result.ToString());
        }

        public static bool UploadFileData(string filename, byte[] data, string filepath)
        {

            // // 上传文件名
             string key = filepath + filename;
            // // 本地文件路径
            // string filePath = filepath;
            // // 存储空间名
            // string Bucket = senparcQiniuSetting.QiniuOSSPictureBucket;
            // // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policytouchun
            // PutPolicy putPolicy = new PutPolicy();

            // putPolicy.Scope = Bucket;
            ////    putPolicy.Scope = Bucket;
            // putPolicy.SetExpires(3600);
            // //putPolicy.DeleteAfterDays = 1;
            // string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            // Config config = new Config();
            // // 设置上传区域
            // Qiniu.Common.Config.AutoZone(senparcQiniuSetting.QiniuAccessKeyId, Bucket, true);

            // Qiniu.CDN.CdnManager cdn = new Qiniu.CDN.CdnManager(mac);
            //  // 表单上传
            //  FormUploader target = new FormUploader(true);

            // //HttpResult result = target.UploadData(data, key, token, null);
            // HttpResult result = target.UploadData(data, key, token);
            // if (result != null && result.Code == 200)
            // {
            //     return true;
            // }

            // else
            // {
            //     return false;
            // }
            // // HttpResult result = target.UploadFile(filePath, key, token, null);
            // Console.WriteLine("form upload result: " + result.ToString());

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(senparcQiniuSetting.QiniuAccessKeyId, senparcQiniuSetting.QiniuOSSAccessKeySecret);
            //string bucket = "test";
            //string saveKey = "myfile";
           // byte[] data = System.IO.File.ReadAllBytes("D:/QFL/1.mp3");
            //byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello World!");
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = senparcQiniuSetting.QiniuOSSPictureBucket;
            Qiniu.Common.Config.AutoZone(senparcQiniuSetting.QiniuAccessKeyId, senparcQiniuSetting.QiniuOSSPictureBucket, true);
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            // putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
         
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = fu.UploadData(data, key, token);
            if (result != null && result.Code == 200)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
