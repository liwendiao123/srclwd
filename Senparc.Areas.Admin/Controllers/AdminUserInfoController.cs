using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Areas.Admin.Models.VD;
using Senparc.Areas.Admin.NopiUtil;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Mvc;
using Senparc.Mvc.Filter;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Controllers
{
    [MenuFilter("AdminUserInfo")]
    public class AdminUserInfoController : BaseAdminController
    {
        private readonly AdminUserInfoService _adminUserInfoService;


        public AdminUserInfoController(AdminUserInfoService adminUserInfoService)
        {
            _adminUserInfoService = adminUserInfoService;
        }

        public ActionResult Index(int pageIndex = 1)
        {
            var seh = new SenparcExpressionHelper<AdminUserInfo>();
            seh.ValueCompare.AndAlso(true, z => !string.IsNullOrEmpty(z.Note) &&!string.IsNullOrEmpty(z.RealName));
            var where = seh.BuildWhereExpression();


            var admins = _adminUserInfoService.GetObjectList(pageIndex, 1000, where, z => z.Id, OrderingType.Ascending);
            var vd = new AdminUserInfo_IndexVD()
            {
                AdminUserInfoList = admins
            };
            return View(vd);
        }
        public ActionResult Edit(int id = 0)
        {
            bool isEdit = id > 0;
            var vd = new AdminUserInfo_EditVD();
            if (isEdit)
            {
                var userInfo = _adminUserInfoService.GetAdminUserInfo(id);
                if (userInfo == null)
                {
                    return RenderError("信息不存在！");
                }
                vd.UserName = userInfo.UserName;
                vd.Note = userInfo.Note;
                vd.RealName = userInfo.RealName;
                vd.Id = userInfo.Id;
            }
            vd.IsEdit = isEdit;
            return View(vd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserInfo_EditVD model)
        {

            bool isEdit = model.Id > 0;

            this.Validator(model.UserName, "用户名", "UserName", false)
                .IsFalse(z => this._adminUserInfoService.CheckUserNameExisted(model.Id, z), "用户名已存在！", true);

            if (!isEdit || !model.Password.IsNullOrEmpty())
            {
                this.Validator(model.Password, "密码", "Password", false).MinLength(6);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AdminUserInfo userInfo = null;
            if (isEdit)
            {
                userInfo = _adminUserInfoService.GetAdminUserInfo(model.Id);
                if (userInfo == null)
                {
                    return RenderError("信息不存在！");
                }
            }
            else
            {
                var passwordSalt = DateTime.Now.Ticks.ToString();
                userInfo = new AdminUserInfo()
                {
                    PasswordSalt = passwordSalt,
                    LastLoginTime = DateTime.Now,
                    ThisLoginTime = DateTime.Now,
                    AddTime = DateTime.Now,
                };
            }

            if (!model.Password.IsNullOrEmpty())
            {
                userInfo.Password = this._adminUserInfoService.GetPassword(model.Password, userInfo.PasswordSalt, false);//生成密码
            }
            userInfo.RealName = model.RealName;
            await this.TryUpdateModelAsync(userInfo, "", z => z.Note,z=>z.RealName ,z => z.UserName);
            this._adminUserInfoService.SaveObject(userInfo);

            base.SetMessager(MessageType.success, $"{(isEdit ? "修改" : "新增")}成功！");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var userInfoList = _adminUserInfoService.GetAdminUserInfo(ids);
            _adminUserInfoService.DeleteAll(userInfoList);
            SetMessager(MessageType.success, "删除成功！");
            return RedirectToAction("Index");
        }

        //[Route("UploadFiles")]
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {


                        using (var stream = formFile.OpenReadStream())
                        {

                            ExcelHelper excelHelper = new ExcelHelper();
                            string fileExtension = Path.GetExtension(formFile.FileName);
                            DataTable table = excelHelper.ExcelImport(stream, fileExtension, 0);

                            if (table != null)
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    if (row.ItemArray.Length != 6)
                                    {
                                        continue;
                                    }

                                    var passwordSalt = DateTime.Now.Ticks.ToString();
                                    AdminUserInfo model = null;
                                    var userName = row.ItemArray[4].ToString();
                                    var password = row.ItemArray[5].ToString();

                                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                                    {
                                        continue;
                                    }

                                    var existModel = _adminUserInfoService.GetUserInfo(userName);

                                    if (existModel != null)
                                    {
                                        model = existModel;
                                    }
                                    else
                                    {
                                        model = new AdminUserInfo()
                                        {
                                            PasswordSalt = passwordSalt,
                                            LastLoginTime = DateTime.Now,
                                            ThisLoginTime = DateTime.Now,
                                            AddTime = DateTime.Now,
                                        };
                                    }

                                    if (!password.IsNullOrEmpty())
                                    {
                                        model.Password = this._adminUserInfoService.GetPassword(password, model.PasswordSalt, false);//生成密码
                                    }

                                    model.RealName = row.ItemArray[3].ToString();
                                    model.Note = row.ItemArray[1].ToString();
                                    model.UserName = userName;
                                    await this.TryUpdateModelAsync(model, "", z => z.Note, z => z.RealName, z => z.UserName);
                                    this._adminUserInfoService.SaveObject(model);



                                }
                            }
                            await Task.Delay(10);
                           // await formFile.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
    }
}
