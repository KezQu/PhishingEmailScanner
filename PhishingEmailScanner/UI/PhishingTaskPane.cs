using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace PhishingEmailScanner.UI
{
    public partial class PhishingTaskPane : UserControl
    {
        private readonly PhishingView _view;
        private readonly PhishingViewModel _viewModel;
        public PhishingTaskPane(WhitelistCacheManager whitelistCacheManager)
        {
            InitializeComponent();

            _viewModel = new PhishingViewModel(whitelistCacheManager);
            _view = new PhishingView
            {
                DataContext = _viewModel
            };

            var host = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = _view
            };

            Controls.Add(host);

            _viewModel.StatusText = "Panel phishingowy działa";
        }

        public PhishingViewModel ViewModel => _viewModel;

        public void Update(PhishingAnalysisResult result, string sender, string domain)
        {
            if(_view.Dispatcher.CheckAccess())
            {
                _viewModel.Update(result, sender, domain);
            }
            else
            {
                _view.Dispatcher.Invoke(() =>
                {
                    _viewModel.Update(result, sender, domain);
                });
            }
        }
    }
}
