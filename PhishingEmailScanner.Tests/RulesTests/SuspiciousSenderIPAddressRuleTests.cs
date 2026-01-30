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
            var sut = new SuspiciousSenderIPAddressRule();

            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = null }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "" }), PhishingConfidenceLevel.kNone);
            Assert.AreEqual(sut.IsMatch(new FakeMailItem { Headers = "   " }), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate10Range()
        {
            var sut = new SuspiciousSenderIPAddressRule();
            var mail = new FakeMailItem { Headers = "Received: from 10.0.0.1" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);

        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate192Range()
        {
            var sut = new SuspiciousSenderIPAddressRule();
            var mail = new FakeMailItem { Headers = "Received: from 192.168.1.100" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);

        }

        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenHeadersContainPrivate172Range()
        {
            var sut = new SuspiciousSenderIPAddressRule();
            var mail = new FakeMailItem { Headers = "Received: from 172.16.0.1" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kHigh);

        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersContainPublicIP()
        {
            var sut = new SuspiciousSenderIPAddressRule();
            var mail = new FakeMailItem { Headers = "Received: from 8.8.8.8" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }
    }
}