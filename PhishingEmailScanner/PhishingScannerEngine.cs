using PhishingEmailScanner.Rules;
using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public class PhishingScannerEngine
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
            var analysis_result = new PhishingAnalysisResult();

            foreach (var rule in rules_)
            {
                var rule_confidence_level = rule.IsMatch(mail);
                if (rule_confidence_level != PhishingConfidenceLevel.kNone)
                {
                    analysis_result.TriggeredRules.Add(rule.Name);

                    analysis_result.ConfidenceLevel = analysis_result.ConfidenceLevel.Add(rule_confidence_level);

                    if (analysis_result.ConfidenceLevel > PhishingConfidenceLevel.kCritical)
                        analysis_result.ConfidenceLevel = PhishingConfidenceLevel.kCritical;
                }
            }
            foreach (var rule in rules_override_)
            {
                if (rule.IsMatch(mail))
                {
                    analysis_result.ConfidenceLevel = PhishingConfidenceLevel.kOverriden;
                    analysis_result.TriggeredRuleOverrides.Add(rule.Name);
                }
            }

            return analysis_result;
        }
    }

}
