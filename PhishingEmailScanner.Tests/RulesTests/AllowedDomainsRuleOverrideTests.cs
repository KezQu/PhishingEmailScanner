using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class AllowedDomainsRuleOverrideTests
    {
        [TestMethod]
        public void IsMatch_ReturnsTrue_WhenDomainIsWhitelisted()
        {
            var wct = new WhitelistCacheManager();
            wct.AddToWhitelist("Domains", "example.com");
            wct.AddToWhitelist("Domains", "other.com");
            var sut = new AllowedDomainsRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "user@example.com" };

            Assert.IsTrue(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenDomainIsNotWhitelisted()
        {
            var wct = new WhitelistCacheManager();
            wct.AddToWhitelist("Domains", "allowed.com");
            var sut = new AllowedDomainsRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "user@notallowed.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderEmailIsNull()
        {
            var wct = new WhitelistCacheManager();
            wct.AddToWhitelist("Domains", "example.com");
            var sut = new AllowedDomainsRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = null };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenWhitelistIsEmpty()
        {
            var wct = new WhitelistCacheManager();
            var sut = new AllowedDomainsRuleOverride(wct);
            var mail = new FakeMailItem { SenderEmail = "user@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }
    }
}