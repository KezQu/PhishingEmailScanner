namespace PhishingEmailScanner
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
            this.PhishingScannerTab = this.Factory.CreateRibbonTab();
            this.PhishingScannerGroup = this.Factory.CreateRibbonGroup();
            this.IsEmailSafe = this.Factory.CreateRibbonCheckBox();
            this.PhishingScannerTab.SuspendLayout();
            this.PhishingScannerGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhishingScannerTab
            // 
            this.PhishingScannerTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.PhishingScannerTab.Groups.Add(this.PhishingScannerGroup);
            this.PhishingScannerTab.Label = "TabAddIns";
            this.PhishingScannerTab.Name = "PhishingScannerTab";
            // 
            // PhishingScannerGroup
            // 
            this.PhishingScannerGroup.DialogLauncher = ribbonDialogLauncherImpl1;
            this.PhishingScannerGroup.Items.Add(this.IsEmailSafe);
            this.PhishingScannerGroup.Label = "PhishingScanner";
            this.PhishingScannerGroup.Name = "PhishingScannerGroup";
            // 
            // IsEmailSafe
            // 
            this.IsEmailSafe.Label = "IsEmailSafe";
            this.IsEmailSafe.Name = "IsEmailSafe";
            this.IsEmailSafe.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.EmailSafe_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Read, Microsoft.Outlook.Post.R" +
    "ead, Microsoft.Outlook.Response.Read";
            this.Tabs.Add(this.PhishingScannerTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.PhishingScannerTab.ResumeLayout(false);
            this.PhishingScannerTab.PerformLayout();
            this.PhishingScannerGroup.ResumeLayout(false);
            this.PhishingScannerGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab PhishingScannerTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup PhishingScannerGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox IsEmailSafe;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
