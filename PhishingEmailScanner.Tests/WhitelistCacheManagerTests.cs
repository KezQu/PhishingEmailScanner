using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class WhitelistCacheManagerTests
    {
        private string temp_dir_;
        private string temp_file_;

        [TestInitialize]
        public void Setup()
        {
            temp_dir_ = Path.Combine(Path.GetTempPath(), "PhishingEmailScannerTest");
            Directory.CreateDirectory(temp_dir_);
            temp_file_ = Path.Combine(temp_dir_, "whitelist_cache.json");

            var type = typeof(WhitelistCacheManager);
            var field = type.GetField("cache_file_path_", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            field.SetValue(null, temp_file_);

            if (File.Exists(temp_file_))
                File.Delete(temp_file_);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(temp_file_))
                File.Delete(temp_file_);
            if (Directory.Exists(temp_dir_))
                Directory.Delete(temp_dir_, true);
        }

        [TestMethod]
        public void AddToWhitelist_AddsValueToCategory()
        {
            var sut = new WhitelistCacheManager();
            sut.AddToWhitelist("domains", "example.com");

            sut.SaveWhitelist();

            var json = File.ReadAllText(temp_file_);
            var dict = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(json);

            Assert.IsTrue(dict.ContainsKey("domains"));
            Assert.IsTrue(dict["domains"].Contains("example.com"));
        }

        [TestMethod]
        public void GetFromWhitelist_BasedOnTheCategory()
        {
            var whitelisted_domains = new HashSet<string> { "example.com", "test.com" };
            var dict = new Dictionary<string, HashSet<string>>
            {
                { "domains",  whitelisted_domains}
            };
            File.WriteAllText(temp_file_, JsonSerializer.Serialize(dict));

            var sut = new WhitelistCacheManager();
            sut.LoadWhitelist();

            Assert.IsTrue(sut.GetFromWhitelist("domains").SetEquals(whitelisted_domains));
        }

        [TestMethod]
        public void GetFromWhitelist_WhenCategoryDoesNotExist()
        {
            var dict = new Dictionary<string, HashSet<string>>
            {
                { "domains",   new HashSet<string> { "example.com", "test.com" }}
            };
            File.WriteAllText(temp_file_, JsonSerializer.Serialize(dict));

            var sut = new WhitelistCacheManager();
            sut.LoadWhitelist();

            Assert.AreEqual(sut.GetFromWhitelist("NotExistentCategory").Count, 0);
        }

        [TestMethod]
        public void LoadWhitelist_LoadsSavedData()
        {
            var dict = new Dictionary<string, HashSet<string>>
            {
                { "domains", new HashSet<string> { "test.com" } }
            };
            File.WriteAllText(temp_file_, JsonSerializer.Serialize(dict));

            var sut = new WhitelistCacheManager();
            sut.LoadWhitelist();

            sut.AddToWhitelist("domains", "another.com");
            sut.SaveWhitelist();

            var loaded = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(File.ReadAllText(temp_file_));
            Assert.IsTrue(loaded.ContainsKey("domains"));
            Assert.IsTrue(loaded["domains"].Contains("test.com"));
            Assert.IsTrue(loaded["domains"].Contains("another.com"));
        }

        [TestMethod]
        public void SaveWhitelist_CreatesFileIfNotExists()
        {
            var sut = new WhitelistCacheManager();
            sut.AddToWhitelist("ips", "1.2.3.4");
            sut.SaveWhitelist();

            Assert.IsTrue(File.Exists(temp_file_));
            var json = File.ReadAllText(temp_file_);
            Assert.IsTrue(json.Contains("1.2.3.4"));
        }

        [TestMethod]
        public void LoadWhitelist_EmptyIfFileMissing()
        {
            if (File.Exists(temp_file_))
                File.Delete(temp_file_);

            var sut = new WhitelistCacheManager();
            sut.LoadWhitelist();

            sut.AddToWhitelist("domains", "missing.com");
            sut.SaveWhitelist();

            Assert.IsTrue(File.Exists(temp_file_));
        }
    }
}