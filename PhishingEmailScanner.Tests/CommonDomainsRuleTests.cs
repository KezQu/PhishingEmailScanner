using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class CommonDomainsRuleTests
    {
        private string CreateConfigFile(List<string> domains)
        {
            var config = new
            {
                Domains = domains
            };
            string json = JsonSerializer.Serialize(config);
            string path = Path.GetTempFileName();
            File.WriteAllText(path, json);
            return path;
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenLinkIsSimilarToCommonDomain_Levenshtein()
        {
            var configPath = CreateConfigFile(new List<string> { "example.com" });
            var rule = new CommonDomainsRule(configPath);
            var mail = new TestMailItem { Links = new List<string> { "https://ex@mp1e.com" } };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenLinkIsSimilarToCommonDomain_JaroWinkler()
        {
            var configPath = CreateConfigFile(new List<string> { "example.com" });
            var rule = new CommonDomainsRule(configPath);
            var mail = new TestMailItem { Links = new List<string> { "https://exampel.com" } };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenLinkIsExactlyCommonDomain()
        {
            var configPath = CreateConfigFile(new List<string> { "example.com" });
            var rule = new CommonDomainsRule(configPath);
            var mail = new TestMailItem { Links = new List<string> { "https://example.com" } };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoLinks()
        {
            var configPath = CreateConfigFile(new List<string> { "example.com" });
            var rule = new CommonDomainsRule(configPath);
            var mail = new TestMailItem { Links = new List<string>() };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoSimilarDomains()
        {
            var configPath = CreateConfigFile(new List<string> { "example.com" });
            var rule = new CommonDomainsRule(configPath);
            var mail = new TestMailItem { Links = new List<string> { "https://totallydifferent.com" } };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }
    }
}