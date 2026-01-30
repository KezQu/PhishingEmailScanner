using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public interface IMailItem
    {
        string Subject { get; }
        string Body { get; }
        string SenderEmail { get; }
        List<string> Links { get; }
        List<string> Attachments { get; }
        string Headers { get; }
    }

}
