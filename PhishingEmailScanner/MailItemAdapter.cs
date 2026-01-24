using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<string> Links =>
            Regex.Matches(mail_.HTMLBody ?? "", @"https?://\S+")
                .Cast<Match>()
                .Select(m => m.Value.Normalize(NormalizationForm.FormKC)).ToList();
        public List<string> Attachments =>
            mail_.Attachments.Cast<Attachment>()
                .Select(a => a.FileName.Normalize(NormalizationForm.FormKC)).ToList();
        public string Headers
        {
            get
            {
                const string PR_TRANSPORT_MESSAGE_HEADERS =
                    "http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/PR_TRANSPORT_MESSAGE_HEADERS";

                try
                {
                    return mail_.PropertyAccessor.GetProperty(PR_TRANSPORT_MESSAGE_HEADERS);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}