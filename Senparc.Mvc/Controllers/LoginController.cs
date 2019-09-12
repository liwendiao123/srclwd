using Senparc.Service;
using Senparc.Core.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Senparc.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private AccountService _accountService;
        private WeixinService _weixinService;
        private IQrCodeLoginCache _qrCodeLoginCache;
        public LoginController(WeixinService weixinService, IQrCodeLoginCache qrCodeLoginCache, AccountService accountService)
        {
            _weixinService = weixinService;
            _qrCodeLoginCache = qrCodeLoginCache;
            this._accountService = accountService;
        }
    }
}
