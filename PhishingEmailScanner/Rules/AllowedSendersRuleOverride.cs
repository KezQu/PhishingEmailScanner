using System.Collections.Generic;

namespace PhishingEmailScanner.Rules
{
    public class AllowedSendersRuleOverride : IPhishingRuleOverride
    {
        public string Name => "Sender In Allowed Address Book";

        private readonly WhitelistCacheManager _whitelistCacheManager;

        public AllowedSendersRuleOverride( WhitelistCacheManager whitelistCacheManager )
        {
            _whitelistCacheManager = whitelistCacheManager;
        }

        public bool IsMatch(IMailItem mail)
        {
            return _whitelistCacheManager.GetFromWhitelist("Senders").Contains(mail.SenderEmail);
        }
    }
}
