using System.Collections.Generic;

namespace PhishingEmailScanner.Tests
{
    public class FakeMailItem : IMailItem
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SenderEmail { get; set; }
        public List<string> Links { get; set; }
        public List<string> Attachments { get; set; }
        public string Headers { get; set; }
    }
}