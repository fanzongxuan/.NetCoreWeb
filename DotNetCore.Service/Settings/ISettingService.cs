using DotNetCore.Core.Domain.Settings;
using DotNetCore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DotNetCore.Service.Settings
{
    public interface ISettingService : IBaseService<Setting>
    {
        void SaveSetting<T>(T settings) where T : ISetting, new();

        void SetSetting<T>(string key, T value, bool clearCache = true);

        T LoadSetting<T>() where T : ISetting, new();

        bool SettingExists<T, TPropType>(T settings,
         Expression<Func<T, TPropType>> keySelector) where T : ISetting, new();

        IList<Setting> GetAllSettings();

        T GetSettingByKey<T>(string key, T defaultValue = default(T), bool loadSharedValueIfNotFound = false);
        
    }
}
