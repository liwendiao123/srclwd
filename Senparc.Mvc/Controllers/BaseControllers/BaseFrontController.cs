using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Core.Extensions;
using Senparc.Core.Models;
using Senparc.Service;

namespace Senparc.Mvc.Controllers
{
    [UserAuthorize("UserAnonymous")]
    [AllowAnonymous]
    public class BaseFrontController : BaseController
    {

        protected AdminUserInfo VerifySession(string session, AdminUserInfo adminUser, AdminUserInfoService _adminUserInfoService)
        {
            var result = _encryptionService.CommonDecrypt(session);
            if (!string.IsNullOrEmpty(result))
            {
                var arr = result.Split("-");
                if (arr.Length == 3)
                {
                    adminUser = _adminUserInfoService.GetUserInfo(arr[0]);

                }
                else
                {

                }
            }

            return adminUser;
        }

    }
}

