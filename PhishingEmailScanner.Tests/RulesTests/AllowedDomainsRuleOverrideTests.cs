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
            var sut = new AllowedDomainsRuleOverride(new[] { "example.com", "other.com" });
            var mail = new FakeMailItem { SenderEmail = "user@example.com" };

            Assert.IsTrue(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenDomainIsNotWhitelisted()
        {
            var sut = new AllowedDomainsRuleOverride(new[] { "allowed.com" });
            var mail = new FakeMailItem { SenderEmail = "user@notallowed.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenSenderEmailIsNull()
        {
            var sut = new AllowedDomainsRuleOverride(new[] { "example.com" });
            var mail = new FakeMailItem { SenderEmail = null };

            Assert.IsFalse(sut.IsMatch(mail));
        }

        [TestMethod]
        public void IsMatch_ReturnsFalse_WhenWhitelistIsEmpty()
        {
            var sut = new AllowedDomainsRuleOverride(new string[0]);
            var mail = new FakeMailItem { SenderEmail = "user@example.com" };

            Assert.IsFalse(sut.IsMatch(mail));
        }
    }
}