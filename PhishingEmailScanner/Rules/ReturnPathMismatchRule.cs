using System.Linq;
using System.Text.RegularExpressions;

namespace PhishingEmailScanner
{
    public class ReturnPathMismatchRule : IPhishingRule
    {
        public string Name => "Return-Path Mismatch";

        public bool IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers) ||
                string.IsNullOrWhiteSpace(mail.SenderEmail))
                return false;

            var fromDomain = mail.SenderEmail.Split('@').Last().ToLowerInvariant();

            var match = Regex.Match(
                mail.Headers,
                @"return-path:\s*<[^@]+@([^>]+)>",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return false;

            var returnPathDomain = match.Groups[1].Value.ToLowerInvariant();

            return !(fromDomain == returnPathDomain || returnPathDomain.Contains(fromDomain));
        }
    }
}
