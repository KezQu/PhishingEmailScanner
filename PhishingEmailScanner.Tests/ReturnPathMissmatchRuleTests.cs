using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class ReturnPathMismatchRuleTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenReturnPathDesNotMatchSendersDomain()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = "Return-Path: <evill@malicious.domain.co>", SenderEmail = "support@paypal.com" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenHeadersAreEmpty()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = string.Empty, SenderEmail = "support@paypal.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSendersEmailIsEmpty()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = "Return-Path: <R4ndom-Has8@paypal.com>", SenderEmail = "" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenReturnPathHeaderIsMissing()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = "Some-Other-Header: value", SenderEmail = "support@paypal.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderDomainIsExactMatchToReturnPathDomain()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = "Return-Path: <R4ndom-Has8@paypal.com>", SenderEmail = "support@paypal.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderDomainIsPartOfReturnPathDomain()
        {
            var rule = new ReturnPathMismatchRule();
            var mail = new TestMailItem { Headers = "Return-Path: <R4ndom-Has8@mail.paypal.com>", SenderEmail = "support@paypal.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

    }
}
