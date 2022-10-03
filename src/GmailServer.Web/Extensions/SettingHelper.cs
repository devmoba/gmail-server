using System;
using System.IO;

namespace GmailServer.Web.Extensions
{
    public static class SettingHelper
    {
        public static void AddOrUpdateAppSetting<T>(string sectionKey, T value)
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                var json = File.ReadAllText(filePath);
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                SetValueRecursively(sectionKey, jsonObject, value);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, output);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing app settings | {ex.Message}");
            }
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObject, T value)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);
            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObject[currentSection], value);
            }
            else
            {
                jsonObject[currentSection] = value;
            }
        }
    }
}
