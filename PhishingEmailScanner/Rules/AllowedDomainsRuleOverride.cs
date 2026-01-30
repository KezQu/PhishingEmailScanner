using System.Collections.Generic;
using System.Linq;

namespace PhishingEmailScanner.Rules
{
    public class AllowedDomainsRuleOverride : IPhishingRuleOverride
    {
        public string Name => "Allowed Domain / IP";

        private readonly WhitelistCacheManager _whitelistCacheManager;

        public AllowedDomainsRuleOverride(
            WhitelistCacheManager whitelistCacheManager)
        {
            _whitelistCacheManager = whitelistCacheManager;
        }

        public bool IsMatch(IMailItem mail)
        {
            var domain = mail.SenderEmail?.Split('@').Last();

            return (domain != null && _whitelistCacheManager.GetFromWhitelist("Domains").Contains(domain));
        }
    }
}
