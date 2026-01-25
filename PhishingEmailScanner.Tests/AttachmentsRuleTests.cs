using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class AttachmentsRuleTests
    {
        private string CreateConfigFile(List<string> extensions, List<string> filenames)
        {
            var config = new
            {
                Attachments = new
                {
                    Extentions = extensions,
                    Filenames = filenames
                }
            };
            string json = JsonSerializer.Serialize(config);
            string path = Path.GetTempFileName();
            File.WriteAllText(path, json);
            return path;
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentHasMultipleDots()
        {
            var configPath = CreateConfigFile(new List<string>(), new List<string>());
            var rule = new AttachmentsRule(configPath);
            var mail = new TestMailItem { Attachments = new List<string> { "invoice.pdf.exe" } };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentExtensionIsSuspicious()
        {
            var configPath = CreateConfigFile(new List<string> { ".exe" }, new List<string>());
            var rule = new AttachmentsRule(configPath);
            var mail = new TestMailItem { Attachments = new List<string> { "file.exe" } };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentFilenameIsSuspicious()
        {
            var configPath = CreateConfigFile(new List<string>(), new List<string> { "malware" });
            var rule = new AttachmentsRule(configPath);
            var mail = new TestMailItem { Attachments = new List<string> { "malware.txt" } };

            Assert.IsTrue(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoAttachments()
        {
            var configPath = CreateConfigFile(new List<string>(), new List<string>());
            var rule = new AttachmentsRule(configPath);
            var mail = new TestMailItem { Attachments = new List<string>() };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenAttachmentsAreNotSuspicious()
        {
            var configPath = CreateConfigFile(new List<string> { ".exe" }, new List<string> { "malware" });
            var rule = new AttachmentsRule(configPath);
            var mail = new TestMailItem { Attachments = new List<string> { "document.pdf" } };

            Assert.IsFalse(rule.IsMatch(mail));
            File.Delete(configPath);
        }
    }
}