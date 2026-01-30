using System.Linq;
using System.Text.RegularExpressions;

namespace PhishingEmailScanner.Rules
{
    public class ReturnPathMismatchRule : IPhishingRule
    {
        public string Name => "Return-Path Mismatch";

        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers) ||
                string.IsNullOrWhiteSpace(mail.SenderEmail))
                return PhishingConfidenceLevel.kNone;

            var from_domain = mail.SenderEmail.Split('@').Last().ToLowerInvariant();

            var match = Regex.Match(
                mail.Headers,
                @"return-path:\s*<[^@]+@([^>]+)>",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return PhishingConfidenceLevel.kNone;

            var return_path_domain = match.Groups[1].Value.ToLowerInvariant();

            if (!(from_domain == return_path_domain || return_path_domain.Contains(from_domain)))
                return PhishingConfidenceLevel.kModerate;

            return PhishingConfidenceLevel.kNone;
        }
    }
}
