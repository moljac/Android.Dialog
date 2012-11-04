using System.Windows;
using System.Windows.Controls;

namespace WP.Dialog
{
    /// <summary>
    /// Used to display toggle button on the screen.
    /// </summary>
    public class BooleanElement : BoolElement
    {

        private readonly CheckBox _checkBox;

        public BooleanElement(string caption = null, bool value = false, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_onofffieldright")
        {
            _checkBox = new CheckBox();
        }

        protected override UIElement GetViewImpl()
        {
            return _checkBox;
        }

        protected override void UpdateCellDisplay()
        {
            UpdateDetailDisplay();
            base.UpdateCellDisplay();
        }

        protected override void UpdateCaptionDisplay()
        {
            if (_checkBox != null)
            {
                _checkBox.Content = Caption;
            }
        }

        protected override void UpdateDetailDisplay()
        {
            if(_checkBox == null)
            {
                return;
            }

            _checkBox.IsChecked = Value;
        }

        public void OnCheckedChanged()
        {
            OnUserValueChanged(true);
        }

        public override void Selected()
        {
            _checkBox.IsChecked = !_checkBox.IsChecked;
        }
    }
}