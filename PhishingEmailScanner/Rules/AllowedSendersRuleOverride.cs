using System.Collections.Generic;

namespace PhishingEmailScanner.Rules
{
    public class AllowedSendersRuleOverride : IPhishingRuleOverride
    {
        public string Name => "Sender In Allowed Address Book";

        private HashSet<string> allowed_senders_;

        public bool IsMatch(IMailItem mail)
        {
            return allowed_senders_.Contains(mail.SenderEmail);
        }
    }
}
