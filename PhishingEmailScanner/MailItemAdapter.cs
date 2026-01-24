using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;



namespace PhishingEmailScanner
{

    public class MailItemAdapter : IMailItem
    {
        private readonly MailItem mail_;

        public MailItemAdapter(MailItem mail)
        {
            mail_ = mail;
        }

        public string Subject => mail_.Subject;
        public string Body => mail_.Body;
        public string SenderEmail => mail_.SenderEmailAddress;

        public IEnumerable<string> Links =>
            Regex.Matches(mail_.HTMLBody ?? "", @"https?://\S+")
                .Cast<Match>()
                .Select(m => m.Value);
    }
}