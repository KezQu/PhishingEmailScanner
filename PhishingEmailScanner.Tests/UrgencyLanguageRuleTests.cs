using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var configPath = CreateConfigFile(new List<string> { "urgent action required" });
            var rule = new UrgencyLanguageRule(configPath);
            var mail = new TestMailItem { Subject = "Urgent Action Required", Body = "Hello" };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenUrgencyPhraseInBody()
        {
            var configPath = CreateConfigFile(new List<string> { "immediate response" });
            var rule = new UrgencyLanguageRule(configPath);
            var mail = new TestMailItem { Subject = "Hello", Body = "We need your immediate response." };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoUrgencyPhrase()
        {
            var configPath = CreateConfigFile(new List<string> { "urgent", "immediate" });
            var rule = new UrgencyLanguageRule(configPath);
            var mail = new TestMailItem { Subject = "Hello", Body = "This is a normal message." };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_IsCaseInsensitive()
        {
            var configPath = CreateConfigFile(new List<string> { "Urgent" });
            var rule = new UrgencyLanguageRule(configPath);
            var mail = new TestMailItem { Subject = "this is URGENT", Body = "" };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSubjectAndBodyAreNull()
        {
            var configPath = CreateConfigFile(new List<string> { "urgent" });
            var rule = new UrgencyLanguageRule(configPath);
            var mail = new TestMailItem { Subject = null, Body = null };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }
    }
}