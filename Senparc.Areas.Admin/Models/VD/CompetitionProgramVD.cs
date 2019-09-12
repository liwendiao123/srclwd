
using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin.Models.VD
{
 

    public class BaseCompetitionProgramVD : BaseAdminVD
    {


    }

    public class CompetitionProgram_IndexVD : BaseCompetitionProgramVD
    {

        public CompetitionProgram_IndexVD()
        {
            ProjectId = string.Empty;
            kw = string.Empty;
            Schedules = new List<Schedule>();
           
        }
        public PagedList<CompetitionProgram_EditVD> CompetitionProgramList { get; set; }
        public string kw { get; set; }

        public string ProjectId { get; set; }


        public List<Schedule> Schedules {
            get;set;

        }
}
    


 public class CompetitionProgram_EditVD : BaseCompetitionProgramVD
{

        public CompetitionProgram_EditVD()
        {
            CateList = new List<Category>() {

                new Category{
                     Id = 0,
                      Name = "舞蹈"
                },
                 new Category
                 {
                     Id = 1,
                     Name = "歌曲"
                 },
                 new Category{
                     Id = 2,
                     Name = "杂技"
                 }                                 
            };
            ProjectMemberList = new List<ProjectMember>();
            Activities = new List<Activity>();
            Schedules = new List<Schedule>();
            ScheduleSignNum = string.Empty;
            ImgUrl = string.Empty;
            BdImgUrl = string.Empty;
            BdImgUrlPwd = string.Empty;
            Name = string.Empty;
            Remark = string.Empty;
            Desc = string.Empty;
            ControlId = string.Empty;
            Status = 0;
            Company = string.Empty;
            AvalidSignNums = new List<string>();

            UserList = new List<AdminUserInfo>();
        }

        public string ActivityId { get; set; }
        public List<Activity> Activities { get; set; }
        public string ProjectId { get; set; }
        public List<Schedule>  Schedules { get; set; } 

        public List<string> AvalidSignNums { get; set; }

        public List<AdminUserInfo> UserList { get; set; }


        public string ScheduleSignNum { get; set; }
        /// <summary>
        /// 参赛节目唯一Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 所属类别 当前仅有3种类别
        /// </summary>
        public int Cate { get; set; }  
        /// <summary>
        /// 节目封面
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 图片百度链接地址
        /// </summary>
        public string BdImgUrl { get; set; }

        /// <summary>
        /// 图片百度链接地址密码
        /// </summary>
        public string BdImgUrlPwd { get; set; }
        /// <summary>
        /// 节目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 节目带头人Id
        /// </summary>
        public string ControlId { get; set; }


        public List<ProjectMember> ProjectMemberList { get; set; }


        public List<Category> CateList { get; set; }

        /// <summary>
        /// 0为开始报名  1、等待抽签 2、演出中
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 演出单位
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 签号
        /// </summary>
        public string SignNum { get; set; }
        /// <summary>
        ///  创建人Id
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 更新者ID
        /// </summary>
        public string UpdatorId { get; set; }

        /// <summary>
        /// 更新人名称
        /// </summary>
        public string UpdatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        public bool IsEdit { get;  set; }




        public string CategoryName {
            get {

                var cateitem= this.CateList.Find(x => x.Id == Cate);

                if (cateitem != null)
                {
                    return cateitem.Name;
                }

                return "";
            }

        }


        public string LeaderName { get; set; }

        public string LeaderCom { get; set; }

        public string LeaderDuty { get; set; }

        public bool Flag { get; set; }

        public string StatusDesc
        {
            get
            {
                switch (this.Status)
                {
                    case 0:
                        return "开始报名";

                    case 1:
                        return "等待抽签";

                    case 2:
                        return "演出中";


                    default:
                        return "开始报名";
                }

            }
        }

        public string ScheduleName { get; internal set; }
        public string  LeaderNation { get;  set; }

        public string LeaderPhone { get; set; }
        public string LeaderCard { get;  set; }
        public string LeaderEmail { get; internal set; }
    }


    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }
    }

}
