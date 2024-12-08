using Settings;
using System.Globalization;
using System.Reflection;

namespace GSSSettingsProto.Models
{
    internal class User 
    {
        public string? UserName
        {
            get => Server.Setting3;
            set
            {
                if (value != null) Server.Setting3 = value;
            }
        }

        public string? Password
        {
            get => Server.Setting5;
            set
            {
                if (value != null) Server.Setting5 = value;
            }
        }
    }
}
