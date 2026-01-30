using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class AttachmentsRuleTests
    {
        private string config_path_;

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(config_path_))
                File.Delete(config_path_);
        }

        private void CreateConfigFile(List<string> extensions, List<string> filenames)
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
            config_path_ = Path.GetTempFileName();
            File.WriteAllText(config_path_, json);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentHasMultipleDots()
        {
            CreateConfigFile(new List<string>(), new List<string>());

            var sut = new AttachmentsRule(config_path_);
            var mail = new FakeMailItem { Attachments = new List<string> { "invoice.pdf.exe" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentExtensionIsSuspicious()
        {
            CreateConfigFile(new List<string> { ".exe" }, new List<string>());

            var sut = new AttachmentsRule(config_path_);
            var mail = new FakeMailItem { Attachments = new List<string> { "file.exe" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenAttachmentFilenameIsSuspicious()
        {
            CreateConfigFile(new List<string>(), new List<string> { "malware" });

            var sut = new AttachmentsRule(config_path_);
            var mail = new FakeMailItem { Attachments = new List<string> { "malware.txt" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kModerate);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoAttachments()
        {
            CreateConfigFile(new List<string>(), new List<string>());

            var sut = new AttachmentsRule(config_path_);
            var mail = new FakeMailItem { Attachments = new List<string>() };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenAttachmentsAreNotSuspicious()
        {
            CreateConfigFile(new List<string> { ".exe" }, new List<string> { "malware" });

            var sut = new AttachmentsRule(config_path_);
            var mail = new FakeMailItem { Attachments = new List<string> { "document.pdf" } };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }
    }
}