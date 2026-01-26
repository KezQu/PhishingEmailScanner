using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{

    [TestClass]
    public class AllowedSendersRuleOverrideTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenSenderIsAllowed()
        {
            var rule = new AllowedSendersRuleOverride(new[] { "test@example.com", "other@example.com" });
            var mail = new TestMailItem { SenderEmail = "test@example.com" };

            Assert.IsTrue(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderIsNotAllowed()
        {
            var rule = new AllowedSendersRuleOverride(new[] { "allowed@example.com" });
            var mail = new TestMailItem { SenderEmail = "notallowed@example.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenAllowedSendersIsEmpty()
        {
            var rule = new AllowedSendersRuleOverride(new string[0]);
            var mail = new TestMailItem { SenderEmail = "anyone@example.com" };

            Assert.IsFalse(rule.IsMatch(mail));
        }
    }
}