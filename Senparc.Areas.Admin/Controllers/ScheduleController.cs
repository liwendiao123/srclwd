using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.Areas.Admin.Models.VD;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Mvc.Filter;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Controllers
{
    [MenuFilter("Schedule")]
    public  class ScheduleController : BaseAdminController
    {
      //  private readonly SenparcEntities _senparcEntities;
        private readonly ScheduleService _scheduleService;
        private readonly ActivityService _activityService;
        public ScheduleController(
           // SenparcEntities senparcEntities
            ScheduleService scheduleService
            ,ActivityService activityService
            )
        {
            _scheduleService = scheduleService;
          //  _senparcEntities = senparcEntities;
            _activityService = activityService;
        }
        public ActionResult Index(string kw = null, int pageIndex = 1)
        {
            var seh = new SenparcExpressionHelper<Schedule>();
            seh.ValueCompare.AndAlso(true, z => !z.Flag)
                .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Address.Contains(kw) );
            var where = seh.BuildWhereExpression();

            var modelList = _scheduleService.GetObjectList(pageIndex, 20, where, z => z.Id, OrderingType.Descending);
            var vd = new Schedule_IndexVD()
            {
                ScheduleList = modelList,
                kw = kw
            };
            return View(vd);
        }


        public ActionResult Edit(string id)
        {
            bool isEdit = !string.IsNullOrEmpty(id);
            var vd = new Schedule_EditVD();

            var activity = _activityService.GetFullList(z => true, z => z.IsPublish, OrderingType.Descending);

            if (activity.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先添加活动！");
                return RedirectToAction("Index");
            }

            vd.ActivityList = activity;
            if (isEdit)
            {

                var model = _scheduleService.GetObject(z => z.Id == id);
                if (model == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }
               
                vd.Id = model.Id;
                vd.Name = model.Name;
                vd.Address = model.Address;
                vd.ActivityId = model.ActivityId;
                vd.Activity = model.Activity;
               
                vd.Remark = model.Remark;
                vd.Desc = model.Desc;
                vd.SignNumber = model.SignNumber;
                vd.StartTime = model.StartTime;
                vd.EndTime = model.EndTime;
                vd.Flag = model.Flag;
                //vd.Note = model.Note;
            }
            else
            {
                vd.SignNumber = "01,02" ;
                vd.CreateTime = DateTime.Now;
                vd.StartTime = DateTime.Now.Date;
                vd.EndTime = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            }
            vd.IsEdit = isEdit;
            return View(vd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Schedule_EditVD model)
        {
            bool isEdit = !string.IsNullOrEmpty(model.Id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Schedule account = null;
            if (isEdit)
            {
                account =  _scheduleService.GetObject(z => z.Id == model.Id);
                if (account == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }

                account.UpdateTime = DateTime.Now;
                account.UpdatorId = UserName;
                account.UpdatorName = AdminUser == null ? "" : AdminUser.RealName;
            }
            else
            {
                account = new Schedule()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Address = model.Address,
                    Desc = model.Desc,
                    Flag = false,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    SignNumber = model.SignNumber,
                    Remark = model.Remark,
                    ActivityId = model.ActivityId,
                    CreateTime = DateTime.Now,
                    Sort = model.Sort,
                    UpdateTime = DateTime.Now,
                    CreatorId = UserName,
                    Creator= AdminUser == null ? "" : AdminUser.RealName,
                    UpdatorId = UserName,
                    UpdatorName = AdminUser == null ? "" : AdminUser.RealName,

                };
            }
            try
            {

                await TryUpdateModelAsync(account, "", v => v.Name
                                        , v => v.Address
                                        , v => v.ActivityId
                                        , v => v.Desc
                                        , v => v.UpdatorId
                                        , v => v.UpdatorName
                                        , v => v.Remark
                                        , v => v.Desc
                                        , v => v.Sort
                                        , v => v.StartTime
                                        , v => v.EndTime
                                        , v => v.Flag
                                        , v => v.SignNumber);
 

                _scheduleService.SaveObject(account);
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
                var objList = _scheduleService.GetFullList(z => ids.Contains(z.Id), z => z.Id, OrderingType.Ascending);
                _scheduleService.DeleteAll(objList);
                SetMessager(MessageType.success, "删除成功！");
            }
            catch (Exception e)
            {
                SetMessager(MessageType.danger, $"删除失败【{e.Message}！");
            }
            return RedirectToAction("Index");
        }


        //public async Task<IActionResult> ScheduleTags()
        //{
        //    await Task.Delay(1);
        //    return Json(new List<string>
        //    {
        //        "One",
        //        "Two"
        //    });
        //}

        public string GetScheduleTags(string id)
        {

            var item = _scheduleService.GetObject(x => x.Id == id);

            if (item != null && !string.IsNullOrEmpty( item.SignNumber))
            {
                var list = item.SignNumber.Split(",").ToList();
                return item.SignNumber;

            }

            return "OK,Cancel";
        }
    }
}

