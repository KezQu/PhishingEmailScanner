using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PhishingEmailScanner
{
    public class CommonDomainsRule : IPhishingRule
    {
        private List<string> common_domains_ = new List<string>();

        public CommonDomainsRule(string config_path)
        {
            var json = File.ReadAllText(config_path);

            var root = JsonSerializer.Deserialize<JsonElement>(json);
            common_domains_ = JsonSerializer.Deserialize<List<string>>(
                root.GetProperty("Domains").GetRawText()
            );
        }
        public string Name => "Common Domain";
        public int Score => 20;

        public bool IsMatch(IMailItem mail)
        {
            var domains = mail.Links
                .Select(link =>
                {
                    var match = Regex.Match(link, @"https?://([^/\s]+)");
                    return match.Success ? match.Groups[1].Value : null;
                })
                .Where(domain => domain != null)
                .ToList();

            var levenstein = new NormalizedLevenshtein();
            var jaro_winkler = new JaroWinkler();

            foreach (var domain in domains)
            {
                foreach (var common_domain in common_domains_)
                {
                    if (domain.Equals(common_domain, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var levenstein_similarity = levenstein.Similarity(domain, common_domain);
                    var jaro_winkler_similarity = jaro_winkler.Similarity(domain, common_domain);
                    if ((levenstein_similarity >= 0.8) || jaro_winkler_similarity >= 0.9)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
