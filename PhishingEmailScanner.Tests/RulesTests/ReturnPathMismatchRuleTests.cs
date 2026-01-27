using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class ReturnPathMismatchRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenReturnPathDesNotMatchSendersDomain()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = "Return-Path: <evill@malicious.domain.co>", SenderEmail = "support@paypal.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kModerate);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersAreEmpty()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = string.Empty, SenderEmail = "support@paypal.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSendersEmailIsEmpty()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = "Return-Path: <R4ndom-Has8@paypal.com>", SenderEmail = "" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenReturnPathHeaderIsMissing()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = "Some-Other-Header: value", SenderEmail = "support@paypal.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderDomainIsExactMatchToReturnPathDomain()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = "Return-Path: <R4ndom-Has8@paypal.com>", SenderEmail = "support@paypal.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderDomainIsPartOfReturnPathDomain()
        {
            var sut = new ReturnPathMismatchRule();
            var mail = new FakeMailItem { Headers = "Return-Path: <R4ndom-Has8@mail.paypal.com>", SenderEmail = "support@paypal.com" };

            Assert.AreEqual(sut.IsMatch(mail), PhishingConfidenceLevel.kNone);
        }

    }
}
