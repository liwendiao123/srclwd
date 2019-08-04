using Senparc.Core.Models.VD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.RequestModel
{
   public class ActivityRequestModel: Base_PagerVD
    {
        public ActivityRequestModel(int pageIndex, int pageCount, int totalCount) 
            : base(pageIndex, pageCount, totalCount)
        {
        }
        public ActivityRequestModel() : base(1, 20,0)
        {

        }
        public string article_id { get; set; }
    }
}
