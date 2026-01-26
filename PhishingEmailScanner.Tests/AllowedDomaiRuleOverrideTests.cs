using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class AllowedDomaiRuleOverrideTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDomainIsWhitelisted()
        {
            var rule = new AllowedDomaiRuleOverride(new[] { "example.com", "other.com" });
            var mail = new TestMailItem { SenderEmail = "user@example.com" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenDomainIsNotWhitelisted()
        {
            var rule = new AllowedDomaiRuleOverride(new[] { "allowed.com" });
            var mail = new TestMailItem { SenderEmail = "user@notallowed.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderEmailIsNull()
        {
            var rule = new AllowedDomaiRuleOverride(new[] { "example.com" });
            var mail = new TestMailItem { SenderEmail = null };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenWhitelistIsEmpty()
        {
            var rule = new AllowedDomaiRuleOverride(new string[0]);
            var mail = new TestMailItem { SenderEmail = "user@example.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }
    }
}