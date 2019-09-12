using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Enums;
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Mvc.Controllers.Api
{

   // [Session]
    public  class ScheduleController : BaseFrontController
    {
       //private readonly SenparcEntities _senparcEntities;
        private readonly ScheduleService _scheduleService;
        private readonly ActivityService _activityService;
        private readonly ProjectMemberService _projectMemberService;
        private readonly CompetitionProgramService _competitionProgramService;
        private readonly AdminUserInfoService _adminUserInfoService;
        public ScheduleController(
         //   SenparcEntities senparcEntities,
            ScheduleService scheduleService
            , ActivityService activityService
             , ProjectMemberService projectMemberService
            ,AdminUserInfoService adminUserInfoService
            , CompetitionProgramService competitionProgramService
            )
        {
            _scheduleService = scheduleService;
         //  _senparcEntities = senparcEntities;
            _activityService = activityService;
            _projectMemberService = projectMemberService;
            _competitionProgramService = competitionProgramService;
            _adminUserInfoService = adminUserInfoService;
        }

        public ActionResult GetList(string session)
        {
            try
            {
                AdminUserInfo adminUser = null;
                adminUser = VerifySession(session, adminUser, _adminUserInfoService);
                if (adminUser == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "请先登录",
                        data = new
                        {
                        }
                    });
                }
                var curactivity = _activityService.GetFullList(x => x.IsPublish, x => x.IssueTime, Core.Enums.OrderingType.Descending).FirstOrDefault();
                if (curactivity == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "暂时没有活动可参与",
                        data = new
                        {

                        }
                    });
                }
                //var seh = new SenparcExpressionHelper<Schedule>();
                //seh.ValueCompare.AndAlso(true, z => !z.Flag)
                //    .AndAlso(!kw.IsNullOrEmpty(), z => z.Name.Contains(kw) || z.Address.Contains(kw));
                //var where = seh.BuildWhereExpression();
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x).ToList();
                if (scheduleList.Count() == 0)
                {
                    if (curactivity == null)
                    {
                        return Json(new
                        {
                            code = -1,
                            msg = "主办方未安排赛程",
                            data = new
                            {

                            }
                        });
                    }
                }
                // var modelList = _scheduleService;
                //var vd = new Schedule_IndexVD()
                //{
                //    ScheduleList = modelList,
                //    kw = kw
                //};
                return Json(new
                {
                    code = 0,
                    msg = "获取数据成功",
                    data = scheduleList.Select(x => new {

                        x.Id,
                        x.ActivityId,
                        ActivityTitle = x.Activity.Title,
                        ActivityRemark = x.Activity.Summary,
                        ActivityDesc = x.Activity.Description,
                        x.Name,
                        x.Address,
                        x.StartTime,
                        x.EndTime,
                        x.Sort,
                        x.Remark,
                        x.SignNumber,
                        x.Desc,
                        x.CreateTime,
                        x.Flag

                    })

                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常:" + ex.Message,
                    data = new { }

                });
            }
           
        }


        public ActionResult GetComposeList(string  session)
        {
            try
            {
                AdminUserInfo adminUser = null;
                adminUser = VerifySession(session, adminUser, _adminUserInfoService);
                if (adminUser == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "请先登录",
                        data = new
                        {
                        }
                    });
                }


                //return Json(new
                //{
                //    code =0,
                //    msg = "ok",
                //    data = list
                //});

                var curactivity = _activityService.GetFullList(x => x.IsPublish, x => x.IssueTime, Core.Enums.OrderingType.Descending).FirstOrDefault();
                if (curactivity == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "暂时没有活动可参与",
                        data = new
                        {

                        }
                    });
                }
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x).ToList();
                if (scheduleList.Count() == 0)
                {
                    if (curactivity == null)
                    {
                        return Json(new
                        {
                            code = -1,
                            msg = "主办方未安排赛程",
                            data = new
                            {

                            }
                        });
                    }
                }
                // var modelList = _scheduleService;
                //var vd = new Schedule_IndexVD()
                //{
                //    ScheduleList = modelList,
                //    kw = kw
                //};
                return Json(new
                {
                    code = 0,
                    msg = "获取数据成功",
                    data = scheduleList.Select(x =>
                    {


                        var i = 0;

                        var list = _competitionProgramService.GetFullList(c => x.Id == (c.ScheduleId) && c.ControlId == adminUser.UserName, c => c.Status, Core.Enums.OrderingType.Ascending)
                      .GroupBy(c => c.Cate).Select(c => new CompetitionProgramViewModel
                      {
                          Cate = c.Key,
                          List = c.Where(y => y.Cate == c.Key).Select(v =>
                          {


                              var addrName = string.Empty;
                              if (v.Schedule == null && !string.IsNullOrEmpty(v.ScheduleId))
                              {
                                  var schedule = _scheduleService.GetObject(s => s.Id == v.ScheduleId);

                                  if (schedule != null)
                                  {
                                      addrName = schedule.Address;
                                  }
                              }
                              else if (v.Schedule != null)
                              {
                                  addrName = v.Schedule.Address;
                              }
                              var mlist = _projectMemberService.GetFullList(m => m.ProjectId == v.Id, m => m.Sort, OrderingType.Ascending).ToList();

                              return new CompetitionProgramVM
                              {
                                  Id = v.Id,
                                  Name = v.Name,
                                  Address = addrName,
                                  SignNum = v.SignNum,
                                  Sort = i++,
                                  Status = v.Status,
                                  IsSign = !string.IsNullOrEmpty(v.SignNum),
                                  BdImgUrl =  v.BdImgUrl,
                                  BdImgUrlPwd= v.BdImgUrlPwd,
                                  ProjectMembers = mlist.Select(mm => new MemberModel
                                  {

                                      Id = mm.Id ?? "",
                                      IdCard = mm.IdCard ?? "",
                                      ProjectId = mm.ProjectId ?? "",
                                      IdCardImgUrl = mm.IdCardImgUrl ?? "",
                                      Name = mm.Name ?? "",
                                      Phone = mm.Phone ?? "",
                                      Company = mm.Company??"",
                                      Nation = mm.Nation??"",
                                       Duty = mm.Duty??""

                                  }).ToList()

                              };
                          }).ToList()
                      }).ToList();


                        return new
                        {
                            x.Id,
                            x.ActivityId,
                            ActivityTitle = x.Activity.Title,
                            ActivityRemark = x.Activity.Summary,
                            ActivityDesc = x.Activity.Description,
                            x.Activity.ScheduleStatus,
                            x.Activity.IsPublish,
                            x.Name,
                            x.Address,
                            x.StartTime,
                            x.EndTime,
                            x.Sort,
                            x.Remark,
                            x.SignNumber,
                            x.Desc,
                            x.CreateTime,
                            x.Flag,
                            CompetitionPrograms = list

                        };



                    })

                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常:" + ex.Message,
                    data = new { }

                });
            }
        }


      
    }
}
