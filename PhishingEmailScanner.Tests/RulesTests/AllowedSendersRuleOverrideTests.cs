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
            var sut = new AllowedSendersRuleOverride(new[] { "test@example.com", "other@example.com" });
            var mail = new FakeMailItem { SenderEmail = "test@example.com" };

            Assert.IsTrue(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderIsNotAllowed()
        {
            var sut = new AllowedSendersRuleOverride(new[] { "allowed@example.com" });
            var mail = new FakeMailItem { SenderEmail = "notallowed@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenAllowedSendersIsEmpty()
        {
            var sut = new AllowedSendersRuleOverride(new string[0]);
            var mail = new FakeMailItem { SenderEmail = "anyone@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }
    }
}