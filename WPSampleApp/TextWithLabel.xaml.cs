using System.Windows.Controls;

namespace WPSampleApp
{
    public partial class TextWithLabel : UserControl
    {
        public TextWithLabel()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return CaptionTextBlock.Text; }
            set { CaptionTextBlock.Text = value; }
        }

        public string Text
        {
            get { return TextTextBlock.Text; }
            set { TextTextBlock.Text = value; }
        }
    }
}
