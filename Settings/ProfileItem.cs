

namespace Settings
{
    internal class ProfileItem
    {
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public string SettingsName { get; set; } = string.Empty;  // make sure this is a valid filename
        public bool Startup { get; set; } = false; 
    }
}
