using Microsoft.AspNetCore.Mvc;
using Senparc.Mvc.Filter;
using Senparc.Mvc.Models.APIResponse;
using Senparc.Mvc.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Mvc.Controllers
{

    [Session]
    public  class CompanyController : BaseFrontController
    {

        public CompanyController()
        {

        }


        [HttpPost]
        public IActionResult Submit(CompanyRequest request)
        {
            return Json(new
            {
                code = 0,
                msg = "提交成功",
                data = new CompanyResponse()
            });
        }

    }
}
