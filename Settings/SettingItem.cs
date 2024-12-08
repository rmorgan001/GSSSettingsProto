

namespace Settings
{
    internal class SettingItem
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string TypeCode { get; set; } = string.Empty;            // simple type keywords
        public string Class { get; set; } = string.Empty;               // location of property
        public string VersionAdded { get; set; } = string.Empty;
        public bool IsDepreciated { get; set; } = false;         
        public string VersionDepreciated { get; set; } = string.Empty;
    }

}
