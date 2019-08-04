using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
  public  class LotteryResponseModel
    {
        public LotteryResponseModel()
        {
            name= string.Empty;
            company_name= string.Empty;
            summary= string.Empty;
            hall= string.Empty;
            time = string.Empty;
            createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

             public string  name{get;set;}
             public string  company_name{get;set;}
             public string  summary{get;set;}
             public string  hall{get;set;}
             public string  time { get; set; }

             public string createtime { get; set; }
            public string updatetime { get; set; }

    }
}
