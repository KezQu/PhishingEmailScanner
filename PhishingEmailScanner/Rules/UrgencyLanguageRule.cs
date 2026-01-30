using System.Collections.Generic;

using System.IO;
using System.Text.Json;

namespace PhishingEmailScanner.Rules
{
    public class UrgencyLanguageRule : IPhishingRule
    {
        private List<string> urgency_phrases_ = new List<string>();

        public UrgencyLanguageRule(string config_path)
        {
            var json = File.ReadAllText(config_path);

            var root = JsonSerializer.Deserialize<JsonElement>(json);
            urgency_phrases_ = JsonSerializer.Deserialize<List<string>>(
                root.GetProperty("Phrases").GetRawText()
            );
        }
        public string Name => "Urgency Language";
        public PhishingConfidenceLevel IsMatch(IMailItem mail)
        {
            string text = (mail.Subject + mail.Body)?.ToLower();

            if (urgency_phrases_.Exists(p => text.Contains(p.ToLowerInvariant())))
                return PhishingConfidenceLevel.kLow;

            return PhishingConfidenceLevel.kNone;
        }

    }
}
