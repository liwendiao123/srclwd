using Senparc.Mvc.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
  public  class CompanyResponse: CompanyRequest
    {
        public CompanyResponse()
        {
           Id = string.Empty;
        /// <summary>
        /// 公司名称
        /// </summary>
         Name = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
         Desc = string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
         Remark = string.Empty;

        /// <summary>
        /// 公司封面
        /// </summary>
         Img = string.Empty;

        /// <summary>
        /// 联系人
        /// </summary>
         Leader = string.Empty;

        /// <summary>
        /// 联系人电话
        /// </summary>
         Phone = string.Empty;

            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
    }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
