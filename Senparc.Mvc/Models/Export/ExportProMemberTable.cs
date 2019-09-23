using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models
{

    /// <summary>
    /// 导出演出人员表
    /// </summary>
   public  class ExportProMemberTable
    {

        /// <summary>
        /// 序号
        /// </summary>
        /// 
       
        public string Sort { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 演出单位
        /// </summary>

        public string Company { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>

        public string ProjectName { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>

        public string ProjectType { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkCom { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
    }
}
