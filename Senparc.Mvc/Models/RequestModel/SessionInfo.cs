using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.RequestModel
{
  public  class SessionInfo
    {
        public string UserName { get; set; }
        public string ClientKey { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
