using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using DotNetCore.Core.Domain.Accounts;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.Core.Extensions;
using DotNetCore.Service.Accounts;
using DotNetCore.Framework.Mvc.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var user = new Account { UserName = model.Username };
                var result = _accountService.Register(user, model.Password);

                if (result.Succeeded)
                {
                    _accountService.LoginWithUserNameAndPwd(model.Username, model.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ErrorNotification(string.Join("|", result.Errors.Select(x => x.Description)));
                }
            }
            else
            {
                ErrorNotification(string.Join("|", ModelState.Errors()));
            }

            var authModel = new AuthModel()
            {
                RegisterModel = model
            };

            return RedirectToAction("Login", authModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "", AuthModel authModel = null)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (authModel == null)
            {
                authModel = new AuthModel();
                authModel.LoginModel.ReturnUrl = returnUrl;
            }

            return View(authModel);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var result = _accountService.LoginWithUserNameAndPwd(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ErrorNotification("User name or password error!");
            }
            else
            {
                ErrorNotification(string.Join("|", ModelState.Errors()));
            }

            var authModel = new AuthModel()
            {
                LoginModel = model
            };

            return View(authModel);
        }

        [Authorize]
        public IActionResult Logout()
        {
            _accountService.LoginOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
