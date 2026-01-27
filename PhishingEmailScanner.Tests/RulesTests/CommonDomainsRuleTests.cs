using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class CommonDomainsRuleTests
    {
        private string config_path_;

        [TestInitialize]
        public void Setup()
        {
            var config = new
            {
                Domains = new List<string> { "example.com" }
            };
            string json = JsonSerializer.Serialize(config);
            config_path_ = Path.GetTempFileName();
            File.WriteAllText(config_path_, json);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(config_path_))
                File.Delete(config_path_);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenLinkIsSimilarToCommonDomain_Levenshtein()
        {
            var sut = new CommonDomainsRule(config_path_);
            var mail = new FakeMailItem { Links = new List<string> { "https://ex@mp1e.com" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kModerate);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenLinkIsSimilarToCommonDomain_JaroWinkler()
        {
            var sut = new CommonDomainsRule(config_path_);
            var mail = new FakeMailItem { Links = new List<string> { "https://exampel.com" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kModerate);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenLinkIsExactlyCommonDomain()
        {
            var sut = new CommonDomainsRule(config_path_);
            var mail = new FakeMailItem { Links = new List<string> { "https://example.com" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoLinks()
        {
            var sut = new CommonDomainsRule(config_path_);
            var mail = new FakeMailItem { Links = new List<string>() };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoSimilarDomains()
        {
            var sut = new CommonDomainsRule(config_path_);
            var mail = new FakeMailItem { Links = new List<string> { "https://totallydifferent.com" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }
    }
}