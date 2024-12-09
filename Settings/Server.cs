using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Settings
{
    internal static class Server
    {
        #region Const, Events, Backers

        private const string Classname = nameof(Server);
        internal static event PropertyChangedEventHandler? StaticPropertyChanged;
        private static bool _setting1 = true;
        private static int _setting2 = 1;
        private static string _setting3 = "Username";
        private static double _setting4 = 0.1;
        private static string _setting5 = "Password";

        #endregion

        #region Properties

        public static bool Setting1
        {
            get => _setting1;
            set
            {
                if (_setting1 == value) return;
                Profile.SetSettingValue("Setting1", value.ToString(CultureInfo.InvariantCulture));
                _setting1 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        public static int Setting2
        {
            get => _setting2;
            set
            {
                if (_setting2 == value) return;
                Profile.SetSettingValue("Setting2", value.ToString(CultureInfo.InvariantCulture));
                _setting2 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        public static string Setting3
        {
            get => _setting3;
            set
            {
                if (_setting3 == value) return;
                Profile.SetSettingValue("Setting3", value.ToString(CultureInfo.InvariantCulture));
                _setting3 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        public static double Setting4
        {
            get => _setting4;
            set
            {
                if (Math.Abs(_setting4 - value) < 0.1) return;
                Profile.SetSettingValue("Setting4", value.ToString(CultureInfo.InvariantCulture));
                _setting4 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        public static string Setting5
        {
            get => _setting5;
            set
            {
                if (_setting5 == value) return;
                Profile.SetSettingValue("Setting5", value.ToString(CultureInfo.InvariantCulture));
                _setting5 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Add or replace properties and their default values in the Profile SettingsList.  Used for initial load and resets 
        /// </summary>
        internal static void LoadDefaults()
        {
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting1", Value = _setting1.ToString(CultureInfo.InvariantCulture), TypeCode = _setting1.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting2", Value = _setting2.ToString(CultureInfo.InvariantCulture), TypeCode = _setting2.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting3", Value = _setting3.ToString(CultureInfo.InvariantCulture), TypeCode = _setting3.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting4", Value = _setting4.ToString(CultureInfo.InvariantCulture), TypeCode = _setting4.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting5", Value = _setting5.ToString(CultureInfo.InvariantCulture), TypeCode = _setting5.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
        }

        /// <summary>
        /// Update local properties from the Profile.SettingsList and trigger property changes
        /// </summary>
        internal static void UpdateProperties()
        {
            Setting1 = Utils.ConvertString<bool>(Profile.GetSettingItem("Setting1").Value);
            Setting2 = Utils.ConvertString<int>(Profile.GetSettingItem("Setting2").Value);
            Setting3 = Utils.ConvertString<string>(Profile.GetSettingItem("Setting3").Value);
            Setting4 = Utils.ConvertString<double>(Profile.GetSettingItem("Setting4").Value);
            Setting5 = Utils.ConvertString<string>(Profile.GetSettingItem("Setting5").Value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// output to session log
        /// </summary>
        /// <param name="method"></param>
        /// <param name="value"></param>
        private static void LogSetting(string? method, string value)
        {
            // send to monitor for logging
            //var monitorItem = new MonitorEntry
            //    { Datetime = Principles.HiResDateTime.UtcNow, Device = MonitorDevice.Server, Category = MonitorCategory.Server, Type = MonitorType.Information, Method = $"{method}", Thread = Thread.CurrentThread.ManagedThreadId, Message = $"{value}" };
            //MonitorLog.LogToMonitor(monitorItem);
        }

        /// <summary>
        /// property event notification
        /// </summary>
        /// <param name="propertyName"></param>
        private static void OnStaticPropertyChanged([CallerMemberName] string propertyName = "blank")
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
