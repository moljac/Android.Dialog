﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace Android.Dialog
{
    public class EntryElement : ValueElement<string>, ITextWatcher
    {
        protected override void UpdateDetailDisplay(View cell)
        {
            if (_entry != null && _entry.Text != Value)
                _entry.Text = Value;
        }

        public EntryElement(string caption, string value)
            : this(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldright)
        {
        }

        public EntryElement(string caption, string hint, string value)
            : this(caption, value)
        {
            Hint = hint;
        }

        public EntryElement(string caption, string value, int layoutId)
            : base(caption, layoutId)
        {
            _val = value;
            Lines = 1;
        }

        public override string Summary()
        {
            return _val;
        }

        public bool Password { get; set; }
        public bool IsEmail
        {
            get
            {
                var type = AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.EmailAddress];
                return (_entry.InputType & type) == type;
            }
            set { if (value) _entry.InputType = AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.EmailAddress]; }
        }

        public bool Numeric
        {
            get
            {
                var type = AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.DecimalPad];
                return (_entry.InputType & type) == type;
            }
            set { if (value) _entry.InputType = AndroidDialogEnumHelper.KeyboardTypeMap[UIKeyboardType.DecimalPad]; }
        }

        public string Hint { get; set; }

        public int Rows { get; set; }
        public int Lines
        {
            get { return Rows; }
            set { Rows = value; }
        }

        /// <summary>
        /// An action to perform when Enter is hit
        /// </summary>
        /// <remarks>This is only meant to be set if this is the last field in your RootElement, to allow the Enter button to be used for submitting the form data.<br>
        /// If you want to perform an action when the text changes, consider hooking into Changed instead.</remarks>
        public Action Send { get; set; }

        protected EditText _entry;
        private string _val;

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            TextView label;
            var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutId, out label, out _entry);
            if (view != null)
            {
                view.FocusableInTouchMode = false;
                view.Focusable = false;
                view.Clickable = false;

                _entry.FocusableInTouchMode = true;
                _entry.Focusable = true;
                _entry.Clickable = true;

                _entry.Text = Value;
                _entry.Hint = Hint;

                _entry.InputType = KeyboardType.InputTypesFromUIKeyboardType();

                if (Password)
                    _entry.InputType |= InputTypes.TextVariationPassword;

                if (Lines > 1)
                {
                    _entry.InputType |= InputTypes.TextFlagMultiLine;
                    _entry.SetLines(Lines);
                }
                else if (Send != null)
                {
                    _entry.ImeOptions = ImeAction.Go;
                    _entry.SetImeActionLabel("Go", ImeAction.Go);
                }
                else _entry.ImeOptions = ReturnKeyType.ImeActionFromUIReturnKeyType();

                if (_entry.Tag != this)
                {
                    _entry.AddTextChangedListener(this);
                    if (Send != null)
                        _entry.EditorAction += _entry_EditorAction;
                }

                _entry.Tag = this;
                if (label == null)
                {
                    _entry.Hint = Caption;
                }
                else
                {
                    label.Text = Caption;
                }
            }

            return view;
        }

        protected void _entry_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == ImeAction.Go)
            {
                Send();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_entry.Dispose();
                _entry = null;
            }
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

        #region TextWatcher Android

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            OnUserValueChanged(s.ToString());
        }

        public void AfterTextChanged(IEditable s)
        {
            // nothing needed
        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            // nothing needed
        }

        #endregion

        #region MonoTouch Dialog Mimicry

        public UIKeyboardType KeyboardType
        {
            get { return keyboardType; }
            set { keyboardType = value; }
        }
        private UIKeyboardType keyboardType;

        public UIReturnKeyType ReturnKeyType
        {
            get { return returnKeyType; }
            set { returnKeyType = value; }
        }
        private UIReturnKeyType returnKeyType;

        // Not used in any way, just there to match MT Dialog api.
        public UITextFieldViewMode ClearButtonMode
        {
            get { return clearButtonMode; }
            set { clearButtonMode = value; }
        }
        private UITextFieldViewMode clearButtonMode;

        #endregion
    }
}