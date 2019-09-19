using Microsoft.AspNetCore.Mvc;
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
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Senparc.Areas.Admin.Controllers
{

    [MenuFilter("CompetitionProgram")]
    public class CompetitionProgramController : BaseAdminController
    {
      //  private readonly SenparcEntities _senparcEntities;
        private readonly ScheduleService _scheduleService;
        private readonly CompetitionProgramService _competitionProgramService;
        private readonly ActivityService _activityService;
        private readonly ProjectMemberService _projectMemberService;
        private readonly AdminUserInfoService _adminUserInfoService;
        public CompetitionProgramController(
             CompetitionProgramService competitionProgramService
            // , SenparcEntities senparcEntities
            ,ActivityService activityService
           , AdminUserInfoService adminUserInfoService
            , ProjectMemberService projectMemberService
            , ScheduleService scheduleService

           )
        {
            _competitionProgramService = competitionProgramService;
           // _senparcEntities = senparcEntities;
            _scheduleService = scheduleService;
            _activityService = activityService;
            _adminUserInfoService = adminUserInfoService;
            _projectMemberService = projectMemberService;
        }

        public ActionResult Index(string kw = null,string projectId = null, int pageIndex = 1)
        {
            var seh = new SenparcExpressionHelper<CompetitionProgram>();
            seh.ValueCompare.AndAlso(true, z => !z.Flag)
                .AndAlso(!projectId.IsNullOrEmpty(),z=>z.ScheduleId == projectId)
                .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Company.Contains(kw)|| z.SignNum.Contains(kw) ||  z.Remark.Contains(kw) || z.Desc.Contains(kw));
                

            var where = seh.BuildWhereExpression();

            var activies = _activityService.GetFullList(x => x.IsPublish, x => x.IsPublish, OrderingType.Descending).ToList();

            if (activies.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先完善活动主题");
                return RedirectToAction("Index");
            }
            var curactivity = activies.Select(x => x.Id).ToList();

            // List<Schedule> list = new List<Schedule>();

            var clist = _scheduleService.GetFullList(x => curactivity.Contains(x.ActivityId), x => x.Sort, OrderingType.Ascending).ToList().Select(x => x).ToList();


            var modelList = _competitionProgramService.GetObjectList(pageIndex, 20, where, z => z.Id, OrderingType.Descending);

            List<CompetitionProgram_EditVD> list = new List<CompetitionProgram_EditVD>();

            modelList.ForEach(x =>
            {

                var leader = new ProjectMember();

                if (!string.IsNullOrEmpty(x.ControlId))
                {
                    var cleader = _projectMemberService.GetObject(xp => xp.IsLeader && xp.ProjectId == x.Id);

                    if (cleader != null)
                    {
                        leader = cleader;
                    }
                }

                var strcateName = string.Empty;

                if (x.Schedule == null)
                {
                    x.Schedule = _scheduleService.GetObject(s => s.Id == x.ScheduleId);
                }

                list.Add(new CompetitionProgram_EditVD
                {
                    ActivityId = x.Schedule.ActivityId,
                    BdImgUrl = x.BdImgUrl,
                    BdImgUrlPwd = x.BdImgUrlPwd,
                    Cate = x.Cate,
                    ScheduleName = x.Schedule.Name,
                    //CategoryName = string.Empty,
                    Company = x.Company,
                    ControlId = x.ControlId,
                    CreateTime = x.CreateTime,
                    CreatorId = x.CreatorId,
                    CreatorName = x.CreatorName,
                    Desc = x.Desc,
                    Id = x.Id,
                    ImgUrl = x.ImgUrl,
                    LeaderCom = leader.Company,
                    LeaderDuty =leader.Duty,
                    LeaderName = leader.Name,
                    Name = x.Name,
                    Remark = x.Remark,
                    ProjectId = x.ScheduleId,
                    SignNum = x.SignNum,
                    Status = x.Status,
                    UpdateTime = x.UpdateTime,
                    UpdatorId = x.UpdatorId,
                    UpdatorName = x.UpdatorName,
                    Flag = x.Flag,
                                     
                 

                });


            });

            var vd = new CompetitionProgram_IndexVD()
            {
                CompetitionProgramList = new PagedList<CompetitionProgram_EditVD>(list, pageIndex, modelList.PageCount, modelList.TotalCount, modelList.SkipCount),
                kw = kw,
                 ProjectId = projectId,
                  Schedules = clist
            };
            return View(vd);
        }


        public ActionResult Edit(string id)
        {
            bool isEdit = !string.IsNullOrEmpty(id);
            var vd = new CompetitionProgram_EditVD();


            var activies = _activityService.GetFullList(x => x.IsPublish, x => x.IsPublish, OrderingType.Descending).ToList();
          
            if (activies.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先完善活动主题");
                return RedirectToAction("Index");
            }

            List<Schedule> list = new List<Schedule>();

            if (isEdit)
            {               
                var model = _competitionProgramService.GetObject(z => z.Id == id);          
                if (model == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }
                var modellist = _competitionProgramService.GetFullList(x => !string.IsNullOrEmpty(x.SignNum)&& x.ScheduleId == model.ScheduleId,x=>x.CreateTime,OrderingType.Descending).Select(x=>x.SignNum).ToList();


                if (model.Schedule == null)
                {
                    model.Schedule = _scheduleService.GetObject(s => s.Id == model.ScheduleId);
                }

                var numlist = new List<string>();
                if (model.Schedule != null)
                {
                    numlist = model.Schedule.SignNumber.Split(",").ToList();
                }
              


                List<string> num = new List<string>();

                foreach (var item in numlist)
                {
                    if(!modellist.Contains(item))
                    {
                        num.Add(item);
                    }
                }

                list = _scheduleService.GetFullList(x => x.ActivityId == model.Schedule.ActivityId, x => x.StartTime, OrderingType.Ascending).ToList();
                //   vd.Id = model.Id;
                var leader =_projectMemberService.GetObject(x => x.IsLeader && x.ProjectId == model.Id);



                vd.AvalidSignNums = num;
                vd.Name = model.Name;            
                vd.Desc = model.Desc;
                vd.ImgUrl = model.ImgUrl;
                vd.ProjectId = model.ScheduleId;
                vd.ActivityId = model.Schedule.ActivityId;
                vd.ScheduleSignNum =string.Join(",",num);
                vd.Cate = model.Cate;
                vd.Remark = model.Remark;
                vd.Desc = model.Desc;
                vd.BdImgUrl = model.BdImgUrl;
                vd.BdImgUrlPwd = model.BdImgUrlPwd;
                vd.Company = model.Company;
                vd.ControlId = model.ControlId;              
                vd.UpdatorId = UserName;
                vd.UpdatorName = AdminUser == null ? "" : AdminUser.RealName;
                vd.UpdateTime = DateTime.Now;
                vd.SignNum = model.SignNum;
                vd.Status = model.Status;

                if (leader != null)
                {
                    vd.LeaderCard = leader.IdCard;
                    vd.LeaderCom = leader.Company;
                    vd.LeaderDuty = leader.Duty;
                    vd.LeaderEmail = leader.Email;
                    vd.LeaderName = leader.Name;
                    vd.LeaderNation = leader.Nation;
                    vd.LeaderPhone = leader.Phone;
                }
        
               
            }
            else
            {
                var curactivir = activies.FirstOrDefault();

                list = _scheduleService.GetFullList(x => x.ActivityId == curactivir.Id, x => x.StartTime, OrderingType.Ascending).ToList();

                //vd.ScheduleSignNum = ;
            }
           
            if (list.Count == 0)
            {
                base.SetMessager(MessageType.danger, "请先安排好赛程");
                return RedirectToAction("Index");
            }
            var accounts = _adminUserInfoService.GetFullList(x => !"初始化数据".Equals(x.Note), x => x.AddTime, OrderingType.Descending);
            vd.UserList = accounts;
            vd.Activities = activies;
            vd.Schedules= list;

            vd.IsEdit = isEdit;
            return View(vd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompetitionProgram_EditVD model)
        {
            bool isEdit = !string.IsNullOrEmpty(model.Id);

            bool iseditManger = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            CompetitionProgram account = null;

            ProjectMember pm = null;
            if (isEdit)
            {
                account = _competitionProgramService .GetObject(z => z.Id == model.Id);
                if (account == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }

                pm = _projectMemberService.GetObject(x => x.ProjectId == account.Id);

                if (pm != null)
                {
                   // pm.Id = Guid.NewGuid().ToString("N");
                    pm.Company = model.LeaderCom;
                    pm.Duty = model.LeaderDuty;
                    pm.Email =model.LeaderEmail;
                    //pm.CreateTime = DateTime.Now;
                    pm.Flag = false;
                    pm.Gender = 0;
                    pm.Name = model.LeaderName;
                    pm.Nation = model.LeaderNation;
                    pm.ProjectId = account.Id;
                    pm.Phone = model.LeaderPhone;
                    pm.UpdateTime = DateTime.Now;
                    pm.IsLeader = true;
                    pm.IdCard = model.LeaderCard;
                    pm.Sort = 1;
                }

                account.Name        = model.Name;
                account.Desc        = model.Desc;
                account.ImgUrl      = model.ImgUrl;
                account.Cate        = model.Cate;
                account.Remark      = model.Remark;
                account.ScheduleId = model.ProjectId;
                account.Desc        = model.Desc;
                account.BdImgUrl    = model.BdImgUrl;
                account.BdImgUrlPwd = model.BdImgUrlPwd;
                account.Company     = model.Company;
                account.ControlId   = model.ControlId;
                account.UpdatorId   = UserName;
                account.UpdatorName = AdminUser == null ? "" : AdminUser.RealName;
                account.UpdateTime  = DateTime.Now;
                account.SignNum     = model.SignNum;
                account.Status      = model.Status;
                account.UpdateTime  = DateTime.Now;
                account.UpdatorId   = UserName;
                account.UpdatorName = AdminUser == null ? "" : AdminUser.RealName;
            }
            else
            {
                account = new CompetitionProgram()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Company = model.Company,
                    Desc = model.Desc,
                    Flag = false,
                    ControlId = model.ControlId,
                    ScheduleId = model.ProjectId,
                    BdImgUrl = model.BdImgUrl,
                    BdImgUrlPwd = model.BdImgUrlPwd,
                    ImgUrl = model.ImgUrl,
                    Cate = model.Cate,
                    UpdateTime = DateTime.Now,
                    UpdatorId = UserName,
                    UpdatorName = AdminUser == null ? "" : AdminUser.RealName,
                    CreatorName = AdminUser == null ? "" : AdminUser.RealName,
                    SignNum = model.SignNum,
                    Status = model.Status,
                    Remark = model.Remark,                     
                    CreateTime = DateTime.Now, 
                    CreatorId = UserName,                   
                };

               
            }
            try
            {

                await TryUpdateModelAsync(account, "", 
                                          v => v.Name       
                                        , v => v.Desc       
                                        , v => v.ImgUrl     
                                        , v => v.Cate       
                                        , v => v.Remark     
                                        , v => v.Desc       
                                        , v => v.BdImgUrl   
                                        , v => v.BdImgUrlPwd
                                        , v => v.Company    
                                        , v => v.ControlId  
                                        , v => v.UpdatorId  
                                        , v => v.UpdatorName
                                        , v => v.UpdateTime
                                        , v => v.SignNum
                                        , v => v.Status
                                        , v => v.UpdateTime
                                        , v => v.UpdatorId
                                        , v => v.UpdatorName  );


                _competitionProgramService.SaveObject(account);
                if (pm != null)
                {
                    _projectMemberService.SaveObject(pm);
                }
                else
                {
                    pm = new ProjectMember
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Company = model.LeaderCom,
                        Duty = model.LeaderDuty,
                        Email = string.Empty,
                        CreateTime = DateTime.Now,
                        Flag = false,
                        Gender = 0,
                        Name = model.Name,
                        Nation = model.LeaderNation,
                        ProjectId = account.Id,
                        Phone = model.LeaderPhone,
                        UpdateTime = DateTime.Now,
                        IsLeader = true,
                        IdCard = model.LeaderCard,
                        Sort = 1,
                    };
                    _projectMemberService.SaveObject(pm);
                }
              
                base.SetMessager(MessageType.success, $"{(isEdit ? "修改" : "新增")}成功！");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                base.SetMessager(MessageType.danger, $"{(isEdit ? "修改" : "新增")}失败！" + ex.Message);
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

            if (item != null && !string.IsNullOrEmpty(item.SignNumber))
            {
                var list = item.SignNumber.Split(",").ToList();
                return item.SignNumber;

            }

            return "OK,Cancel";
        }
    }
}

