using Microsoft.Office.Interop.Outlook;
using PhishingEmailScanner.Rules;

namespace PhishingEmailScanner
{
    public partial class ThisAddIn
    {
        private PhishingScannerEngine phishing_engine_ = new PhishingScannerEngine();
        private WhitelistCacheManager cache_manager_ = new WhitelistCacheManager();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            cache_manager_.LoadWhitelist();
            InitializePhishingScanner();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Uwaga: program Outlook nie generuje już tego zdarzenia. Jeśli masz kod, który 
            //    musi działać po zamknięciu programu Outlook, zobacz https://go.microsoft.com/fwlink/?LinkId=506785
        }

        private void InitializePhishingScanner()
        {
            phishing_engine_.AddRule(new AttachmentsRule("config\\AttachmentsConfig.json"));
            phishing_engine_.AddRule(new CommonDomainsRule("config\\CommonDomainsConfig.json"));
            phishing_engine_.AddRule(new ReturnPathMismatchRule());
            phishing_engine_.AddRule(new SenderAuthenticityRule());
            phishing_engine_.AddRule(new SenderDomainAlignmentRule());
            phishing_engine_.AddRule(new SuspiciousSenderIPAddressRule());
            phishing_engine_.AddRule(new UrgencyLanguageRule("common\\UrgencyLanguageConfig.json"));

            phishing_engine_.AddRuleOverride(new AllowedDomainsRuleOverride(cache_manager_.GetFromWhitelist("Domains")));
            phishing_engine_.AddRuleOverride(new AllowedSendersRuleOverride(cache_manager_.GetFromWhitelist("Senders")));
        }

        private void ScanEmailForPhishing(MailItem mail)
        {
            MailItemAdapter mail_adapter = new MailItemAdapter(mail);
            var analysis_result = phishing_engine_.Analyze(mail_adapter);
        }



        #region Kod wygenerowany przez program VSTO

        /// <summary>
        /// Wymagana metoda obsługi projektanta — nie należy modyfikować 
        /// zawartość tej metody z edytorem kodu.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
