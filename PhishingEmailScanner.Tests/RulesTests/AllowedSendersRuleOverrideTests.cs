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
            var wct = new WhitelistCacheManager();
            wct.AddToWhitelist("Senders", "test@example.com");
            wct.AddToWhitelist("Senders", "other@example.com");
            var sut = new AllowedSendersRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "test@example.com" };

            Assert.IsTrue(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderIsNotAllowed()
        {
            var wct = new WhitelistCacheManager();
            wct.AddToWhitelist("Senders", "allowed@example.com");
            var sut = new AllowedSendersRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "notallowed@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenAllowedSendersIsEmpty()
        {
            var wct = new WhitelistCacheManager();
            var sut = new AllowedSendersRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "anyone@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }
    }
}