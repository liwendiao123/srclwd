using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
   public class LotteryDetailResponseModel: LotteryResponseModel
    {

        public LotteryDetailResponseModel()
        {
            leader = string.Empty;
            number = string.Empty;
            phone = string.Empty;
        }
                   public string    leader{ get; set; }
                   public string    number { get; set; }
                   public string phone { get; set; }

                    

    }
}
