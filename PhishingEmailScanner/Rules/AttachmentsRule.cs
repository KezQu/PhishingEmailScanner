using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Rules
{
    public class AttachmentsRule : IPhishingRule
    {
        private readonly List<string> suspicious_extentions_ = new List<string>();
        private readonly List<string> suspicious_filenames_ = new List<string>();

        public AttachmentsRule(string config_path)
        {
            var json = File.ReadAllText(config_path);

            var root = JsonSerializer.Deserialize<JsonElement>(json);
            suspicious_extentions_ = JsonSerializer.Deserialize<List<string>>(
                root.GetProperty("Attachments").GetProperty("Extentions").GetRawText()
            );
            suspicious_filenames_ = JsonSerializer.Deserialize<List<string>>(
                    root.GetProperty("Attachments").GetProperty("Filenames").GetRawText()
                );
        }
        public string Name => "Dangerous Attachment";
        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            foreach (var attachment in mail.Attachments)
            {
                if (attachment.Split('.').Length > 2)
                    return PhishingConfidenceLevel.kHigh;

                var extention = Path.GetExtension(attachment)?.ToLowerInvariant();
                if (suspicious_extentions_.Contains(extention))
                    return PhishingConfidenceLevel.kHigh;

                var filename = Path.GetFileNameWithoutExtension(attachment)?.ToLowerInvariant();
                if (suspicious_filenames_.Contains(filename))
                    return PhishingConfidenceLevel.kModerate;
            }
            return PhishingConfidenceLevel.kNone;
        }

    }
}
