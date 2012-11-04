using System.Windows.Controls;

namespace WPSampleApp
{
    public partial class HeaderFooterGroup : UserControl
    {
        public HeaderFooterGroup()
        {
            InitializeComponent();
        }

        public string Header
        {
            get { return HeaderTextBlock.Text; }
            set { HeaderTextBlock.Text = value; }
        }

        public string Footer
        {
            get { return FooterTextBlock.Text; }
            set { FooterTextBlock.Text = value; }
        }
        
    }
}
