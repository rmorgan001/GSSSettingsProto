using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Settings
{
    internal static class Server
    {

        public static event PropertyChangedEventHandler StaticPropertyChanged = null!;


        private static bool _canAlignMode;
        public static bool CanAlignMode
        {
            get => _canAlignMode;
            private set
            {
                if (_canAlignMode == value) return;
                _canAlignMode = value;
                LogSetting(MethodBase.GetCurrentMethod().Name, value.ToString());
                OnStaticPropertyChanged();



            }
        }

        /// <summary>
        /// output to session log
        /// </summary>
        /// <param name="method"></param>
        /// <param name="value"></param>
        private static void LogSetting(string method, string value)
        {
            // send to monitor for logging
        }

        /// <summary>
        /// property event notification
        /// </summary>
        /// <param name="propertyName"></param>
        private static void OnStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

    }
}
