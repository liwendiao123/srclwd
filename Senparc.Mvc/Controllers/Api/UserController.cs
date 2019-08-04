using Microsoft.AspNetCore.Mvc;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.RequestModel;
using Senparc.Service;
using Senparc.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Controllers
{
  public class UserController : BaseFrontController
    {
        private readonly EncryptionService _encryptionService;
        private readonly QQWry _qqWry;
        public UserController(
                EncryptionService encryptionService,
                QQWry qqWry
            )
        {
            _encryptionService = encryptionService;
            _qqWry = qqWry;
        }

       [Session]
        public IActionResult Common()
        {
            return Json(new
            {
                code = 0,
                msg = "",
                data =true
            });
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var ip = _qqWry.GetCurrentIP();

                if ("sa".Equals(request.UserName) && "sa".Equals(request.Password))
                {
                    string loginInfo = "{0}-{1}-{2}";
                  var result =  string.Format(loginInfo, request.UserName, ip, DateTime.Now.AddDays(30).Ticks);
                    return Json(new
                    {
                        code = 0,
                        msg = "登录成功,",
                        data = _encryptionService.CommonEncrypt(result)
                    });
                }
                return Json(new
                {
                    code = -1,
                    msg = "登录失败；用户名、密码错误",
                    data = ""
                });
            }
            else
            {
                return Json(new
                {
                    code = -1,
                    msg = "登录失败；请输入用户名、密码",
                    data = ""
                });
            }


           
        }

    }
}
