//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using Outlook = Microsoft.Office.Interop.Outlook;
//using Office = Microsoft.Office.Core;

using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Outlook;
using System.Windows.Forms;

namespace PhishingEmailScanner
{
    public partial class ThisAddIn
    {
        private Items inboxItems;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            var inbox = this.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);

            inboxItems = inbox.Items;
            inboxItems.ItemAdd += InboxItems_ItemAdd;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Uwaga: program Outlook nie generuje już tego zdarzenia. Jeśli masz kod, który 
            //    musi działać po zamknięciu programu Outlook, zobacz https://go.microsoft.com/fwlink/?LinkId=506785
        }

        private void InboxItems_ItemAdd(object item )
        {
            if(item is MailItem mail)
            {
                AnalyzeMail(mail);
            }
        }

        private void AnalyzeMail(MailItem mail)
        {
            if (IsPhishing(mail))
            {
                MarkAsPhishing(mail);
                MessageBox.Show("Uwaga! Ta wiadomość może być phishingiem!",
                    "Ostrzeżenie bezpieczeństwa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private bool IsPhishing(MailItem mail )
        {
            var subject = mail.Subject?.ToLower() ?? "";

            var testKeyword = "test_wzirni";

            if(subject.Contains(testKeyword))
            {
                return true;
            }

            return false;
        }

        private void MarkAsPhishing(MailItem mail )
        {
            mail.Categories = "Phishing";
            mail.Save();
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
