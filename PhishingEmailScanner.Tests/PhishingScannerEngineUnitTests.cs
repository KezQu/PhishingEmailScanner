using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner.Tests
{
    public class FakeRule : IPhishingRule
    {
        public string Name { get; set; }
        public PhishingConfidenceLevel ReturnLevel { get; set; }
        public PhishingConfidenceLevel IsMatch(IMailItem mail) => ReturnLevel;
    }

    public class FakeOverrideRule : IPhishingRuleOverride
    {
        public string Name { get; set; }
        public bool ShouldOverride { get; set; }
        public bool IsMatch(IMailItem mail) => ShouldOverride;
    }

    [TestClass]
    public class PhishingScannerEngineUnitTests
    {
        [TestMethod]
        public void Analyze_NoRules_ReturnsNone()
        {
            var sut = new PhishingScannerEngine();
            var mail = new FakeMailItem();
            var result = sut.Analyze(mail);

            Assert.AreEqual(result.ConfidenceLevel, PhishingConfidenceLevel.kNone);
            Assert.AreEqual(result.TriggeredRules.Count, 0);
            Assert.AreEqual(result.TriggeredRuleOverrides.Count, 0);
        }

        [TestMethod]
        public void Analyze_OneRuleTriggered()
        {
            var sut = new PhishingScannerEngine();
            sut.AddRule(new FakeRule { Name = "Fake", ReturnLevel = PhishingConfidenceLevel.kHigh });
            var mail = new FakeMailItem();
            var result = sut.Analyze(mail);

            Assert.AreEqual(result.ConfidenceLevel, PhishingConfidenceLevel.kHigh);
            Assert.AreEqual(result.TriggeredRules.Count, 1);
            CollectionAssert.Contains(result.TriggeredRules, "Fake");
        }

        [TestMethod]
        public void Analyze_MultipleRulesTriggered()
        {
            var sut = new PhishingScannerEngine();
            sut.AddRule(new FakeRule { Name = "Rule1", ReturnLevel = PhishingConfidenceLevel.kLow });
            sut.AddRule(new FakeRule { Name = "Rule2", ReturnLevel = PhishingConfidenceLevel.kMedium });
            var mail = new FakeMailItem();
            var result = sut.Analyze(mail);

            Assert.AreEqual(result.ConfidenceLevel, PhishingConfidenceLevel.kLow.Add(PhishingConfidenceLevel.kMedium));
            Assert.AreEqual(result.TriggeredRules.Count, 2);
            CollectionAssert.Contains(result.TriggeredRules, "Rule1");
            CollectionAssert.Contains(result.TriggeredRules, "Rule2");
        }

        [TestMethod]
        public void Analyze_OverrideRuleTriggered()
        {
            var sut = new PhishingScannerEngine();
            sut.AddRule(new FakeRule { Name = "Rule1", ReturnLevel = PhishingConfidenceLevel.kHigh });
            sut.AddRuleOverride(new FakeOverrideRule { Name = "Override", ShouldOverride = true });
            var mail = new FakeMailItem();
            var result = sut.Analyze(mail);

            Assert.AreEqual(result.ConfidenceLevel, PhishingConfidenceLevel.kOverridden);
            Assert.AreEqual(result.TriggeredRuleOverrides.Count, 1);
            CollectionAssert.Contains(result.TriggeredRuleOverrides, "Override");
        }
    }
}
