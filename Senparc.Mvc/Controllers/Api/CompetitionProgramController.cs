using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Models;
using Senparc.Mvc.Filter;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Core.Enums;
using Senparc.Mvc.Models.RequestModel;
using Senparc.Core.Models.DataBaseModel;

namespace Senparc.Mvc.Controllers.Api
{

  //  [Session]
    public class CompetitionProgramController : BaseFrontController
    {

        private readonly SenparcEntities _senparcEntities;
        private readonly ScheduleService _scheduleService;
        private readonly CompetitionProgramService _competitionProgramService;
        private readonly ActivityService _activityService;
        private readonly ProjectMemberService _projectMemberService;
        private readonly AdminUserInfoService _adminUserInfoService;
        public CompetitionProgramController(
             CompetitionProgramService competitionProgramService
             , SenparcEntities senparcEntities
            , ActivityService activityService
            , ProjectMemberService projectMemberService
            , ScheduleService scheduleService
            ,AdminUserInfoService adminUserInfoService

           )
        {
            _competitionProgramService = competitionProgramService;
            _senparcEntities = senparcEntities;
            _scheduleService = scheduleService;
            _activityService = activityService;
            _projectMemberService = projectMemberService;
            _adminUserInfoService = adminUserInfoService;
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
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

                var curactivity = _senparcEntities.Activities.Where(x => x.IsPublish).OrderByDescending(x => x.IssueTime).FirstOrDefault();
                // _activityService.GetFullList(x => x.IsPublish, x => x.IssueTime, Core.Enums.OrderingType.Descending).FirstOrDefault();
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
                // var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x.Id);

               var  scheduleList =  _senparcEntities.Schedules.Where(x => x.ActivityId == curactivity.Id).OrderBy(x=>x.StartTime).Select(x=>x.Id).ToList();
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

                var i = 0;
                var cpc = _competitionProgramService.GetFullList(x => scheduleList.Contains(x.ScheduleId) && x.ControlId == adminUser.UserName, x => x.Status, Core.Enums.OrderingType.Ascending)
                    .GroupBy(x => x.Cate).Select(x => new CompetitionProgramViewModel
                    {
                        Cate = x.Key,
                        List = x.Where(c => c.Cate == x.Key).Select(c =>
                        {


                            var addrName = string.Empty;
                            if (c.Schedule == null && !string.IsNullOrEmpty(c.ScheduleId))
                            {
                                var schedule = _scheduleService.GetObject(s => s.Id == c.ScheduleId);

                                if (schedule != null)
                                {
                                    addrName = schedule.Address;
                                }
                            }
                            else if (c.Schedule != null)
                            {
                                addrName = c.Schedule.Address;
                            }
                            var mlist = _projectMemberService.GetFullList(m => m.ProjectId == c.Id, m => m.Sort, OrderingType.Ascending).ToList();

                            return new CompetitionProgramVM
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Address = addrName,
                                SignNum = c.SignNum,
                                Sort = i++,
                                Status = c.Status,
                                BdImgUrl = c.BdImgUrl,
                                BdImgUrlPwd = c.BdImgUrlPwd,
                                IsSign = !string.IsNullOrEmpty(c.SignNum),
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

                //var cpc = _senparcEntities.CompetitionPrograms.Where(x=>scheduleList.Contains(x.ScheduleId) && x.ControlId == adminUser.UserName).OrderBy(x=>x.Status)
                //    .GroupBy(x => x.Cate).Select(x=> {
                //    })

                return Json(new
                {
                    code = 0,
                    msg = "成功获取数据",
                    data = cpc
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常：" + ex.Message,
                    data = new
                    {
                    }
                });
            }
    
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        public ActionResult GetProgramDetail(string programID,string session)
        {
            try
            {
                if (string.IsNullOrEmpty(programID))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "参数不能为空",
                        data = new
                        {
                        }
                    });
                }
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
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x.Id);
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
                var obj = _competitionProgramService.GetObject(x => x.Id == programID);

                if (obj == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "未找到指定项目",
                        data = new
                        {

                        }
                    });
                }

