using Senparc.Core.Models.VD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.RequestModel
{
   public class LotteryRequestModel : Base_PagerVD
    {
        public LotteryRequestModel(int pageIndex, int pageCount, int totalCount)
           : base(pageIndex, pageCount, totalCount)
        {
        }
        public LotteryRequestModel() : base(1, 20, 0)
        {

        }
        public string program_id { get; set; }
    }
}
