using System.Text.RegularExpressions;

namespace PhishingEmailScanner
{
    public class SenderAuthenticityRule : IPhishingRule
    {
        public string Name => "Sender Authenticity (SPF/DKIM/DMARC)";

        public bool IsMatch(IMailItem mail)
        {
            if (string.IsNullOrWhiteSpace(mail.Headers))
                return true;

            var headers = mail.Headers.ToLowerInvariant();

            return HasSpfFail(headers) || HasDkimFail(headers) || HasDmarcFail(headers);
        }

        private bool HasSpfFail(string headers)
        {
            return Regex.IsMatch(headers, @"spf\s*=\s*(fail|softfail)");
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