                else
                {

                    var mlist = _projectMemberService.GetFullList(m => m.ProjectId == obj.Id, m => m.Sort, OrderingType.Ascending).ToList();

                   var count = (obj.SignNum ?? "").Split(",").Where(x => string.IsNullOrEmpty(x)).ToList().Count();
                    return Json(new
                    {
                        code = 0,
                        msg = "成功获取数据",
                        data = new   
                        {          
                            Id = obj.Id,
                            Name = obj.Name,
                            Company = obj.Company,
                            obj.Desc,
                            Address = obj.Schedule.Address,
                            ScheduleName = obj.Schedule.Name,
                            StartTime = obj.Schedule.StartTime,
                            TotalSignNum = count,
                            EndTime = obj.Schedule.EndTime,
                            SignNum = obj.SignNum??"",                           
                            Status = obj.Status,
                          
                            IsSign = !string.IsNullOrEmpty(obj.SignNum),
                            ProjectMembers = mlist.Select(mm => new MemberModel
                            {

                                Id = mm.Id ?? "",
                                IdCard = mm.IdCard ?? "",
                                ProjectId = mm.ProjectId ?? "",
                                IdCardImgUrl = mm.IdCardImgUrl ?? "",
                                Name = mm.Name ?? "",
                                Phone = mm.Phone ?? "",
                                Company=   mm.Company,
                                Nation =  mm.Nation,
                                Duty = mm.Duty

                            }).ToList()

                        }
                });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常：" + ex.Message,
                    data = new
                    {
                    }
                });
            }
           
        }

        /// <summary>
        /// 抽签成功
        /// </summary>
        /// <param name="cpcId"></param>
        /// <returns></returns>

        public ActionResult GetSignNum(string cpcId,string session)
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
                var model = _competitionProgramService.GetObject(z => z.Id == cpcId);
                if (model == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "数据异常",
                        data = new
                        {

                        }
                    });
                }

                if (!string.IsNullOrEmpty(model.SignNum))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "您已抽到签",
                        data = new
                        {
                            signnum = model.SignNum
                        }
                    });
                }

                var modellist = _competitionProgramService.GetFullList(x => !string.IsNullOrEmpty(x.SignNum) && x.ScheduleId == model.ScheduleId, x => x.CreateTime, OrderingType.Descending).Select(x => x.SignNum).ToList();

                var numlist = model.Schedule.SignNumber.Split(",");


                List<string> num = new List<string>();

                foreach (var item in numlist)
                {
                    if (!modellist.Contains(item))
                    {
                        num.Add(item);
                    }
                }
                #region 抽签有并发风险------暂未处理 后期处理
                var itemnum = GetRadom(num);

                if (!string.IsNullOrEmpty(itemnum))
                {

                    model.SignNum = itemnum;
                    _competitionProgramService.SaveObject(model);


                    return Json(new
                    {
                        code = 0,
                        msg = "恭喜您抽签成功",
                        data = new
                        {
                            signnum = itemnum
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "抽签失败：主办方未安排签",
                        data = new
                        {
                            signnum = itemnum
                        }
                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常：" + ex.Message,
                    data = new
                    {
                    }
                });
            }


          






        }


        [NonAction]
        private string GetRadom(List<string> items)
        {
            if (items == null)
            {
                items = new List<string>();
            }
            var arr = items.ToArray();
            Random r = new Random();

            int n = r.Next(0, arr.Length - 1);

            return arr[n];
        }

        /// <summary>
        /// 添加项目成员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult AddMember(AddProjectMemberRequest request,string session)
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
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x.Id);
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

                var pm = _projectMemberService.GetObject(x => x.ProjectId == request.ProjectId && x.Phone == request.Phone);


                if (!string.IsNullOrEmpty(request.Id))
                {
                     pm = _projectMemberService.GetObject(x => x.Id == request.Id);

                    if (pm == null)
                    {
                        return Json(new
                        {
                            code = -1,
                            msg = "指定用户不存在",
                            data = new { }

                        });
                    }
                }

                if (pm != null)
                {
                    // pm.Id = Guid.NewGuid().ToString("N");
                    pm.Company = request.Company;
                    pm.Duty = request.Duty;
                    pm.Email = request.Email;
                    //pm.CreateTime = DateTime.Now;
                    pm.Flag = false;
                    pm.Gender = request.Gender;
                    pm.Name = request.Name;
                    pm.Nation = request.Nation;
                    pm.ProjectId = request.ProjectId;
                    pm.Phone = request.Phone;
                    pm.UpdateTime = DateTime.Now;
                    pm.IsLeader = request.IsLeader;
                    pm.IdCard = request.IdCard;
                    pm.Sort = pm.Sort;
                }

                else
                {
                    var count = _projectMemberService.GetFullList(x => x.ProjectId == request.ProjectId && x.Phone == request.Phone, x => x.CreateTime, OrderingType.Descending).Count;

                    pm = new ProjectMember
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Company = request.Company,
                        Duty = request.Duty,
                        Email = request.Email,
                        CreateTime = DateTime.Now,
                        Flag = false,
                        Gender = request.Gender,
                        Name = request.Name,
                        Nation = request.Name,
                        ProjectId = request.ProjectId,
                        Phone = request.Phone,
                        UpdateTime = DateTime.Now,
                        IsLeader = request.IsLeader,
                        IdCard = request.IdCard,
                        Sort = count + 1,
                    };

                }

                _projectMemberService.SaveObject(pm);


                return Json(new
                {
                    code = 0,
                    msg = "添加成功",
                    data = new {
                        pm.Id,
                        pm.IdCard,
                        pm.Name,
                        pm.Phone,
                        pm.ProjectId,
                        pm.IdCardImgUrl
                    }
                });


            }
            catch (Exception ex)
            {

                return Json(new
                {
                    code = -1,
                    msg = "服务器异常", 
                    data = new { }
                });
            }

     
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DelMember(string Id,string session)
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
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x.Id);
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


                if (string.IsNullOrEmpty(Id))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "参数不正确",
                        data = new
                        {
                        }

                    });
                }

                var obj = _projectMemberService.GetObject(x => x.Id == Id);

                if (obj != null)
                {
                    _projectMemberService.DeleteObject(obj);

                    return Json(new
                    {
                        code = 0,
                        msg = "删除成功",
                        data = new
                        {
                        }

                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "当前人员不存在",
                        data = new
                        {
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常",
                    data = new { }
                });
            }
        }


        public ActionResult GetMember(string Id,string session)
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
                        data= new {
                           
                        }
                    });
                }

             var obj =    _projectMemberService.GetObject(x => x.Id == Id);

                if (obj == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "当前人员不存在",
                        data = new
                        {
                           
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "获取成功",
                        data = new {
                            Id = obj.Id ?? "",
                            IdCard = obj.IdCard ?? "",
                            ProjectId = obj.ProjectId ?? "",
                            IdCardImgUrl = obj.IdCardImgUrl ?? "",
                            Name = obj.Name ?? "",
                            Phone = obj.Phone ?? "",
                            Company = obj.Company,
                            Nation = obj.Nation,
                            Duty = obj.Duty
                        }

                    });
                }
  

            }

            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常："+ex.Message ,
                    data = new
                    {
                    }

                });
            }
        }


        /// <summary>
        /// 修改参赛项目信息
        /// </summary>
        /// <param name="projectId">参赛项目 Id</param>
        /// <param name="name">参赛项目 名称</param>
        /// <param name="company"> 参赛项目  演出单位</param>
        /// <param name="desc">参赛项目 舞台技术要求</param>
        /// <param name="session"> 凭证</param>
        /// <returns></returns>
        public ActionResult UpdateProgram(string projectId,string name,string company ,string desc,string session)
        {

            try
            {

                if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(company) || string.IsNullOrEmpty(desc))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "参数格式错误",
                        data = new { }

                    });
                }
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
                var scheduleList = _scheduleService.GetFullList(x => x.ActivityId == curactivity.Id, x => x.StartTime, Core.Enums.OrderingType.Ascending).Select(x => x.Id);
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

              var result =  _competitionProgramService.GetObject(x => x.Id == projectId);

                if (result != null)
                {
                    result.Name = name;
                    result.Company = company;
                    result.Desc = desc;
                    result.UpdateTime = DateTime.Now;

                    _competitionProgramService.SaveObject(result);
                    return Json(new
                    {
                        code = 0,
                        msg = "节目更新成功",
                        data = new
                        {
                        }

                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "当前节目不存在",
                        data = new
                        {
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = "服务器异常：" +ex.Message,
                    data = new
                    {
                    }

                });
            }

          
        }
    }
}
