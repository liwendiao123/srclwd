using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Core.Models.DataBaseModel
{

    [Serializable]
   public class Activity : BaseModel
    {

        public Activity()
        {
            Schedules = new List<Schedule>();
            CoverUrl = string.Empty;
            Summary = string.Empty;
            Content = string.Empty;
            Description = string.Empty;
            Title = string.Empty;
             
        }
        public string Id { get; set; }

        /// <summary>
        /// 活动封面
        /// </summary>
        public string CoverUrl { get; set; }
        /// <summary>
        /// 活动主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 活动内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否为当前活动
        /// </summary>
        public bool IsPublish { get; set; }

        public int ScheduleStatus { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        //public int ScheduleStatus { get; set; }
        public ICollection<Schedule> Schedules
        {
            get;
            set;
        }
    }
}
