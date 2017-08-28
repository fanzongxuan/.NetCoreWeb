using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Service.Settings;
using DotNetCore.Core.Domain.UserInfos;
using DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper;
using DotNetCore.Web.Areas.Admin.Models.Setting;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IActionResult AuthorizeSettings()
        {
            var authorizeSettings = _settingService.LoadSetting<AuthorizeSettings>();
            var model = authorizeSettings.ToModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AuthorizeSettings(AuthorizeSettingsModel model)
        {
            var entity = model.ToEntity();
            _settingService.SaveSetting(entity);
            return View(model);
        }
    }
}