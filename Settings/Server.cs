using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Settings
{
    internal static class Server
    {
        private const string Classname = nameof(Server);
        internal static event PropertyChangedEventHandler StaticPropertyChanged = null!;

        #region Defaults

        private static bool _setting1 = true;
        private static int _setting2 = 1;
        private static string _setting3 = "Test1";
        private static double _setting4 = 0.1;


        #endregion

        #region Properties

        public static bool Setting1
        {
            get => _setting1;
            set
            {
                if (_setting1 == value) return;
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
                _setting4 = value;
                LogSetting(MethodBase.GetCurrentMethod()?.Name, $"{value}");
                OnStaticPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load properties and their default values into the Profile SettingsList.
        /// </summary>
        internal static void LoadDefaults()
        {
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting1", Value = _setting1.ToString(), TypeCode = _setting1.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting2", Value = _setting2.ToString(), TypeCode = _setting2.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting3", Value = _setting3, TypeCode = _setting3.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
            Profile.AddSetting(new SettingItem
            {
                Name = "Setting4", Value = _setting4.ToString(CultureInfo.InvariantCulture), TypeCode = _setting4.GetTypeCode().ToString(), Class = Classname, VersionAdded = "1.0"
            });
        }
        
        /// <summary>
        /// output to session log
        /// </summary>
        /// <param name="method"></param>
        /// <param name="value"></param>
        private static void LogSetting(string?  method, string value)
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
        private static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)){ return; }
            StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
