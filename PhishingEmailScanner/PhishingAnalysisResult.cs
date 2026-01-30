using System;
using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public static class EnumExtensions
    {
        public static TEnum Add<TEnum>(this TEnum left, TEnum right) where TEnum : struct, System.Enum
        {
            int sum = Convert.ToInt32(left) | Convert.ToInt32(right);
            return (TEnum)Enum.ToObject(typeof(TEnum), sum);
        }
    }

    [Flags]
    public enum PhishingConfidenceLevel
    {
        kNone = 0,
        kLow = 1,
        kModerate = 1 << 1,
        kMedium = 1 << 2,
        kHigh = 1 << 3,
        kCritical = 1 << 4,
        kOverridden = 1 << 30
    }

    public class PhishingAnalysisResult
    {
        public PhishingConfidenceLevel ConfidenceLevel { get; set; } = PhishingConfidenceLevel.kNone;
        public List<string> TriggeredRules { get; set; } = new List<string>();
        public List<string> TriggeredRuleOverrides { get; set; } = new List<string>();
    }
}
