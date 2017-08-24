using Microsoft.AspNetCore.Mvc;
using DotNetCore.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using DotNetCore.Core.Domain.UserInfos;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.Core.Extensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private SignInManager<Account> _signManager;
        private UserManager<Account> _userManager;
        public AccountController(UserManager<Account> userManager,
            SignInManager<Account> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var user = new Account { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ErrorNotification(string.Join("|", result.Errors.Select(x => x.Description)));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "home");

            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username,
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
                ErrorNotification("用户名或密码错误！");
            }
            else
            {
                ErrorNotification(string.Join("|", ModelState.Errors()));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
