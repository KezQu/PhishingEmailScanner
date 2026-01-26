using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public class PhishingDetectorEngine
    {
        private readonly List<IPhishingRule> rules_ = new List<IPhishingRule>();
        private readonly List<IPhishingRuleOverride> rules_override_ = new List<IPhishingRuleOverride>();

        public void AddRule(IPhishingRule rule)
        {
            rules_.Add(rule);
        }

        public void AddRuleOverride(IPhishingRuleOverride rule)
        {
            rules_override_.Add(rule);
        }
        public PhishingAnalysisResult Analyze(IMailItem mail)
        {
            var result = new PhishingAnalysisResult();

            foreach (var rule in rules_)
            {
                if (rule.IsMatch(mail))
                {
                    result.TriggeredRules.Add(rule.Name);
                }
            }
            result.ConfidenceLevel = CalculateConfidence(result.TriggeredRules.Count);

            foreach (var rule in rules_override_)
            {
                if (rule.IsMatch(mail))
                {
                    result.ConfidenceLevel = PhishingConfidenceLevel.kNone;

                }
            }

            return result;
        }
        private PhishingConfidenceLevel CalculateConfidence(int score)
        {
            if (score >= 4)
            {
                return PhishingConfidenceLevel.kCritical;

            }
            else if (score == 3)
            {
                return PhishingConfidenceLevel.kHigh;

            }
            else if (score == 2)
            {
                return PhishingConfidenceLevel.kMedium;

            }
            else if (score == 1)
            {
                return PhishingConfidenceLevel.kLow;

            }
            return PhishingConfidenceLevel.kNone;

        }
    }
}
