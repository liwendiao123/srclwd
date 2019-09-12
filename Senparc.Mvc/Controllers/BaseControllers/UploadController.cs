using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Senparc.CO2NET;
using Senparc.Core.Extensions;
using Senparc.Core.Oss;
using Senparc.File;
using Senparc.Mvc.Models.UploadedModel;
using Senparc.Mvc.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Senparc.Mvc.Controllers
{
    [UserAuthorize("UserAnonymous")]
    [AllowAnonymous]
    public class UploadController : BaseController
    {

       private readonly IHostingEnvironment _env;
        public UploadController(IHostingEnvironment env)
        {
            _env = env;
        }
       
        public IActionResult Imgupload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var fileFolder = Path.Combine(_env.WebRootPath, "scfrichedit");
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    Hashtable extTable = new Hashtable();
                    extTable.Add("image", "gif,jpg,jpeg,png,bmp");

                    //获取文件名
                    string sFilename = System.IO.Path.GetFileName(formFile.FileName).ToLower();
                    //获取upImage文件的扩展名
                    string extendName = System.IO.Path.GetExtension(sFilename);
                    //最大文件大小
                    int maxSize = 1024 * 1024 * 1024;
                    if (formFile == null)
                    {
                        SetMessager(Core.Enums.MessageType.danger, "请选择文件。");
                        //showError("请选择文件。");
                    }
                    if (formFile == null || formFile.Length > maxSize)
                    {
                        SetMessager(Core.Enums.MessageType.danger, "上传文件大小超过限制。");
                        //showError("上传文件大小超过限制。");
                    }


                    String dirName = "";
                    if (String.IsNullOrEmpty(dirName))
                    {
                        dirName = "image";
                    }
                    if (String.IsNullOrEmpty(extendName) || Array.IndexOf(((String)extTable[dirName]).Split(','), extendName.Substring(1).ToLower()) == -1)
                    {
                       
                        return Json(new UpLoadResult()
                        {
                            append = true,
                            error = "上传失败:" + "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。",
                            initialPreview = new List<string> {
                       "http://arbookresouce.73data.cn//arappbg.jpg"
                   },
                            initialPreviewConfig = new List<InitialPreviewConfig> {
                         new InitialPreviewConfig{
                              caption = "",
                               key = "0",
                                size = 10240,
                                 url  =  Request.Scheme + "://" + Request.Host + "/Upload/ImguploadDel?key=124",
                                  width = "120px"


                         }
                    },

                        });
                    }


                    long fileLeng = formFile.Length;
                    byte[] photoValue = new byte[fileLeng];
                    try
                    {
                        using (var ms = new MemoryStream())
                        {
                            formFile.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            photoValue = fileBytes;
                        }
                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                    }
                    string strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff");//取得时间字符串
                    string strRan = Convert.ToString(new Random().Next(100, 999));//生成三位随机数
                    string saveName = strDateTime + strRan + extendName;

                    //var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +
                    //               Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(fileFolder, saveName);
                    try
                    {
                        FileExtension.Upload(formFile, filePath);
                    }
                    catch (Exception ex)
                    {

                    }
                  
                    var senparcQiniuSetting = SenparcDI.GetService<IOptions<SenparcQiniuSetting>>();

                    if (QiniuClient.UploadFileData(saveName, photoValue, "scfrichedit/"))
                    {
                        String fileUrl = "http://" + senparcQiniuSetting.Value.QiniuOSSAddress + "/scfrichedit/" + saveName;
                        Hashtable hash = new Hashtable();
                        hash["error"] = 0;
                        hash["url"] = fileUrl;

                        return Json(new UpLoadResult()
                        {
                            append = true,
                            error = "",
                            code = 0,
                            curl = fileUrl,
                            initialPreview = new List<string> {
                                fileUrl
                             },
                            initialPreviewConfig = new List<InitialPreviewConfig> {
                         new InitialPreviewConfig{
                              caption = "",
                               key = "0",
                                size =formFile.Length,
                                 url  =  Request.Scheme + "://" + Request.Host + "/Upload/ImguploadDel?key=124",
                                  width = "120px"


                         }
                    },

                        });
                    }
                    else
                    {

                    }

                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    formFile.CopyTo(stream);
                    //}
                }
            }
            

            return Json(new UpLoadResult() {
                  append = true,
                  error = "上传失败",
                   initialPreview = new List<string> {
                       "http://arbookresouce.73data.cn//arappbg.jpg"
                   },
                    initialPreviewConfig = new List<InitialPreviewConfig> {
                         new InitialPreviewConfig{
                              caption = "",
                               key = "0",
                                size = 10240,
                                 url  =  Request.Scheme + "://" + Request.Host + "/Upload/ImguploadDel?key=124",
                                  width = "120px"


                         }
                    },

            });
        }

        public IActionResult ImguploadDel(int key)
        {
            return Json(new UpLoadResult()
            {
                append = true,
                error = "",
                initialPreview = new List<string> {
                       "http://arbookresouce.73data.cn//arappbg.jpg"
                   },
                initialPreviewConfig = new List<InitialPreviewConfig> {
                         new InitialPreviewConfig{
                              caption = "",
                               key = "0",
                                size = 10240,
                                 url  =  "http://arbookresouce.73data.cn//arappbg.jpg",
                                  width = "120px"


                         }
                    },

            });
        }


        public ActionResult UploadKindEditor(IFormFile imgFile, string dir)
        {
          

            try
            {
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
                    SetMessager(Core.Enums.MessageType.danger, "请选择文件。");
                    //showError("请选择文件。");
                }
                if (imgFile == null || imgFile.Length > maxSize)
                {
                    SetMessager(Core.Enums.MessageType.danger, "上传文件大小超过限制。");
                    //showError("上传文件大小超过限制。");
                }
                String dirName = dir;
                if (String.IsNullOrEmpty(dirName))
                {
                    dirName = "image";
                }
                if (String.IsNullOrEmpty(extendName) || Array.IndexOf(((String)extTable[dirName]).Split(','), extendName.Substring(1).ToLower()) == -1)
                {
                    SetMessager(Core.Enums.MessageType.danger, "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
                    //  showError();
                }
                long fileLeng = imgFile.Length;
                byte[] photoValue = new byte[fileLeng];          
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
                finally
                {
                }
                string strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff");//取得时间字符串
                string strRan = Convert.ToString(new Random().Next(100, 999));//生成三位随机数
                string saveName = strDateTime + strRan + extendName;             
                var senparcQiniuSetting = SenparcDI.GetService<IOptions<SenparcQiniuSetting>>();
                //Common.AliyunOSS.CreateBucket(bucketName);
                if (QiniuClient.UploadFileData(saveName, photoValue, "richedit/"))
                {
                    String fileUrl = "http://" + senparcQiniuSetting.Value.QiniuOSSAddress + "/richedit/" + saveName;
                    Hashtable hash = new Hashtable();
                    hash["error"] = 0;
                    hash["url"] = fileUrl;
                    Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                    using (StreamWriter sw = new StreamWriter(Response.Body))
                    {
                        sw.Write(JsonConvert.SerializeObject(hash));
                    }
                   
                    return new EmptyResult();
                   
                }
                else
                {
                    
                }
              
            }
            catch (Exception ex)
            {
                SetMessager(Core.Enums.MessageType.danger, "上传文件到OSS出错！");
               
            }
            return new EmptyResult();

        }
    }
}
