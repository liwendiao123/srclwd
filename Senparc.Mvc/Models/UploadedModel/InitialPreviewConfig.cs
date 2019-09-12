using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.UploadedModel
{

    public class UpLoadResult
    {
        public int code;
        public string curl;

        public UpLoadResult()
        {
            error = string.Empty;
            initialPreview = new List<string>();
            initialPreviewConfig = new List<InitialPreviewConfig>();
            append = true;
            code = -1; curl = string.Empty;
        }
        public string error { get; set; }

        public List<string> initialPreview { get; set; }

        public List<InitialPreviewConfig> initialPreviewConfig { get;set;}

        public bool append { get; set; }

    }

   public class InitialPreviewConfig
    {

        public InitialPreviewConfig()
        {
            caption = string.Empty;
            size = 10200;
            width = "120px";
            url = string.Empty;
            key = "0";
        }
        public string caption { get; set; }

        public long size { get; set; }

        public string width { get; set; }

        public string url { get; set; }
       
        public string key { get; set; }
       

     
    }
}
