using System.Collections.Generic;
using System.Linq;

namespace PhishingEmailScanner
{
    public class DomainReputationRuleOverride : IPhishingRuleOverride
    {
        public string Name => "Domain / IP Reputation";

        private readonly HashSet<string> whitelisted_domains_;

        public DomainReputationRuleOverride(
            IEnumerable<string> whitelisted_domains)
        {
            whitelisted_domains_ = new HashSet<string>(whitelisted_domains);
        }

        public bool IsMatch(IMailItem mail)
        {
            var domain = mail.SenderEmail?.Split('@').Last();

            return (domain != null && whitelisted_domains_.Contains(domain));
        }
    }
}
