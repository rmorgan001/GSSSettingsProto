using System.IO;
using System.Text.Json;

namespace Settings
{
    internal static class Profile
    {
        private static  List<ProfileItem>? _profilesList = [];
        internal static List<SettingItem>? SettingsList = [];
        private static readonly string LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Settings");


        /// <summary>
        /// Add a Profile
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void Add(ProfileItem profile)
        {
            //check for duplicates
            //save current settings to file
            //add the profile to ProfilesList
            //save profiles to file
        }

        /// <summary>
        /// Copy a Profile to a new one
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void Copy(ProfileItem profile)
        {
            //make new name and check for duplicates
            //Copy old settings to file to new one
            //add the new profile to ProfilesList
            //save profiles to file
        }

        /// <summary>
        /// Delete ProfileItem from ProfilesList, then SaveProfileFile
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void Delete(ProfileItem profile)
        {
            // delete from ProfilesList
            // save the profiles file
            // delete the correct settings file
        }

        /// <summary>
        /// Find a ProfileItem in the ProfilesList
        /// </summary>
        /// <param name="profileName">Name of Profile</param>
        /// <returns>a specific ProfileItem from ProfilesList</returns>
        public static ProfileItem GetProfile(string profileName)
        {
            // find in ProfilesList
            var a = new ProfileItem
            {
                Name = profileName,
                IsCurrent = true
            };
            
            return a;
        }

        /// <summary>
        /// Get all Profiles
        /// </summary>
        /// <returns>returns ProfilesList</returns>
        public static List<ProfileItem>? GetProfiles()
        {
            // return ProfilesList
            return _profilesList;
        }

        /// <summary>
        /// Load Profiles.json file.  Find the first IsCurrent profile.  Load the settings file into SettingsList.
        /// </summary>
        public static void Load()
        {
            //load the profiles
            //load the correct settings

            var p = Directory.Exists(LocalPath);
            if (p)
            {
                return;
            }


            using var r = new StreamReader("file.json");
            var json = r.ReadToEnd();
            _profilesList = JsonSerializer.Deserialize<List<ProfileItem>>(json);

        }

        /// <summary>
        /// Save the ProfilesList to file Profiles.json
        /// </summary>
        public static void SaveProfiles()
        {
            // validate each setting files exist, or copy from default
            // save ProfilesList to file
        }

        /// <summary>
        /// Save SettingsList for a specific Profile to file
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void SaveSettings(ProfileItem profile)
        {
            // save SettingsList to file, overwrite
        }

        /// <summary>
        /// Update the ProfileItem in the ProfilesList by name
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void UpdateProfile(ProfileItem profile)
        {
            //update _profilesList
        }

        /// <summary>
        /// Update the ProfileItem in the ProfilesList by name
        /// </summary>
        /// <param name="profile">ProfileItem</param>
        public static void UpdateSetting(ProfileItem profile)
        {
            //update _profilesList
        }
    }

    internal class ProfileItem
    {
        public required string Name;
        public required bool IsCurrent;
    }

    internal class SettingItem
    {
        public required string Name;
        public string? Value;
        public string? Type;                // simple type keywords
        public string? Category;            // GSS tab or location
        public bool IsDepreciated;           
        public string? VersionDepreciated;  
        public string? VersionAdded;
    }
}
