using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public class PhishingDetectorEngine
    {
        private readonly List<IPhishingRule> rules_ = new List<IPhishingRule>();

        public void AddRule(IPhishingRule rule)
        {
            rules_.Add(rule);
        }

        public PhishingAnalysisResult Analyze(IMailItem mail)
        {
            var result = new PhishingAnalysisResult();
            int score = 0;

            foreach (var rule in rules_)
            {
                if (rule.IsMatch(mail))
                {
                    score += rule.Score;
                    result.TriggeredRules.Add(rule.Name);
                }
            }

            result.Score = score;
            result.ConfidenceLevel = CalculateConfidence(score);

            return result;
        }

        private PhishingConfidenceLevel CalculateConfidence(int score)
        {
            if (score >= 80)
            {
                return PhishingConfidenceLevel.kCritical;

            }
            else if (score >= 50)
            {
                return PhishingConfidenceLevel.kHigh;

            }
            else if (score >= 50)
            {
                return PhishingConfidenceLevel.kMedium;

            }
            else if (score >= 50)
            {
                return PhishingConfidenceLevel.kLow;

            }
            return PhishingConfidenceLevel.kNone;

        }
    }
}
