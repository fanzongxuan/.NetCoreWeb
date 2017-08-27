using System;
using System.Collections.Generic;
using System.Text;
using DotNetCore.Core.Domain.Settings;
using DotNetCore.Core.Interface;
using DotNetCore.Service.Events;
using DotNetCore.Core.Cache;
using System.Linq;
using System.Linq.Expressions;
using DotNetCore.Core;
using System.ComponentModel;

namespace DotNetCore.Service.Settings
{
    public class SettingService : ISettingService
    {

        private const string SETTINGS_ALL_KEY = "DotNetWeb.setting.all";
        private const string SETTINGS_PATTERN_KEY = "DotNetWeb.setting.";

        private readonly IRepository<Setting> _settingRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        public SettingService(IRepository<Setting> settingRepository,
            IEventPublisher eventPublisher,
            ICacheManager cacheManager
            )
        {
            _settingRepository = settingRepository;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
        }

        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            //cache
            string key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
                var query = from s in _settingRepository.TableNoTracking
                            orderby s.Name
                            select s;
                var settings = query.ToList();
                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }

        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T), bool loadSharedValueIfNotFound = false)
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                if (setting == null && loadSharedValueIfNotFound)
                    setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        public void Delete(Setting entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("setting");
            _settingRepository.Delete(entitiy);

            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(entitiy);
        }

        public Setting GetById(int id)
        {
            if (id == 0)
                return null;
            return _settingRepository.GetById(id);
        }

        public IPagedList<Setting> GetListPageable(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _settingRepository.Table;
            return new PagedList<Setting>(query, pageIndex, pageSize);
        }

        public void Insert(Setting entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("Setting");

            _settingRepository.Insert(entitiy);
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            _eventPublisher.EntityInserted(entitiy);
        }

        public void Update(Setting entitiy)
        {
            if (entitiy == null)
                throw new ArgumentNullException("Setting");
            _settingRepository.Update(entitiy);

            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(entitiy);
        }

        public virtual IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.Table
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            return settings;
        }

        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _settingRepository.Delete(settings);

            //cache
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            //event notification
            foreach (var setting in settings)
            {
                _eventPublisher.EntityDeleted(setting);
            }
        }

        public virtual bool SettingExists<T, TPropType>(T settings,
         Expression<Func<T, TPropType>> keySelector)
         where T : ISetting, new()
        {
            string key = settings.GetSettingKey(keySelector);

            var setting = GetSettingByKey<string>(key);
            return setting != null;
        }

        public virtual T LoadSetting<T>() where T : ISetting, new()
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //load by store
                var setting = GetSettingByKey<string>(key, loadSharedValueIfNotFound: true);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        public virtual void SetSetting<T>(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();
            string valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetById(settingForCaching.Id);
                setting.Value = valueStr;
                Update(setting);
            }
            else
            {
                //insert
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr,
                };
                Insert(setting);
            }
        }

        public virtual void SaveSetting<T>(T settings) where T : ISetting, new()
        {
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(key, value);
                else
                    SetSetting(key, "");
            }

        }
        
        public virtual void DeleteSetting<T>() where T : ISetting, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }

        public virtual void DeleteSetting<T, TPropType>(T settings,
          Expression<Func<T, TPropType>> keySelector) where T : ISetting, new()
        {
            string key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetById(settingForCaching.Id);
                Delete(setting);
            }
        }

        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }


        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
