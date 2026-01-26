using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{

    [TestClass]
    public class SuspiciousSenderIPAddressRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersAreNullOrWhitespace()
        {
            var rule = new SuspiciousSenderIPAddressRule();

            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = null }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "" }));
            Assert.IsFalse(rule.IsMatch(new TestMailItem { Headers = "   " }));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate10Range()
        {
            var rule = new SuspiciousSenderIPAddressRule();
            var mail = new TestMailItem { Headers = "Received: from 10.0.0.1" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate192Range()
        {
            var rule = new SuspiciousSenderIPAddressRule();
            var mail = new TestMailItem { Headers = "Received: from 192.168.1.100" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate172Range()
        {
            var rule = new SuspiciousSenderIPAddressRule();
            var mail = new TestMailItem { Headers = "Received: from 172.16.0.1" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersContainPublicIP()
        {
            var rule = new SuspiciousSenderIPAddressRule();
            var mail = new TestMailItem { Headers = "Received: from 8.8.8.8" };

            Assert.IsFalse(rule.IsMatch(mail));
        }
    }
}