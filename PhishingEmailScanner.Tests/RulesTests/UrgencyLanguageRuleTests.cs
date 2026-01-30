using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{

    [TestClass]
    public class UrgencyLanguageRuleTests
    {
        private string CreateConfigFile(List<string> phrases)
        {
            var config = new { Phrases = phrases };
            string json = JsonSerializer.Serialize(config);
            string path = Path.GetTempFileName();
            File.WriteAllText(path, json);
            return path;
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenUrgencyPhraseInSubject()
        {
            var config_path = CreateConfigFile(new List<string> { "urgent action required" });
            var sut = new UrgencyLanguageRule(config_path);
            var mail = new FakeMailItem { Subject = "Urgent Action Required", Body = "Hello" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kLow);
            File.Delete(config_path);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenUrgencyPhraseInBody()
        {
            var config_path = CreateConfigFile(new List<string> { "immediate response" });
            var sut = new UrgencyLanguageRule(config_path);
            var mail = new FakeMailItem { Subject = "Hello", Body = "We need your immediate response." };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kLow);
            File.Delete(config_path);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoUrgencyPhrase()
        {
            var config_path = CreateConfigFile(new List<string> { "urgent", "immediate" });
            var sut = new UrgencyLanguageRule(config_path);
            var mail = new FakeMailItem { Subject = "Hello", Body = "This is a normal message." };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
            File.Delete(config_path);
        }

        [TestMethod]
        public void IsMatch_IsCaseInsensitive()
        {
            var config_path = CreateConfigFile(new List<string> { "Urgent" });
            var sut = new UrgencyLanguageRule(config_path);
            var mail = new FakeMailItem { Subject = "this is URGENT", Body = "" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kLow);
            File.Delete(config_path);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSubjectAndBodyAreNull()
        {
            var config_path = CreateConfigFile(new List<string> { "urgent" });
            var sut = new UrgencyLanguageRule(config_path);
            var mail = new FakeMailItem { Subject = null, Body = null };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
            File.Delete(config_path);
        }
    }
}