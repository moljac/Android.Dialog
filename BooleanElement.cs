using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public abstract class BoolElement : ValueElement<bool>
    {
        public string TextOff { get; set; }
        public string TextOn { get; set; }

        public BoolElement(string caption, bool value)
            : base(caption)
        {
        }

        public BoolElement(string caption, bool value, int layoutId)
            : base(caption, layoutId)
        {
        }

        public override string Summary()
        {
            return Value ? TextOn : TextOff;
        }
    }

    /// <summary>
    /// Used to display toggle button on the screen.
    /// </summary>
    public class BooleanElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
        public BooleanElement(string caption, bool value)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_onofffieldright)
        {
        }

        public BooleanElement(string caption, bool value, int layoutId)
            : base(caption, value, layoutId)
        {
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId);
            return view;
        }

        protected override void  UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
 	        base.UpdateCellDisplay(cell);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawToggleButton);
            _caption.Text = Caption;
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawToggleButton);
            ToggleButton _toggleButton = (ToggleButton)_rawToggleButton;
            _toggleButton.SetOnCheckedChangeListener(null);
            _toggleButton.Checked = Value;
            _toggleButton.SetOnCheckedChangeListener(this);

            if (TextOff != null)
            {
                _toggleButton.TextOff = TextOff;
                if (!Value)
                    _toggleButton.Text = TextOff;
            }

            if (TextOn != null)
            {
                _toggleButton.TextOn = TextOn;
                if (Value)
                    _toggleButton.Text = TextOn;
            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            OnUserValueChanged(isChecked);
        }

        public override void Selected()
        {
            if (CurrentAttachedCell == null)
            {
                // how did this happen?!
                return;
            }

            View _rawToggleButton;
            TextView _caption;
            TextView _subCaption;

            DroidResources.DecodeBooleanElementLayout(Context, CurrentAttachedCell, out _caption, out _subCaption, out _rawToggleButton);
            ToggleButton _toggleButton = (ToggleButton)_rawToggleButton;

            if (_toggleButton != null)
                _toggleButton.Toggle();
        }
    }
}