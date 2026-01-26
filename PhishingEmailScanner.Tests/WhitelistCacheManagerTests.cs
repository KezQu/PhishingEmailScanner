using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class WhitelistCacheManagerTests
    {
        private string tempDir;
        private string tempFile;

        [TestInitialize]
        public void Setup()
        {
            tempDir = Path.Combine(Path.GetTempPath(), "PhishingEmailScannerTest");
            Directory.CreateDirectory(tempDir);
            tempFile = Path.Combine(tempDir, "whitelist_cache.json");

            var type = typeof(PhishingEmailScanner.WhitelistCacheManager);
            var field = type.GetField("cache_file_path_", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            field.SetValue(null, tempFile);

            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }

        [TestMethod]
        public void AddToWhitelist_AddsValueToCategory()
        {
            var manager = new PhishingEmailScanner.WhitelistCacheManager();
            manager.AddToWhitelist("domains", "example.com");

            manager.SaveWhitelist();

            var json = File.ReadAllText(tempFile);
            var dict = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(json);

            Assert.IsTrue(dict.ContainsKey("domains"));
            Assert.IsTrue(dict["domains"].Contains("example.com"));
        }

        [TestMethod]
        public void LoadWhitelist_LoadsSavedData()
        {
            // Prepare file
            var dict = new Dictionary<string, HashSet<string>>
            {
                { "domains", new HashSet<string> { "test.com" } }
            };
            File.WriteAllText(tempFile, JsonSerializer.Serialize(dict));

            var manager = new PhishingEmailScanner.WhitelistCacheManager();
            manager.LoadWhitelist();

            // Save to a new file to check if loaded correctly
            manager.AddToWhitelist("domains", "another.com");
            manager.SaveWhitelist();

            var loaded = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(File.ReadAllText(tempFile));
            Assert.IsTrue(loaded.ContainsKey("domains"));
            Assert.IsTrue(loaded["domains"].Contains("test.com"));
            Assert.IsTrue(loaded["domains"].Contains("another.com"));
        }

        [TestMethod]
        public void SaveWhitelist_CreatesFileIfNotExists()
        {
            var manager = new PhishingEmailScanner.WhitelistCacheManager();
            manager.AddToWhitelist("ips", "1.2.3.4");
            manager.SaveWhitelist();

            Assert.IsTrue(File.Exists(tempFile));
            var json = File.ReadAllText(tempFile);
            Assert.IsTrue(json.Contains("1.2.3.4"));
        }

        [TestMethod]
        public void LoadWhitelist_EmptyIfFileMissing()
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);

            var manager = new PhishingEmailScanner.WhitelistCacheManager();
            manager.LoadWhitelist();

            // Add and save to ensure no exception and file is created
            manager.AddToWhitelist("domains", "missing.com");
            manager.SaveWhitelist();

            Assert.IsTrue(File.Exists(tempFile));
        }
    }
}