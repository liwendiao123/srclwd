using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Senparc.CO2NET;
using Senparc.Core.Extensions;
using Senparc.Core.Oss;
using Senparc.Mvc.Util;
using Senparc.SMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Mvc.Controllers
{
    [UserAuthorize("UserAnonymous")]
    [AllowAnonymous]
    public class CommonComponentsControllers: BaseController
    {

        public CommonComponentsControllers()
        {

        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymousAttribute]
        public ActionResult UploadKindEditor(IFormFile imgFile,string dir)
        {
            //String aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);

            try
            {
                //HttpPostedFileBase file = Request.Files["imgFile"];

                //定义允许上传的文件扩展名
                Hashtable extTable = new Hashtable();
                extTable.Add("image", "gif,jpg,jpeg,png,bmp");
                extTable.Add("flash", "swf,flv");
                extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
                extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

                //获取文件名
                string sFilename = System.IO.Path.GetFileName(imgFile.FileName).ToLower();
                //获取upImage文件的扩展名
                string extendName = System.IO.Path.GetExtension(sFilename);

                //最大文件大小
                int maxSize = 1024 * 1024 * 1024;

                if (imgFile == null)
                {
                    //showError("请选择文件。");
                }

                if (imgFile == null || imgFile.Length > maxSize)
                {
                    //showError("上传文件大小超过限制。");
                }

                String dirName = dir;
                if (String.IsNullOrEmpty(dirName))
                {
                    dirName = "image";
                }

                if (String.IsNullOrEmpty(extendName) || Array.IndexOf(((String)extTable[dirName]).Split(','), extendName.Substring(1).ToLower()) == -1)
                {
                  //  showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
                }
                long fileLeng = imgFile.Length;
                byte[] photoValue = new byte[fileLeng];
                //  file.InputStream.Read(photoValue, 0, fileLeng);
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        imgFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        photoValue = fileBytes;
                    }
                }
                catch (Exception ex)
                { }
                finally {


                }
              
                string strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff");//取得时间字符串
                string strRan = Convert.ToString(new Random().Next(100, 999));//生成三位随机数
                string saveName = strDateTime + strRan + extendName;

                //string bucketName = "xintou-kindeditor-0001";
                //string bucketName = Configs.GetPicBucketName ?? "hongsenlin-picture-0001";
                var senparcQiniuSetting = SenparcDI.GetService<IOptions<SenparcQiniuSetting>>();
                //Common.AliyunOSS.CreateBucket(bucketName);
                if (QiniuClient.UploadFileData(saveName, photoValue, "richedit/"))
                {
                    //return Json(new
                    //{
                    //    Result = true,
                    //    Message = "",
                    //    WebSite = "https://" + Configs.GetOSSAddress + "/richedit/" + saveName
                    //}, JsonRequestBehavior.AllowGet);

                    String fileUrl = "http://" + senparcQiniuSetting.Value.QiniuOSSAddress + "/richedit/" + saveName;
                    Hashtable hash = new Hashtable();
                    hash["error"] = 0;
                    hash["url"] = fileUrl;

                    Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");

                    using (StreamWriter sw = new StreamWriter(Response.Body))
                    {
                        sw.Write(JsonConvert.SerializeObject(hash));
                    }
                    // await Response.WriteAsync(JsonConvert.SerializeObject(hash));
                    return new EmptyResult();
                    //Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    //  Response.Write(JsonMapper.ToJson(hash));
                    //  Response.End();
                }
                else
                {
                   // showError("上传文件到OSS出错！");
                    // return Json(new { Result = false, Message = "上传图片到OSS出错！" }, JsonRequestBehavior.AllowGet);
                }
                //if (Common.AliyunOSS.PutByteObject(bucketName, saveName, photoValue))
                //{
                //    String fileUrl = "https://" + bucketName + "." + Configs.GetOSSAddress + "/" + saveName;
                //    Hashtable hash = new Hashtable();
                //    hash["error"] = 0;
                //    hash["url"] = fileUrl;
                //    Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                //    Response.Write(JsonMapper.ToJson(hash));
                //    Response.End();
                //}
                //else
                //{
                //    showError("上传文件到OSS出错！");
                //}
            }
            catch (Exception ex)
            {
                SetMessager(Core.Enums.MessageType.danger, "上传文件到OSS出错！");
                //showError("上传文件到OSS出错！");
            }
            return View();

        }
    }
}
