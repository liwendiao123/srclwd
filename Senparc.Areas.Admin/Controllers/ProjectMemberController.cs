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

    [MenuFilter("ProjectMember")]
    public class ProjectMemberController:BaseAdminController
    {
        //  private readonly SenparcEntities _senparcEntities;
        private readonly ScheduleService _scheduleService;
        private readonly CompetitionProgramService _competitionProgramService;
        private readonly ActivityService _activityService;
        private readonly ProjectMemberService _projectMemberService;
        public ProjectMemberController(
             CompetitionProgramService competitionProgramService
            // , SenparcEntities senparcEntities
            , ActivityService activityService
            , ProjectMemberService projectMemberService
            , ScheduleService scheduleService

           )
        {
            _competitionProgramService = competitionProgramService;
            // _senparcEntities = senparcEntities;
            _scheduleService = scheduleService;
            _activityService = activityService;
            _projectMemberService = projectMemberService;
        }

        public ActionResult Index(string kw = null,string ProjectId ="", int pageIndex = 1)
        {

            var activies = _activityService.GetFullList(x => x.IsPublish, x => x.IsPublish, OrderingType.Descending).ToList();

            if (activies.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先完善活动主题");
                return RedirectToAction("Index");
            }
            var curactivity = activies.Select(x => x.Id).ToList();

            // List<Schedule> list = new List<Schedule>();

            var clist = _scheduleService.GetFullList(x => curactivity.Contains(x.ActivityId), x => x.Sort, OrderingType.Ascending).ToList().Select(x => x.Id).ToList();

            var modellist = _competitionProgramService.GetFullList(x => clist.Contains(x.ScheduleId), x => x.CreateTime, OrderingType.Descending).Select(x => x).ToList();


            var seh = new SenparcExpressionHelper<ProjectMember>();
            seh.ValueCompare.AndAlso(true, z => !z.Flag)
                .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Company.Contains(kw) || z.Duty.Contains(kw) || z.Phone.Contains(kw) || z.Nation.Contains(kw));

            if (!string.IsNullOrEmpty(ProjectId))
            {
                seh.ValueCompare.AndAlso(true, z => z.ProjectId == ProjectId);
            }

            var where = seh.BuildWhereExpression();


            //var seh = new SenparcExpressionHelper<ProjectMember>();
            //seh.ValueCompare.AndAlso(true, z => !z.Flag)
            //    .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Company.Contains(kw) || z.Duty.Contains(kw) || z.Phone.Contains(kw) || z.Nation.Contains(kw));
            //var where = seh.BuildWhereExpression();

            var modelList = _projectMemberService.GetObjectList(pageIndex, 20, where, z => z.Id, OrderingType.Descending);
            List<ProjectMember_EditVD> list = new List<ProjectMember_EditVD>();

            modelList.ForEach(x =>
            {

               var leader = new CompetitionProgram();

                if (!string.IsNullOrEmpty(x.ProjectId))
                {
                    var cleader =_competitionProgramService.GetObject(xp => xp.Id == x.ProjectId);

                    if (cleader != null)
                    {
                        leader = cleader;
                    }
                }

                var strcateName = string.Empty;

                if (leader.Schedule == null)
                {
                    leader.Schedule = _scheduleService.GetObject(s => s.Id == leader.ScheduleId);
                }

                list.Add(new ProjectMember_EditVD
                {
                    ProjectId = x.ProjectId,
                    Name = x.Name,
                    Nation = x.Nation,
                    IdCard = x.IdCard,
                    Company = x.Company,
                    Duty = x.Duty,
                    Gender = x.Gender,
                    Id = x.Id,
                    Phone = x.Phone,
                    CreateTime = x.CreateTime,
                    CompetitionProgram = leader,
                    Email = x.Email,
                    IsLeader = x.IsLeader,
                    HeadImgUrl = x.HeadImgUrl,
                    IdCardImgUrl = x.IdCardImgUrl,
                    UserName = x.Phone                      


                });


            });

            var vd = new ProjectIndex_EditVD()
            {
                CompetitionProgramList = new PagedList<ProjectMember_EditVD>(list, pageIndex, modelList.PageCount, modelList.TotalCount, modelList.SkipCount),
                kw = kw,
                 ProjectId = ProjectId,
                  CpList = modellist
            };
            return View(vd);
        }


        public ActionResult Edit(string id)
        {
            bool isEdit = !string.IsNullOrEmpty(id);
            var vd = new ProjectMember_EditVD();


            var activies = _activityService.GetFullList(x => x.IsPublish, x => x.IsPublish, OrderingType.Descending).ToList();

            if (activies.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先完善活动主题");
                return RedirectToAction("Index");
            }
            var curactivity = activies.Select(x=>x.Id).ToList();

            // List<Schedule> list = new List<Schedule>();

          var list =  _scheduleService.GetFullList(x =>curactivity.Contains( x.ActivityId) , x => x.Sort, OrderingType.Ascending).ToList().Select(x=>x.Id).ToList();

            var modellist = _competitionProgramService.GetFullList(x => list.Contains( x.ScheduleId), x => x.CreateTime, OrderingType.Descending).Select(x =>x).ToList();
            if (isEdit)
            {
                var model = _projectMemberService.GetObject(z => z.Id == id);
                if (model == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }

                vd.Name = model.Name;
                vd.Id = model.Id;
                vd.IdCard = model.IdCard;
                vd.Nation = model.Nation;
                vd.Phone = model.Phone;
                vd.ProjectId = model.ProjectId;
                vd.Sort = model.Sort;
                vd.IsLeader = model.IsLeader;
                vd.Gender = model.Gender;
                vd.Duty = model.Duty;
                vd.Email = model.Email;
                vd.Company = model.Company;
                vd.HeadImgUrl = model.HeadImgUrl;
                vd.CreateTime = model.CreateTime;
                vd.UpdateTime = model.UpdateTime;
               

                

            }
            else
            {
             
            }

            if (modellist.Count == 0)
            {
                base.SetMessager(MessageType.danger, "没有参赛项目");
                return RedirectToAction("Index");
            }

            vd.CompetitionPrograms = modellist;
            //vd.Schedules = list;

            vd.IsEdit = isEdit;
            return View(vd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectMember_EditVD model)
        {
            bool isEdit = !string.IsNullOrEmpty(model.Id);

            bool iseditManger = false;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
          //  ProjectMember_EditVD account = null;

            ProjectMember pm = null;
            if (isEdit)
            {
                pm = _projectMemberService.GetObject(z => z.Id == model.Id);
                if (pm == null)
                {
                    base.SetMessager(MessageType.danger, "信息不存在！");
                    return RedirectToAction("Index");
                }

               // pm = _projectMemberService.GetObject(x => x.ProjectId == account.Id && x.Phone == account.ControlId);

                if (pm != null)
                {
                    // pm.Id = Guid.NewGuid().ToString("N");
                    pm.Company = model.Company;
                    pm.Duty = model.Duty;
                    pm.Email = model.Email;
                    //pm.CreateTime = DateTime.Now;
                    pm.Flag = false;
                    pm.Gender = model.Gender;
                    pm.Name = model.Name;
                    pm.Nation = model.Nation;
                    pm.ProjectId = model.ProjectId;
                    pm.Phone = model.Phone;
                    pm.UpdateTime = DateTime.Now;
                    pm.IsLeader = model.IsLeader;
                    pm.IdCard = model.IdCard;
                    pm.Sort = 1;
                }

               
            }
            else
            {
                pm = new ProjectMember()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Company = model.Company,
                    IdCard = model.IdCard,
                    IsLeader = model.IsLeader,
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Duty = model.Duty,
                    CreateTime = DateTime.Now,
                    Email = model.Email,
                    Flag = false,
                     HeadImgUrl =model.HeadImgUrl,
                      IdCardImgUrl = model.IdCardImgUrl,
                       Nation = model.Nation,
                        ProjectId = model.ProjectId,
                         Sort = model.Sort,
                          UpdateTime = DateTime.Now

                };


            }
            try
            {

                await TryUpdateModelAsync(pm, "",
                                          v => v.Name
                                        , v => v.IdCard
                                        , v => v.Company
                                        , v => v.Gender
                                        , v => v.Nation
                                        , v => v.Phone
                                        , v => v.ProjectId
                                        , v => v.Sort
                                        , v => v.IsLeader
                                        , v => v.HeadImgUrl
                                        , v => v.IdCardImgUrl
                                        );


                _projectMemberService.SaveObject(pm);
           

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
                var objList = _projectMemberService.GetFullList(z => ids.Contains(z.Id), z => z.Id, OrderingType.Ascending);
                _projectMemberService.DeleteAll(objList);
                SetMessager(MessageType.success, "删除成功！");
            }
            catch (Exception e)
            {
                SetMessager(MessageType.danger, $"删除失败【{e.Message}！");
            }
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportMember()
        {
            var activies = _activityService.GetFullList(x => x.IsPublish, x => x.IsPublish, OrderingType.Descending).ToList();

            if (activies.Count < 1)
            {
                base.SetMessager(MessageType.danger, "请先完善活动主题");
                return RedirectToAction("Index");
            }
            var curactivity = activies.Select(x => x.Id).ToList();

            // List<Schedule> list = new List<Schedule>();

            var clist = _scheduleService.GetFullList(x => curactivity.Contains(x.ActivityId), x => x.Sort, OrderingType.Ascending).ToList().Select(x => x.Id).ToList();

            var modellist = _competitionProgramService.GetFullList(x => clist.Contains(x.ScheduleId), x => x.CreateTime, OrderingType.Descending).Select(x => x).ToList();


            var seh = new SenparcExpressionHelper<ProjectMember>();
            //seh.ValueCompare.AndAlso(true, z => !z.Flag)
            //    .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Company.Contains(kw) || z.Duty.Contains(kw) || z.Phone.Contains(kw) || z.Nation.Contains(kw));

            //if (!string.IsNullOrEmpty(ProjectId))
            //{
            //    seh.ValueCompare.AndAlso(true, z => z.ProjectId == ProjectId);
            //}

            var where = seh.BuildWhereExpression();


            //var seh = new SenparcExpressionHelper<ProjectMember>();
            //seh.ValueCompare.AndAlso(true, z => !z.Flag)
            //    .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Company.Contains(kw) || z.Duty.Contains(kw) || z.Phone.Contains(kw) || z.Nation.Contains(kw));
            //var where = seh.BuildWhereExpression();

            var modelList = _projectMemberService.GetObjectList(1, 20000, where, z => z.Id, OrderingType.Descending);
            List<ProjectMember_EditVD> list = new List<ProjectMember_EditVD>();

            modelList.ForEach(x =>
            {

                var leader = new CompetitionProgram();

                if (!string.IsNullOrEmpty(x.ProjectId))
                {
                    var cleader = _competitionProgramService.GetObject(xp => xp.Id == x.ProjectId);

                    if (cleader != null)
                    {
                        leader = cleader;
                    }
                }

                var strcateName = string.Empty;

                if (leader.Schedule == null)
                {
                    leader.Schedule = _scheduleService.GetObject(s => s.Id == leader.ScheduleId);
                }

                list.Add(new ProjectMember_EditVD
                {
                    ProjectId = x.ProjectId,
                    Name = x.Name,
                    Nation = x.Nation,
                    IdCard = x.IdCard,
                    Company = x.Company,
                    Duty = x.Duty,
                    Gender = x.Gender,
                    Id = x.Id,
                    Phone = x.Phone,
                    CreateTime = x.CreateTime,
                    CompetitionProgram = leader,
                    Email = x.Email,
                    IsLeader = x.IsLeader,
                    HeadImgUrl = x.HeadImgUrl,
                    IdCardImgUrl = x.IdCardImgUrl,
                    UserName = x.Phone


                });


            });


           
            //var vd = new ProjectIndex_EditVD()
            //{
            //    CompetitionProgramList = new PagedList<ProjectMember_EditVD>(list, pageIndex, modelList.PageCount, modelList.TotalCount, modelList.SkipCount),
            //    kw = kw,
            //    ProjectId = ProjectId,
            //    CpList = modellist
            //};
            return View(null);
        }
       

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
