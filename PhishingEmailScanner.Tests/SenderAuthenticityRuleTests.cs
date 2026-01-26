using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class SenderAuthenticityRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersAreNullOrWhitespace()
        {
            var rule = new SenderAuthenticityRule();

            Assert.IsTrue(rule.IsMatch(new TestMailItem { Headers = null }));
            Assert.IsTrue(rule.IsMatch(new TestMailItem { Headers = "" }));
            Assert.IsTrue(rule.IsMatch(new TestMailItem { Headers = "   " }));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSpfIsFail()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "Received-SPF: spf=fail" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSpfIsSoftFail()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "Received-SPF: spf=softfail" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDkimIsFail()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "Authentication-Results: dkim=fail" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDmarcIsFail()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "Authentication-Results: dmarc=fail" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDmarcIsReject()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "Authentication-Results: dmarc=reject" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenNoFailuresDetected()
        {
            var rule = new SenderAuthenticityRule();
            var mail = new TestMailItem { Headers = "spf=pass dkim=pass dmarc=pass" };

            Assert.IsFalse(rule.IsMatch(mail));
        }
    }
}