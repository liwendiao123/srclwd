using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{

   /// <summary>
   /// 参赛节目
   /// </summary>
   public class CompetitionProgram:BaseModel
    {

        public CompetitionProgram()
        {
            Name = string.Empty;
            Company = string.Empty;
            SignNum = string.Empty;
            Remark = string.Empty;
            Desc = string.Empty;
            Id = string.Empty;
            ScheduleId = string.Empty;
            ImgUrl = string.Empty;
            BdImgUrl = string.Empty;
            BdImgUrlPwd = string.Empty;
            ControlId = string.Empty;
            CreatorId = string.Empty;
            CreatorName = string.Empty;
            UpdatorId = string.Empty;
            UpdatorName = string.Empty;
            ProjectMembers = new List<ProjectMember>();

        }
        /// <summary>
        /// 参赛节目唯一Id
        /// </summary>
        public string Id { get; set; }


        public string ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }
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
        ///  签号
        /// </summary>
        public string SignNum { get; set; }

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

        /// <summary>
        /// 0为开始报名  1、等待抽签 2、演出中
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 演出单位
        /// </summary>
        public string Company { get; set; }
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


        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
