using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Areas.Admin.Models.VD
{
    public class ScheduleVD
    {
    }

    public class BaseScheduleVD : BaseAdminVD
    {


    }

    public class Schedule_IndexVD:BaseScheduleVD
    {
        public PagedList<Schedule> ScheduleList { get; set; }
        public string kw { get; set; }
    }

    public class Schedule_EditVD : BaseActivityVD
    {

        public Schedule_EditVD()
        {
            Activity = new Activity();
            Name = string.Empty;
            Address = string.Empty;

            ActivityList = new List<Activity>();

        }
        /// <summary>
        /// 赛程Id
        /// </summary>
        public string Id { get; set; }

        public List<Activity> ActivityList { get; set; }
        /// <summary>
        /// 所属活动ID
        /// </summary>
        /// 
        [Required]
        public string ActivityId { get; set; }


        public virtual Activity Activity { get; set; }

        /// <summary>
        /// 赛程名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 赛程地址
        /// </summary>
        /// 
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// 赛程序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// 
        [Required]
        public DateTime StartTime { get; set; }


        public DateTime EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 参赛签号
        /// </summary>
        /// 
        [Required]
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
            get; set;
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

        public bool IsEdit { get; set; }

        public bool Flag { get; set; }
    }

}
