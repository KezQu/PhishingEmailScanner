using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class PhishingScannerEngineIntegrationTests
    {
        private string attachments_config_path_;
        private string common_domains_config_path_;
        private string urgency_config_path_;

        [TestInitialize]
        public void Setup()
        {
            var attachments_config = new
            {
                Attachments = new
                {
                    Extentions = new List<string> { ".exe", ".bat" },
                    Filenames = new List<string> { "malware" }
                }
            };
            attachments_config_path_ = Path.GetTempFileName();
            File.WriteAllText(attachments_config_path_, JsonSerializer.Serialize(attachments_config));

            var common_domains_config = new
            {
                Domains = new List<string> { "example.com" }
            };
            common_domains_config_path_ = Path.GetTempFileName();
            File.WriteAllText(common_domains_config_path_, JsonSerializer.Serialize(common_domains_config));

            var urgency_config = new
            {
                Phrases = new List<string> { "urgent action required" }
            };
            urgency_config_path_ = Path.GetTempFileName();
            File.WriteAllText(urgency_config_path_, JsonSerializer.Serialize(urgency_config));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(attachments_config_path_)) File.Delete(attachments_config_path_);
            if (File.Exists(common_domains_config_path_)) File.Delete(common_domains_config_path_);
            if (File.Exists(urgency_config_path_)) File.Delete(urgency_config_path_);
        }

        [TestMethod]
        public void Analyze_TriggersMultipleRules()
        {
            var sut = new PhishingScannerEngine();
            sut.AddRule(new AttachmentsRule(attachments_config_path_));
            sut.AddRule(new CommonDomainsRule(common_domains_config_path_));
            sut.AddRule(new UrgencyLanguageRule(urgency_config_path_));
            sut.AddRule(new ReturnPathMismatchRule());
            sut.AddRule(new SenderAuthenticityRule());
            sut.AddRule(new SenderDomainAlignmentRule());
            sut.AddRule(new SuspiciousSenderIPAddressRule());

            var mail = new FakeMailItem
            {
                Subject = "Urgent Action Required",
                Body = "Please see the attached file.",
                SenderEmail = "attacker@examp1e.com",
                Links = new List<string> { "https://examp1e.com" },
                Attachments = new List<string> { "malware.exe" },
                Headers = "return-path: <attacker@evil.com>\r\nspf=fail\r\ndkim=fail\r\ndmarc=fail\r\nfrom evil.com"
            };

            var result = sut.Analyze(mail);

            CollectionAssert.Contains(result.TriggeredRules, "Dangerous Attachment");
            CollectionAssert.Contains(result.TriggeredRules, "Common Domain");
            CollectionAssert.Contains(result.TriggeredRules, "Urgency Language");
            CollectionAssert.Contains(result.TriggeredRules, "Return-Path Mismatch");
            CollectionAssert.Contains(result.TriggeredRules, "Sender Authenticity (SPF/DKIM/DMARC)");
            CollectionAssert.Contains(result.TriggeredRules, "Sender Domain Alignment");

            CollectionAssert.DoesNotContain(result.TriggeredRules, "Suspicious Sender IP Address");

            Assert.AreEqual(PhishingConfidenceLevel.kCritical, result.ConfidenceLevel);
        }

        [TestMethod]
        public void Analyze_DoesNotTriggerAnyRules_ForLegitimateEmail()
        {
            var sut = new PhishingScannerEngine();
            sut.AddRule(new AttachmentsRule(attachments_config_path_));
            sut.AddRule(new CommonDomainsRule(common_domains_config_path_));
            sut.AddRule(new UrgencyLanguageRule(urgency_config_path_));
            sut.AddRule(new ReturnPathMismatchRule());
            sut.AddRule(new SenderAuthenticityRule());
            sut.AddRule(new SenderDomainAlignmentRule());
            sut.AddRule(new SuspiciousSenderIPAddressRule());

            var mail = new FakeMailItem
            {
                Subject = "Monthly Report",
                Body = "Please find the monthly report attached.",
                SenderEmail = "employee@example.com",
                Links = new List<string> { "https://example.com/report" },
                Attachments = new List<string> { "report.pdf" },
                Headers = "return-path: <employee@example.com>\r\nspf=pass\r\ndkim=pass\r\ndmarc=pass\r\nfrom example.com"
            };

            var result = sut.Analyze(mail);

            Assert.AreEqual(result.TriggeredRules.Count, 0);
            Assert.AreEqual(result.TriggeredRuleOverrides.Count, 0);
            Assert.AreEqual(result.ConfidenceLevel, PhishingConfidenceLevel.kNone);
        }
    }
}