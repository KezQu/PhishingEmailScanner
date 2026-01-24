using System.Collections.Generic;

namespace PhishingEmailScanner
{

    public enum PhishingConfidenceLevel
    {
        kNone,
        kLow,
        kMedium,
        kHigh,
        kCritical
    }

    public class PhishingAnalysisResult
    {
        public PhishingConfidenceLevel ConfidenceLevel { get; set; }
        public List<string> TriggeredRules { get; set; } = new List<string>();
        public int Score { get; set; }
    }
}