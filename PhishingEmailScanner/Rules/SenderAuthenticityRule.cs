using System.Text.RegularExpressions;

namespace PhishingEmailScanner.Rules
{
    public class SenderAuthenticityRule : IPhishingRule
    {
        public string Name => "Sender Authenticity (SPF/DKIM/DMARC)";

        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers))
                return PhishingConfidenceLevel.kLow;

            var headers = mail.Headers.ToLowerInvariant();

            if (HasDmarcFail(headers))
                return PhishingConfidenceLevel.kCritical;
            if (HasDkimFail(headers))
                return PhishingConfidenceLevel.kHigh;
            if (HasSpfFail(headers))
                return PhishingConfidenceLevel.kHigh;
            if (HasSpfSoftFail(headers))
                return PhishingConfidenceLevel.kLow;

            return PhishingConfidenceLevel.kNone;
        }

        private bool HasSpfSoftFail(string headers)
        {
            return Regex.IsMatch(headers, @"spf\s*=\s*softfail");
        }
        private bool HasSpfFail(string headers)
        {
            return Regex.IsMatch(headers, @"spf\s*=\s*fail");
        }
        private bool HasDkimFail(string headers)
        {
            return Regex.IsMatch(headers, @"dkim\s*=\s*fail");
        }
        private bool HasDmarcFail(string headers)
        {
            return Regex.IsMatch(headers, @"dmarc\s*=\s*(fail|reject)");
        }
    }
}
