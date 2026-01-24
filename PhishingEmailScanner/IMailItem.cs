using System.Collections.Generic;

namespace PhishingEmailScanner
{
    public interface IMailItem
    {
        string Subject { get; }
        string Body { get; }
        string SenderEmail { get; }
        IEnumerable<string> Links { get; }
    }

}
