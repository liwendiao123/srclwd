using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Models.APIResponse
{
    public class ActivityResponse
    {
        public ActivityResponse()
        {
            cover_url    = string.Empty;
            title        = string.Empty;
            summary      =string.Empty; 
            description  =string.Empty;
            issue_time = string.Empty;
        }



        public string     cover_url      {get;set;}
        public string     title         {get;set;}
        public string     summary      {get;set;}
        public string     description   {get;set;}
        public string     issue_time   {get;set;}

    }
}
