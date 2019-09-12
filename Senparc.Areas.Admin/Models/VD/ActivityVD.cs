using Senparc.Core.Models;
using Senparc.Core.Models.DataBaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Senparc.Areas.Admin.Models.VD
{
   public class BaseActivityVD : BaseAdminVD
    {


    }

    public class Activity_IndexVD:BaseActivityVD
    {
        public PagedList<Activity> ActivityList { get; set; }
        public string kw { get; set; }
    }

    public class Activity_EditVD : BaseActivityVD
    {

        public string Id { get; set; }

        /// <summary>
        /// 活动封面
        /// </summary>
        public string CoverUrl { get; set; }
        /// <summary>
        /// 活动主题
        /// </summary>
        /// 
        [Required(ErrorMessage = "请输入活动名称")]
        [MaxLength(200, ErrorMessage = "真实姓名不能超过200个字符")]
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

        public bool IsEdit { get; set; }
    }
}
