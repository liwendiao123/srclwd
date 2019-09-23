using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models
{

    /// <summary>
    /// 导出项目表格
    /// </summary>
   public class ExportProjectTable
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型名称
        /// </summary>
        public string CateName { get; set; }
        /// <summary>
        /// 签号
        /// </summary>
        public string SignNum { get; set; }            


    }
}
