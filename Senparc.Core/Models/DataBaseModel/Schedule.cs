using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{

    /// <summary>
    /// 赛程管理
    /// </summary>
  public  class Schedule : BaseModel
    {

        public Schedule()
        {
            Activity = new Activity();
            CompetitionPrograms = new List<CompetitionProgram>();
            SignNumber = string.Empty;
            Creator = string.Empty;
            CreatorId = string.Empty;
            Address = string.Empty;
            Id = string.Empty;
            ActivityId = string.Empty;
            Name = string.Empty;
            Remark = string.Empty;
            Desc = string.Empty;
            UpdatorId = string.Empty;
            UpdatorName = string.Empty;
        }
        /// <summary>
        /// 赛程Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 所属活动ID
        /// </summary>
        public string ActivityId { get; set; }


        public virtual Activity Activity { get; set; }

        /// <summary>
        /// 赛程名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 赛程地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 赛程序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 参赛签号
        /// </summary>
        public string SignNumber { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string Desc { get; set; }  
        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 最后一个修改者Id
        /// </summary>
        public string UpdatorId
        {
            get;set;
        }
        /// <summary>
        /// 最后一个修改者姓名
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

        public ICollection<CompetitionProgram> CompetitionPrograms { get; set; }
    }
}
