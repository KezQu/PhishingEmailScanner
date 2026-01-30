using System;
using System.Linq;

using System.Text.RegularExpressions;

namespace PhishingEmailScanner.Rules
{
    public class SenderDomainAlignmentRule : IPhishingRule
    {
        public string Name => "Sender Domain Alignment";

        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers) ||
                string.IsNullOrWhiteSpace(mail.SenderEmail))
                return PhishingConfidenceLevel.kNone;

            var from_domain = mail.SenderEmail.Split('@').Last().ToLowerInvariant();

            var received_match = Regex.Match(
                mail.Headers,
                @"from\s+([a-z0-9\.-]+\.[a-z]{2,})",
                RegexOptions.IgnoreCase);

            if (!received_match.Success)
                return PhishingConfidenceLevel.kNone;

            var sending_domain = received_match.Groups[1].Value.ToLowerInvariant();

            if (!sending_domain.EndsWith(from_domain))
                return PhishingConfidenceLevel.kModerate;

            return PhishingConfidenceLevel.kNone;
        }
    }
}

