using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class SenderDomainAlignmentRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersAreNullOrWhitespace()
        {
            var sut = new SenderDomainAlignmentRule();

            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = null, SenderEmail = "user@example.com" }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "", SenderEmail = "user@example.com" }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "   ", SenderEmail = "user@example.com" }), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderEmailIsNullOrWhitespace()
        {
            var sut = new SenderDomainAlignmentRule();

            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "from mail.example.com", SenderEmail = null }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "from mail.example.com", SenderEmail = "" }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "from mail.example.com", SenderEmail = "   " }), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoFromDomainInHeaders()
        {
            var sut = new SenderDomainAlignmentRule();
            var mail = new FakeMailItem { Headers = "some other header", SenderEmail = "user@example.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSendingDomainEndsWithFromDomain()
        {
            var sut = new SenderDomainAlignmentRule();
            var mail = new FakeMailItem
            {
                Headers = "from mail.example.com",
                SenderEmail = "user@example.com"
            };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSendingDomainDoesNotEndWithFromDomain()
        {
            var sut = new SenderDomainAlignmentRule();
            var mail = new FakeMailItem
            {
                Headers = "from malicious.com",
                SenderEmail = "user@example.com"
            };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kModerate);
        }
    }
}