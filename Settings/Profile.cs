
using System.IO;
using System.Text.Json;

namespace Settings
{
    internal static class Profile
    {
        private const string JsonExt = ".json";
        private const string FilesFolder = "GS_Server\\Settings";
        private const string Default = "Default";
        private const string ProfileFileName = "Profiles";
        internal static List<ProfileItem>? ProfilesList = [];
        internal static List<SettingItem>? SettingsList = [];
        private static readonly string LocalSettingsPath;
        private static readonly ProfileItem DefaultProfile;
        private static ProfileItem _activeProfile;

        static Profile()
        {
            DefaultProfile = new ProfileItem { Name = Default, Description = Default, SettingsName = Default, Startup = true }; // default profile
            ProfilesList?.Add(DefaultProfile);
            _activeProfile = DefaultProfile;
            LocalSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,Environment.SpecialFolderOption.Create), FilesFolder);
            if (!Directory.Exists(LocalSettingsPath)){ Directory.CreateDirectory(LocalSettingsPath); }

        }

        /// <summary>
        /// Add a ProfileItem to ProfilesList
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        internal static void AddProfile(ProfileItem profile)
        {
            if (string.Equals(profile.Name, DefaultProfile.Name, StringComparison.CurrentCultureIgnoreCase)){ return; }     // can't add Default profile
            var found = ProfilesList != null && ProfilesList.Any(item => item.Name == profile.Name);
            if (!found){ProfilesList?.Add(profile);}
        }

        /// <summary>
        /// Add a SettingItem to the SettingsList
        /// </summary>
        /// <param name="setting">SettingItem</param>
        internal static void AddSetting(SettingItem setting)
        {
            var found = SettingsList != null && SettingsList.Any(item => string.Equals(item.Name, setting.Name, StringComparison.CurrentCultureIgnoreCase));
            if (!found){SettingsList?.Add(setting);}
        }

        /// <summary>
        /// Delete a Profile and its corresponding settings file
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        /// <param name="removeSettingsFile">delete corresponding settings file</param>
        internal static void DeleteProfile(ProfileItem profile, bool removeSettingsFile)
        {
            if (string.Equals(profile.Name, DefaultProfile.Name, StringComparison.CurrentCultureIgnoreCase)){ return; }     // can't delete default profile
            if ( profile == _activeProfile) { return; }                                                                           // can't delete active profile
            var found = ProfilesList != null && ProfilesList.Any(item => string.Equals(item.Name, profile.Name, StringComparison.CurrentCultureIgnoreCase));
            if (!found){ProfilesList?.Remove(profile);}
            if (removeSettingsFile) { DeleteSettingsFile(profile); }
        }

        /// <summary>
        /// Delete Settings file
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        internal static void DeleteSettingsFile(ProfileItem profile)
        {
            var filename = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
            if (File.Exists(filename)){ File.Delete(filename); }
        }

        /// <summary>
        /// Find a ProfileItem in the ProfilesList
        /// </summary>
        /// <param name="profileName">Name of Profile</param>
        /// <returns>a specific ProfileItem from ProfilesList</returns>
        internal static ProfileItem? GetProfile(string profileName)
        {
            var a = ProfilesList?.FirstOrDefault(item =>  string.Equals(item.Name, profileName, StringComparison.CurrentCultureIgnoreCase)); // find in ProfilesList
            return a;
        }

        /// <summary>
        /// Get all Profiles
        /// </summary>
        /// <returns>returns ProfilesList</returns>
        internal static List<ProfileItem>? GetProfiles()
        {
            // return ProfilesList
            return ProfilesList;
        }

        /// <summary>
        /// Get all Settings
        /// </summary>
        /// <returns>returns ProfilesList</returns>
        internal static List<SettingItem>? GetSettings()
        {
            // return ProfilesList
            return SettingsList;
        }
        
        internal static void Load()
        {
            LoadDefaults();
            LoadProfile(DefaultProfile);

            SaveSettingsFile(DefaultProfile);
        }
        
        /// <summary>
        /// Load default settings from the other classes
        /// </summary>
        private static void LoadDefaults()
        {
            SettingsList = new List<SettingItem>();

            Server.LoadDefaults();          //  load default settings from each class

            var sortedList = SettingsList.OrderBy(o => o.Class).ThenBy(o => o.Name).ToList();
            SettingsList = sortedList;
        }

        /// <summary>
        /// Load a specific profile
        /// </summary>
        /// <param name="profile"></param>
        internal static void LoadProfile(ProfileItem profile)
        {
            var newSettings = LoadSettingsFile(profile);
            if (newSettings == null){ return; }
            if (ProfilesList == null || !ProfilesList.Contains(profile)) return;

            _activeProfile = profile;

            foreach (var setting in newSettings)
            {
                if (SettingsList == null) continue;
                var index = SettingsList.FindIndex(item => string.Equals(item.Name, setting.Name, StringComparison.CurrentCultureIgnoreCase));
                if (index != -1)
                {
                    SettingsList[index] = setting;
                }
            }
        }

        /// <summary>
        /// Load the setting file and return a list
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>List of SettingItems</returns>
        private static List<SettingItem>? LoadSettingsFile(ProfileItem profile)
        {
           var path = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
           if (!File.Exists(path)) return null;

           //var sList = new List<SettingItem>();
           var lines = new List<string?>();

           using (var reader = new StreamReader(path))
           {
               while(!reader.EndOfStream)
               {
                   lines.Add(reader.ReadLine());
                   //var line = reader.ReadLine();
                   //if (line == null) continue;
                   //var pItem = JsonSerializer.Deserialize<SettingItem>(line);
                   //if (pItem != null) sList.Add(pItem);

               }
           }

           //var lines = File.ReadLines(path);
           //foreach (var line in lines)
           //{
           //    var pItem = JsonSerializer.Deserialize<SettingItem>(line);
           //    if (pItem != null) sList.Add(pItem);
           //}

           //var sortedList = sList.OrderBy(o => o.Class).ThenBy(o => o.Name).ToList();
          //SettingsList = sortedList;
           return SettingsList;
        }

        /// <summary>
        /// Load Profiles.json file.
        /// </summary>
        private static void LoadProfiles(ProfileItem? profile = null)
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




            //var p = Directory.Exists(LocalSettingsPath);
            //if (p)
            //{
            //    return;
            //}


            //using var r = new StreamReader("file.json");
            //var json = r.ReadToEnd();
            //ProfilesList = JsonSerializer.Deserialize<List<ProfileItem>>(json);

            DeleteSettingsFile(DefaultProfile);

            SaveSettingsFile(DefaultProfile);

            //SaveProfilesFile();

        }

        /// <summary>
        /// Save Profiles to file Profiles.json
        /// </summary>
        private static void SaveProfilesFile()
        {
            if (ProfilesList == null) return;

            var profile = string.Empty;
            foreach (var item in ProfilesList)
            {
                var a = JsonSerializer.Serialize(item);
                profile += a; // one profile per line
            }
            
            var filename = Path.Combine(LocalSettingsPath, ProfileFileName + JsonExt);
            var fileStream  = new FileStream(filename,FileMode.Create);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(profile);
            }
        }

        /// <summary>
        /// Save SettingsList for a Profile to file in LocalApplicationData
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        private static void SaveSettingsFile(ProfileItem profile)
        {
            if (SettingsList == null) return;

            var settings = string.Empty;
            foreach (var setting in SettingsList)
            {
                var a = JsonSerializer.Serialize(setting);
                settings += a + Environment.NewLine; // one setting per line
            }
            
            var filename = Path.Combine(LocalSettingsPath, profile.SettingsName + JsonExt);
            var fileStream  = new FileStream(filename,FileMode.Create);
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(settings);
            }
        }

        /// <summary>
        /// Replace a ProfileItem in ProfilesList by name
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        private static void ReplaceProfile(ProfileItem profile)
        {
            if (string.Equals(profile.Name, DefaultProfile.Name, StringComparison.CurrentCultureIgnoreCase)){ return; }     // can't replace default profile
            var found = ProfilesList != null && ProfilesList.Any(item => string.Equals(item.Name, profile.Name, StringComparison.CurrentCultureIgnoreCase));
            if (found) return;

            DeleteProfile(profile, false);
            AddProfile(profile);
            // what to do with settings name and file?
        }

        /// <summary>
        /// Update a ProfileItem in the ProfilesList by name
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        private static void UpdateSetting(ProfileItem profile)
        {
            //update _profilesList
        }
    }
}
