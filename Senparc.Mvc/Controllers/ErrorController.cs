﻿using Senparc.Core.Models.VD;
using Microsoft.AspNetCore.Mvc;

namespace Senparc.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            Error_ExceptionVD vd = new Error_ExceptionVD()
            {
                //COCONET 暂时隐藏
                //HandleErrorInfo = new HandleErrorInfo(new Exception("发生未知错误，请联系客服！"), "Error", "Error")
            };
            return View(vd);
        }

        public ActionResult Error404(string aspxerrorpath)
        {
            Error_Error404VD vd = new Error_Error404VD()
            {
                Url = aspxerrorpath
            };
            return View(vd);
        }
    }
}