using System;
using System.Linq;

using System.Text.RegularExpressions;

namespace PhishingEmailScanner
{
    public class SenderDomainAlignmentRule : IPhishingRule
    {
        public string Name => "Sender Domain Alignment";

        public bool IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers) ||
                string.IsNullOrWhiteSpace(mail.SenderEmail))
                return false;

            var fromDomain = mail.SenderEmail.Split('@').Last().ToLowerInvariant();

            var receivedMatch = Regex.Match(
                mail.Headers,
                @"from\s+([a-z0-9\.-]+\.[a-z]{2,})",
                RegexOptions.IgnoreCase);

            if (!receivedMatch.Success)
                return false;

            var sendingDomain = receivedMatch.Groups[1].Value.ToLowerInvariant();

            return !sendingDomain.EndsWith(fromDomain);
        }
    }
}

