
using System.IO;
using System.Text.Json;

namespace Settings
{
    internal static class Profile
    {
        #region Const, Events, Backers

        private const string JsonExt = ".json";
        private const string SettingsPath = "GS_Server\\Settings";
        private const string Default = "Default";
        private const string ProfileFileName = "Profiles";
        internal static List<ProfileItem>? ProfilesList = [];
        internal static List<SettingItem>? SettingsList = [];
        private static readonly string LocalSettingsPath;
        private static readonly ProfileItem DefaultProfile;
        private static ProfileItem _activeProfile;

        #endregion

        static Profile()
        {
            DefaultProfile = new ProfileItem { Name = Default, Description = Default, SettingsName = Default, Startup = true }; // default profile
            ProfilesList?.Add(DefaultProfile);
            _activeProfile = DefaultProfile;
            LocalSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,Environment.SpecialFolderOption.Create), SettingsPath);
            if (!Directory.Exists(LocalSettingsPath)){ Directory.CreateDirectory(LocalSettingsPath); }

        }

        #region Internal Methods

       /// <summary>
        /// Add a ProfileItem to ProfilesList
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        /// <param name="replace">replace if found</param>
        internal static void AddProfile(ProfileItem profile, bool replace = true)
        {
            if (string.Equals(profile.Name, DefaultProfile.Name, StringComparison.CurrentCultureIgnoreCase)){ return; }
            ProfilesList ??= [];
            var index = ProfilesList.FindIndex(item => string.Equals(item.Name, profile.Name, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1) { ProfilesList[index] = profile; }
            else { if (replace){ ProfilesList.Add(profile); } }
            SortProfilesList();
        }

        /// <summary>
        /// Add or replace a SettingItem to the SettingsList
        /// </summary>
        /// <param name="setting">SettingItem</param>
        /// <param name="replace">Replace SettingItem in SettingsList</param>
        internal static void AddSetting(SettingItem setting, bool replace = true)
        {
            SettingsList ??= [];
            var index = SettingsList.FindIndex(item => string.Equals(item.Name, setting.Name, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1) { SettingsList[index] = setting; }
            else { if (replace){ SettingsList.Add(setting); } }
            SortSettingsList();
        }

        /// <summary>
        /// Delete a Profile and its corresponding settings file
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        /// <param name="remove">delete corresponding settings file</param>
        internal static void DeleteProfile(ProfileItem profile, bool remove)
        {
            if (string.Equals(profile.Name, DefaultProfile.Name, StringComparison.CurrentCultureIgnoreCase)){ return; }     // can't delete default profile
            if ( profile == _activeProfile) { return; }                                                                         // can't delete active profile
            ProfilesList ??= [];
            var index = ProfilesList.FindIndex(item => string.Equals(item.Name, item.Name, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1) {ProfilesList.Remove(profile);}
            if (remove) { DeleteSettingsFile(profile); }
            SortProfilesList();
        }

        /// <summary>
        /// Delete a Settings file
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        internal static void DeleteSettingsFile(ProfileItem profile)
        {
            var filename = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
            if (File.Exists(filename)){ File.Delete(filename); }
        }

        /// <summary>
        /// retrieve a ProfileItem in the ProfilesList by name
        /// </summary>
        /// <param name="name">Name of a Profile</param>
        /// <returns>a specific ProfileItem from ProfilesList or null if not found</returns>
        internal static ProfileItem? GetProfile(string name)
        {
            ProfilesList ??= [];
            var index = ProfilesList.FindIndex(item => string.Equals(item.Name, name, StringComparison.CurrentCultureIgnoreCase));
            return index != -1 ? null : ProfilesList[index];
        }

        /// <summary>
        /// Get all Profiles
        /// </summary>
        /// <returns>returns ProfilesList</returns>
        internal static List<ProfileItem>? GetProfiles()
        {
            return ProfilesList; // return ProfilesList
        }

        /// <summary>
        /// Retrieve a SettingItem from SettingsList by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static SettingItem GetSettingItem(string name)
        {
            if (SettingsList == null) throw new Exception("SettingsList not found");
            var index = SettingsList.FindIndex(item => string.Equals(item.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1){ return SettingsList[index]; }
            throw new Exception("Setting not found");
        }
        
        /// <summary>
        /// Set the value of a SettingItem in SettingsList
        /// </summary>
        /// <param name="name">Setting name</param>
        /// <param name="value">Setting Value</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static void SetSettingValue(string name, string value)
        {
            if (SettingsList == null) throw new Exception("SettingsList not found");
            var index = SettingsList.FindIndex(item => string.Equals(item.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (index != -1){ SettingsList[index].Value = value ;}
            SaveProfileAsync(_activeProfile);
        }

        /// <summary>
        /// Get all current Settings
        /// </summary>
        /// <returns>returns ProfilesList</returns>
        internal static List<SettingItem>? GetSettings()
        {
            return SettingsList; // return ProfilesList
        }
        
       /// <summary>
        /// Load default settings from the other classes
        /// </summary>
        internal static void LoadDefaults()
        {
            // Base settings
            SettingsList = new List<SettingItem>();
            Server.LoadDefaults();                      //  load default settings from each class
            SortSettingsList();

            // Profiles
            LoadProfilesFile();
            LoadProfile(DefaultProfile);                //  Overwrite defaults with settings file
            SaveProfileAsync(DefaultProfile);
        }

        /// <summary>
        /// Load a specific profile and its settings file
        /// </summary>
        /// <param name="profile"></param>
        internal static void LoadProfile(ProfileItem profile)
        {
            var newSettings = GetSettingsFile(profile);
            if (newSettings == null){ return; }
            if (ProfilesList == null || !ProfilesList.Contains(profile)) return;
            _activeProfile = profile;
            SortProfilesList();

            foreach (var setting in newSettings)  // replace the setting item in SettingsList or add it
            {
                if (SettingsList == null) continue;
                var index = SettingsList.FindIndex(item => string.Equals(item.Name, setting.Name, StringComparison.CurrentCultureIgnoreCase));
                if (index != -1)
                {
                    SettingsList[index] = setting;
                }
                else
                {
                    SettingsList.Add(setting);
                }
            }
            SortSettingsList();
            UpdateAllClassProperties();
        }

        /// <summary>
        /// Get a settings file and return as a sorted list
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>List of SettingItems</returns>
        private static List<SettingItem>? GetSettingsFile(ProfileItem profile)
        {
            var filename = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
            if (!File.Exists(filename)) return null;

            var sList = new List<SettingItem>();
            var fileStream  = new FileStream(filename,FileMode.Open);
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    var pItem = JsonSerializer.Deserialize<SettingItem>(line);
                    if (pItem != null) sList.Add(pItem);
                }
            }
            var sortedList = sList.OrderBy(o => o.Class).ThenBy(o => o.Name).ToList();
            return sortedList;
        }

        /// <summary>
        /// Load Profiles.json file.
        /// </summary>
        private static void LoadProfilesFile()
        {
            //load profiles into ProfilesList
            var path = Path.Combine(LocalSettingsPath, ProfileFileName, JsonExt);
            if (File.Exists(path))
            {
                var pList = new List<ProfileItem>();
                var lines = File.ReadLines(path);
                foreach (var line in lines)
                {
                    var pItem = JsonSerializer.Deserialize<ProfileItem>(line);
                    if (pItem != null) pList.Add(pItem);
                }
                ProfilesList = pList;
            } 
            else
            {
                SaveProfilesFile();     // save what's in ProfilesList
            }
        }

        /// <summary>
        /// Save Profiles and Settings to file as a task
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        /// <param name="profiles">Save Profiles File</param>
        /// <param name="settings">Save Settings File</param>
        /// <returns></returns>
        internal static Task SaveProfileAsync(ProfileItem profile,bool profiles = true, bool settings = true)
        {
            if (profiles)
            {
                SortProfilesList();
                SaveProfilesFile();
            }

            if (settings)
            {
                SortSettingsList();
                SaveSettingsFile(profile);
            }
            return Task.CompletedTask;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Save Profiles to file Profiles.json
        /// </summary>
        private static void SaveProfilesFile()
        {
            if (ProfilesList == null) return;
            var list = ProfilesList;
            var filename = Path.Combine(LocalSettingsPath, ProfileFileName + JsonExt);
            var fileStream  = new FileStream(filename,FileMode.Create);
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var profile in list)
                {
                    var line = JsonSerializer.Serialize(profile);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Save SettingsList for a Profile to file in LocalApplicationData
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        private static void SaveSettingsFile(ProfileItem profile)
        {
            if (SettingsList == null) return;
            var list = SettingsList;
            var filename = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
            var fileStream = new FileStream(filename, FileMode.Create);
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var setting in list)
                {
                    var line = JsonSerializer.Serialize(setting);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Update class properties
        /// </summary>
        private static void UpdateAllClassProperties()
        {
            Server.UpdateProperties();
        }

        /// <summary>
        /// Sort SettingsList by class and then name
        /// </summary>
        private static void SortSettingsList()
        {
            if (SettingsList == null){ return; }
            var sortedList = SettingsList.OrderBy(o => o.Class).ThenBy(o => o.Name).ToList();
            SettingsList = sortedList;
        }

        /// <summary>
        /// Sort ProfilesList by name
        /// </summary>
        private static void SortProfilesList()
        {
            if (ProfilesList == null){ return; }
            var sortedList = ProfilesList.OrderBy(o => o.Name).ToList();
            ProfilesList = sortedList;
        }

        #endregion
    }
}
