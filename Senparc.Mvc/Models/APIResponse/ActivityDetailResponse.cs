using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
  public  class ActivityDetailResponse:ActivityResponse
    {

        public ActivityDetailResponse()
        {
            content = string.Empty;
        }

        public string content { get; set; }
    }
}
