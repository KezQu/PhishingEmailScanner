using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner
{
    public class WhitelistCacheManager
    {
        private static readonly string cache_file_path_ = Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
            "PhishingEmailScanner",
            "whitelist_cache.json"
        );
        private Dictionary<string, HashSet<string>> whitelist_cache_ = new Dictionary<string, HashSet<string>>();

        public void AddToWhitelist(string category, string value)
        {
            if (!whitelist_cache_.ContainsKey(category))
            {
                whitelist_cache_[category] = new HashSet<string>();
            }
            whitelist_cache_[category].Add(value);
        }
        public HashSet<string> GetFromWhitelist(string category)
        {
            if (!whitelist_cache_.ContainsKey(category))
            {
                whitelist_cache_[category] = new HashSet<string>();
            }
            return whitelist_cache_[category];
        }
        public void LoadWhitelist()
        {
            if (!File.Exists(cache_file_path_))
            {
                whitelist_cache_ = new Dictionary<string, HashSet<string>>();
                return;
            }

            var json = File.ReadAllText(cache_file_path_);
            whitelist_cache_ = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(json) ?? new Dictionary<string, HashSet<string>>();
        }

        public void SaveWhitelist()
        {
            var dir = Path.GetDirectoryName(cache_file_path_);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(whitelist_cache_);
            File.WriteAllText(cache_file_path_, json);
        }
    }
}