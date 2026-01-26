using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class SenderDomainAlignmentRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersAreNullOrWhitespace()
        {
            var rule = new SenderDomainAlignmentRule();

            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = null, SenderEmail = "user@example.com" }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "", SenderEmail = "user@example.com" }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "   ", SenderEmail = "user@example.com" }));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderEmailIsNullOrWhitespace()
        {
            var rule = new SenderDomainAlignmentRule();

            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "from mail.example.com", SenderEmail = null }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "from mail.example.com", SenderEmail = "" }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "from mail.example.com", SenderEmail = "   " }));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoFromDomainInHeaders()
        {
            var rule = new SenderDomainAlignmentRule();
            var mail = new TestMailItem { Headers = "some other header", SenderEmail = "user@example.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSendingDomainEndsWithFromDomain()
        {
            var rule = new SenderDomainAlignmentRule();
            var mail = new TestMailItem
            {
                Headers = "from mail.example.com",
                SenderEmail = "user@example.com"
            };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSendingDomainDoesNotEndWithFromDomain()
        {
            var rule = new SenderDomainAlignmentRule();
            var mail = new TestMailItem
            {
                Headers = "from malicious.com",
                SenderEmail = "user@example.com"
            };

            Assert.IsTrue(rule.IsMatch(mail));
        }
    }
}