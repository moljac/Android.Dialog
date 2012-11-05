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
using WP.Dialog.Enums;
using WP.Dialog.Helpers;

namespace WP.Dialog
{
    public class EntryElement : ValueElement<string>
    {
        protected readonly Grid GridUiElement;
        protected readonly TextBlock LabelUiElement;
        protected FrameworkElement ValueUiElement;


        public EntryElement(string caption = null, string hint = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_textfieldright")
        {
            Hint = hint;

            GridUiElement = new Grid();
            GridUiElement.RowDefinitions.Add(new RowDefinition());
            GridUiElement.RowDefinitions.Add(new RowDefinition());

            LabelUiElement = new TextBlock();
            Grid.SetRow(LabelUiElement, 0);
            GridUiElement.Children.Add(LabelUiElement);

        }

        protected override void UpdateCellDisplay()
        {
            UpdateDetailDisplay();
            base.UpdateCellDisplay();
        }

        protected override void UpdateCaptionDisplay()
        {
            if (LabelUiElement != null)
            {
                LabelUiElement.Text = Caption;
            }
        }

        protected override void UpdateDetailDisplay()
        {
            if (GridUiElement == null || ValueUiElement != null)
            {
                return;
            }

            if (Password)
            {
                ValueUiElement = new PasswordBox {Password = Value ?? ""};
            }
            else
            {
                var textBox = new TextBox();

                if(IsEmail)
                {
                    textBox.InputScope = InputScopeNameValue.EmailNameOrAddress.ToInputScope();
                }

                if(Numeric)
                {
                    textBox.InputScope = InputScopeNameValue.Number.ToInputScope();
                }

                if(Lines > 1)
                {
                    textBox.AcceptsReturn = true;
                }

                if (Value != null)
                {
                    textBox.Text = Value;
                }

                ValueUiElement = textBox;
            }

            Grid.SetRow(ValueUiElement, 1);
            GridUiElement.Children.Add(ValueUiElement);
          
            //if (Lines > 1)
            //{
            //    inputType |= InputTypes.TextFlagMultiLine;
            //    _entry.SetLines(Lines);
            //}
            //else if (Send != null)
            //{
            //    _entry.ImeOptions = ImeAction.Go;
            //    _entry.SetImeActionLabel("Go", ImeAction.Go);
            //}
            //else
            //{
            //    _entry.ImeOptions = ReturnKeyType.ImeActionFromUIReturnKeyType();
            //}

            //_entry.InputType = inputType;
        }

        public override string Summary()
        {
            return Value;
        }

        private bool _password;
        public bool Password
        {
            get { return _password; }
            set { _password = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private bool _isEmail;
        public bool IsEmail
        {
            get { return _isEmail; }
            set
            { _isEmail = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        public bool _numeric;
        public bool Numeric
        {
            get { return _numeric; }
            set { _numeric = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private int _rows;
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

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
        private Action _send;
        public Action Send
        {
            get { return _send; }
            set { _send = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        protected override UIElement GetViewImpl()
        {
            return GridUiElement;
        }
        //protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        //{
        //    var view = DroidResources.LoadStringEntryLayout(context, convertView, parent, LayoutName);
        //    if (view != null)
        //    {
        //        view.FocusableInTouchMode = false;
        //        view.Focusable = false;
        //        view.Clickable = false;

        //        TextView label;
        //        EditText _entry;
        //        DroidResources.DecodeStringEntryLayout(context, view, out label, out _entry);

        //        _entry.FocusableInTouchMode = true;
        //        _entry.Focusable = true;
        //        _entry.Clickable = true;

        //        var helper = EntryElementHelper.EnsureTagged(_entry);
        //        helper.Owner = this;
        //    }

        //    return view;
        //}

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

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

        public virtual void OnTextChanged(string newText)
        {
            OnUserValueChanged(newText);
        }

        //public virtual void OnEditorAction(TextView.EditorActionEventArgs e)
        //{
        //    if (e.ActionId == ImeAction.Go)
        //    {
        //        Send();
        //    }
        //}
    }
}
