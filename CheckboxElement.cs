using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class CheckboxElement : BoolElement, CompoundButton.IOnCheckedChangeListener
    {
        public string SubCaption { get; set; }

        public bool ReadOnly
        {
            get;
            set;
        }

        public string Group;

        public CheckboxElement(string caption)
            : base(caption, false, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {

        }

        public CheckboxElement(string caption, bool value)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {
        }

        public CheckboxElement(string caption, bool value, string subCaption, string group)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_boolfieldsubright)
        {
            Group = group;
            SubCaption = subCaption;
        }

        public CheckboxElement(string caption, bool value, string group)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_boolfieldright)
        {
            Group = group;
        }

        public CheckboxElement(string caption, bool value, string group, int layoutId)
            : base(caption, value, layoutId)
        {
            Group = group;
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            View checkboxView;
            View view = DroidResources.LoadBooleanElementLayout(context, convertView, parent, LayoutId);
            return view;
        }

        protected override void  UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
 	        base.UpdateCellDisplay(cell);
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawCheckboxView);

            var _checkbox = (CheckBox)_rawCheckboxView;
            _checkbox.SetOnCheckedChangeListener(null);
            _checkbox.Checked = Value;
            _checkbox.SetOnCheckedChangeListener(this);
            _checkbox.Clickable = !ReadOnly;
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, cell, out _caption, out _subCaption, out _rawCheckboxView);
            _caption.Text = Caption;

            if (_subCaption != null)
            {
                _subCaption.Text = SubCaption;
            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            OnUserValueChanged(isChecked);
        }

        public override void Selected()
        {
            if (ReadOnly)
                return;

            if (CurrentAttachedCell == null)
            {
                // how on earth did this happen!
                return;
            }

            TextView _caption;
            TextView _subCaption;
            View _rawCheckboxView;
            DroidResources.DecodeBooleanElementLayout(Context, CurrentAttachedCell, out _caption, out _subCaption, out _rawCheckboxView);

            var _checkbox = (CheckBox)_rawCheckboxView;
            _checkbox.Toggle();
        }

        public override string Summary()
        {
            return Value ? "On" : "Off"; //Because iOS, that's why.
        }
    }
}