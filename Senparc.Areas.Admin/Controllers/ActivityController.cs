using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Areas.Admin.Models.VD;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Mvc.Filter;
using Senparc.Office;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Controllers
{
    [MenuFilter("Activity")]
    public class ActivityController : BaseAdminController
    {
        private readonly AccountService _accountService;
      //  private readonly SenparcEntities _senparcEntities;
        private readonly ActivityService _activityService;


        public ActivityController(AccountService accountService
                                 // , SenparcEntities senparcEntities
                                  , ActivityService activityService
                                    )
        {
            _accountService = accountService;
          //  _senparcEntities = senparcEntities;
            _activityService = activityService;
        }

        public ActionResult Index(string kw = null, int pageIndex = 1)
        {
            //var seh = new SenparcExpressionHelper<Account>();
            //seh.ValueCompare.AndAlso(true, z => !z.Flag)
            //    .AndAlso(!kw.IsNullOrEmpty(), z => z.RealName.Contains(kw) || z.NickName.Contains(kw) || z.UserName.Contains(kw) || z.Phone.Contains(kw));
            //var where = seh.BuildWhereExpression();

            //var modelList = _accountService.GetObjectList(pageIndex, 20, where, z => z.Id, OrderingType.Descending);
            //var vd = new Account_IndexVD()
            //{
            //    AccountList = modelList,
            //    kw = kw
            //};

            var seh = new SenparcExpressionHelper<Activity>();
            seh.ValueCompare.AndAlso(true, z => !z.Flag)
                .AndAlso(!kw.IsNullOrEmpty(), z => z.Title.Contains(kw) || z.Content.Contains(kw) || z.Description.Contains(kw));
            var where = seh.BuildWhereExpression();

            var modelList = _activityService.GetObjectList(pageIndex, 20, where, z => z.Id, OrderingType.Descending);
            var vd = new Activity_IndexVD()
            {
                 ActivityList = modelList,
                kw = kw
            };
            return View(vd);
        }


        public ActionResult Create()
        {
            return View();
                
        }


        public ActionResult Edit(string id)
        {
            bool isEdit = !string.IsNullOrEmpty(id);
            var vd = new Activity_EditVD();
            if (isEdit)
            {
                var model = _activityService.GetObject(z => z.Id == id);
                if (model == null)
                {
                    return RenderError("信息不存在！");
                }
                vd.Id = model.Id;
                vd.CoverUrl = model.CoverUrl;
                vd.Content = model.Content;
                vd.Description = model.Description;
                vd.Summary = model.Summary;
                vd.IsPublish = model.IsPublish;
                vd.Title = model.Title;
                vd.ScheduleStatus = model.ScheduleStatus;
                //vd.Note = model.Note;
            }
            vd.IsEdit = isEdit;
            return View(vd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Activity_EditVD model)
        {
            bool isEdit =!string.IsNullOrEmpty(model.Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Activity account = null;
            if (isEdit)
            {
                account = _activityService.GetObject(z => z.Id == model.Id);
                if (account == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }

                account.ScheduleStatus = model.ScheduleStatus;
                

                     account.Content = model.Content ?? "";
                     account.CoverUrl = model.CoverUrl ?? "";
                     account.Description = model.Description ?? "";
                     
                     account.IsPublish = true;
                    
                     account.Summary = model.Summary ?? "";
                     account.Title = model.Title ?? "";
                account.ScheduleStatus = model.ScheduleStatus;
            }
            else
            {
                account = new Activity()
                {
                     Id = Guid.NewGuid().ToString("N"),
                     Content = model.Content ?? "",
                     CoverUrl = model.CoverUrl ?? "",
                     Description = model.Description ?? "",
                     Flag = false,
                     IsPublish = true,
                     IssueTime = DateTime.Now,
                     Summary = model.Summary??"",
                     Title = model.Title??"",
                     ScheduleStatus = model.ScheduleStatus
                };
            }
            try
            {
                //if (_accountService.CheckPhoneExisted(account.Id, model.Phone))
                //{
                //    ModelState.AddModelError("Phone", "手机号码重复");
                //    return View(model);
                //}

                // await this.TryUpdateModelAsync<Activity>(account, ""
                //, z => 
                //, z => z.Phone
                //, z => z.Note);

                await this.TryUpdateModelAsync(account, "",
                                    v => v.Title,
                                    v => v.Content,
                                    v => v.Summary,
                                   v => v.Flag,
                                    v => v.IsPublish,
                                    v => v.CoverUrl
                                    );
                this._activityService.SaveObject(account);
                base.SetMessager(MessageType.success, $"{(isEdit ? "修改" : "新增")}成功！");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                base.SetMessager(MessageType.danger, $"{(isEdit ? "修改" : "新增")}失败！");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(List<string> ids)
        {
            try
            {
                var objList = _activityService.GetFullList(z => ids.Contains(z.Id), z => z.Id, OrderingType.Ascending);
                _activityService.DeleteAll(objList);
                SetMessager(MessageType.success, "删除成功！");
            }
            catch (Exception e)
            {
                SetMessager(MessageType.danger, $"删除失败【{e.Message}！");
            }
            return RedirectToAction("Index");
        }
    }
}
