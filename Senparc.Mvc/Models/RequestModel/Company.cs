using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.RequestModel
{
   public  class CompanyRequest
    {

        public string Id { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
             
        /// <summary>
        /// 公司封面
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Phone { get; set; }


        
  



    }
}
