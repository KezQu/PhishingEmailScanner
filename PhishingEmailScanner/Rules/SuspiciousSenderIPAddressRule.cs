using System.Text.RegularExpressions;

namespace PhishingEmailScanner.Rules
{
    public class SuspiciousSenderIPAddressRule : IPhishingRule
    {
        public string Name => "Suspicious Sender IP Address";

        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers))
                return PhishingConfidenceLevel.kNone;

            if (Regex.IsMatch(mail.Headers, @"(10\.|192\.168\.|172\.(1[6-9]|2\d|3[0-1]))"))
                return PhishingConfidenceLevel.kHigh;

            return PhishingConfidenceLevel.kNone;
        }
    }
}
