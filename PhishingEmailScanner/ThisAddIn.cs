using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools;
using PhishingEmailScanner.Rules;
using PhishingEmailScanner.UI;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhishingEmailScanner
{
    public partial class ThisAddIn
    {
        private PhishingScannerEngine phishing_engine_ = new PhishingScannerEngine();
        private WhitelistCacheManager cache_manager_ = new WhitelistCacheManager();
        private PhishingTaskPane phishingPane;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            phishingPane = new PhishingTaskPane(cache_manager_);

            var activeExplorer = Application.ActiveExplorer();
            if (activeExplorer != null)
            {
                AddPaneToExplorer(activeExplorer);
            }

            HookExplorerSelection();

            cache_manager_.LoadWhitelist();
            InitializePhishingScanner();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Uwaga: program Outlook nie generuje już tego zdarzenia. Jeśli masz kod, który 
            //    musi działać po zamknięciu programu Outlook, zobacz https://go.microsoft.com/fwlink/?LinkId=506785
        }

        private void AddPaneToExplorer( Explorer explorer )
        {
            var pane = this.CustomTaskPanes.Add(
                phishingPane,
                "Bezpieczeństwo wiadomości",
                explorer
            );

            pane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionRight;
            pane.Visible = true;
        }

        private void InitializePhishingScanner()
        {
            phishing_engine_.AddRule(new AttachmentsRule("rules\\config\\AttachmentsConfig.json"));
            phishing_engine_.AddRule(new CommonDomainsRule("rules\\config\\CommonDomainsConfig.json"));
            phishing_engine_.AddRule(new ReturnPathMismatchRule());
            phishing_engine_.AddRule(new SenderAuthenticityRule());
            phishing_engine_.AddRule(new SenderDomainAlignmentRule());
            phishing_engine_.AddRule(new SuspiciousSenderIPAddressRule());
            phishing_engine_.AddRule(new UrgencyLanguageRule("rules\\config\\UrgencyLanguageConfig.json"));

            phishing_engine_.AddRuleOverride(new AllowedDomainsRuleOverride(cache_manager_));
            phishing_engine_.AddRuleOverride(new AllowedSendersRuleOverride(cache_manager_));
        }

        private void ScanEmailForPhishing(MailItem mail)
        {
            MailItemAdapter mail_adapter = new MailItemAdapter(mail);
            var analysis_result = phishing_engine_.Analyze(mail_adapter);
            phishingPane.ViewModel.CurrentSender = mail_adapter.SenderEmail;
            phishingPane.ViewModel.CurrentDomain = mail_adapter.SenderEmail.Split('@').LastOrDefault() ?? "";
            phishingPane.Update(analysis_result, mail_adapter.SenderEmail, mail_adapter.SenderEmail.Split('@').LastOrDefault() ?? "");
        }

        private void HookExplorerSelection()
        {
            var explorer = Application.ActiveExplorer();
            explorer.SelectionChange += Explorer_SelectionChange;
        }

        private void Explorer_SelectionChange()
        {
            var explorer = Application.ActiveExplorer();
            if (explorer.Selection.Count != 1)
                return;

            if (explorer.Selection[1] is MailItem mail)
            {
                Task.Run(() =>
                {
                    try
                    {
                        ScanEmailForPhishing(mail);
                    }
                    catch (COMException)
                    {
                        // ignoruj, spróbujesz przy kolejnym SelectionChange
                    }
                });
            }
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
