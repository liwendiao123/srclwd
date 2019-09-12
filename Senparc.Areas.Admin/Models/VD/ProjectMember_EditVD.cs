using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Areas.Admin.Models.VD
{

    public class ProjectMember_BaseVD : BaseAdminVD
    {


    }

    public class ProjectIndex_EditVD : ProjectMember_BaseVD
    {
        public ProjectIndex_EditVD()
        {
            CpList = new List<CompetitionProgram>();
        }
        public PagedList<ProjectMember_EditVD> CompetitionProgramList { get; set; }
        public string kw { get; set; }

        public string ProjectId { get; set; }


        public List<CompetitionProgram> CpList { get; set; }
    }

    public class ProjectMember_EditVD: ProjectMember_BaseVD
    {

        public ProjectMember_EditVD()
        {
            CompetitionPrograms = new List<CompetitionProgram>();
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProjectId { get; set; }


        public List<CompetitionProgram> CompetitionPrograms
        {
            get;
            set;
        }



        public virtual CompetitionProgram CompetitionProgram { get; set; }

        public string IdCard { get; set; }

        public string HeadImgUrl { get; set; }

        public string Duty { get; set; }

        public string Email { get; set; }

        public string IdCardImgUrl { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否为项目负责人
        /// </summary>
        public bool IsLeader { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public bool IsEdit { get; set; }

    }
}
