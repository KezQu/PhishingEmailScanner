using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class SenderAuthenticityRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersAreNullOrWhitespace()
        {
            var sut = new SenderAuthenticityRule();

            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = null }), PhishingConfidenceLevel.kLow);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "" }), PhishingConfidenceLevel.kLow);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "   " }), PhishingConfidenceLevel.kLow);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSpfIsFail()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "Received-SPF: spf=fail" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSpfIsSoftFail()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "Received-SPF: spf=softfail" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kLow);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDkimIsFail()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "Authentication-Results: dkim=fail" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDmarcIsFail()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "Authentication-Results: dmarc=fail" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kCritical);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDmarcIsReject()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "Authentication-Results: dmarc=reject" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kCritical);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoFailuresDetected()
        {
            var sut = new SenderAuthenticityRule();
            var mail = new FakeMailItem { Headers = "spf=pass dkim=pass dmarc=pass" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);

        }
    }
}