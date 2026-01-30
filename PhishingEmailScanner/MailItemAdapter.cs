using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;



namespace PhishingEmailScanner
{

    public class MailItemAdapter : IMailItem
    {
        const string PR_TRANSPORT_MESSAGE_HEADERS =
                    "http://schemas.microsoft.com/mapi/string/{00020386-0000-0000-C000-000000000046}/PR_TRANSPORT_MESSAGE_HEADERS";

        public MailItemAdapter(MailItem mail)
        {
            try 
            { Subject = mail.Subject; } 
            catch (System.Exception ex)
            { 
                Subject = string.Empty;
                Debug.WriteLine("Failed to get mail subject");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }

            try
            { Body = mail.Body; }
            catch (System.Exception ex)
            { 
                Body = string.Empty;
                Debug.WriteLine("Failed to get mail body");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }

            try
            { SenderEmail = mail.SenderEmailAddress; }
            catch (System.Exception ex) 
            { 
                SenderEmail = string.Empty;
                Debug.WriteLine("Failed to get mail sender");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }

            try
            {
                Links = Regex.Matches(mail.HTMLBody ?? "", @"https?://[^\s""'<>\)\],;]+")
                    .Cast<Match>()
                    .Select(m => m.Value.Normalize(NormalizationForm.FormKC)).ToList(); 
            }
            catch (System.Exception ex) 
            { 
                Links = new List<string>();
                Debug.WriteLine("Failed to get mail links");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }

            try
            {
                Attachments = mail.Attachments.Cast<Attachment>()
                    .Select(a => a.FileName.Normalize(NormalizationForm.FormKC)).ToList();
            }
            catch (System.Exception ex) 
            { 
                Attachments = new List<string>();
                Debug.WriteLine("Failed to get mail attachments");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }

            try
            {   
                var accessor = mail.PropertyAccessor;
                var propValue = accessor?.GetProperty(PR_TRANSPORT_MESSAGE_HEADERS);
                Headers = propValue?.ToString() ?? string.Empty; 
            }
            catch (System.Exception ex) 
            { 
                Headers = string.Empty;
                Debug.WriteLine("Mail headers not available for this item");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException?.Message ?? "No inner exception");
            }
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string SenderEmail { get; set; }
        public List<string> Links { get; set; }
        public List<string> Attachments { get; set; }
        public string Headers { get; set; }
    }
}