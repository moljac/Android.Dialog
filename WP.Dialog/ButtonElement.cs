using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WP.Dialog
{
    public class ButtonElement : Element
    {
        public ButtonElement(string caption = null, EventHandler tapped = null)
            : base(caption, "dialog_button")
        {
            Click = tapped;
            _button = new Button();
            _button.Click += (s, e) => OnClick();
        }

        private readonly Button _button;

        protected override void UpdateCaptionDisplay()
        {
            if (_button != null)
            {
                _button.Content = Caption;
            }
            base.UpdateCaptionDisplay();
        }

        protected override UIElement GetViewImpl()
        {
            return _button;
        }

        public override string Summary()
        {
            return Caption;
        }

        public void OnClick()
        {
            if (Click != null)
                Click(this, EventArgs.Empty);

            if (SelectedCommand != null)
            {
                // TODO should we have a SelectedCommandParameter here?
                if (SelectedCommand.CanExecute(null))
                {
                    SelectedCommand.Execute(null);
                }
            }
        }
    }
}
