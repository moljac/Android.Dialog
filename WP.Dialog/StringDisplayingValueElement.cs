using System;
using System.Windows;
using System.Windows.Controls;

namespace WP.Dialog
{
    public abstract class StringDisplayingValueElement<T> : ValueElement<T>
    {
        public int FontSize { get; set; }
        
        protected readonly Grid GridUiElement;
        protected readonly TextBlock LabelUiElement;
        protected readonly TextBlock ValueUiElement;

        protected StringDisplayingValueElement(string caption, T value, string layoutName)
            : base(caption, value, layoutName)
        {
            GridUiElement = new Grid();
            GridUiElement.RowDefinitions.Add(new RowDefinition());
            GridUiElement.RowDefinitions.Add(new RowDefinition());

            LabelUiElement = new TextBlock();
            Grid.SetRow(LabelUiElement, 0);
            GridUiElement.Children.Add(LabelUiElement);


            LabelUiElement.Text = "DINGS";

            ValueUiElement = new TextBlock();
            Grid.SetRow(ValueUiElement, 1);
            GridUiElement.Children.Add(ValueUiElement);

            ValueUiElement.Text = "DANGS";
        }

        protected override void UpdateDetailDisplay()
        {
            if (ValueUiElement != null)
            {
                ValueUiElement.Text = Format(Value);
            }
        }

        protected override void UpdateCaptionDisplay()
        {
            if(LabelUiElement == null)
            {
                return;
            }
            LabelUiElement.Text = Caption;
            LabelUiElement.Visibility = string.IsNullOrEmpty(Caption) ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void UpdateCellDisplay()
        {
            UpdateDetailDisplay();
            base.UpdateCellDisplay();
        }

        protected abstract string Format(T value);

        public virtual bool Matches(string text)
        {
            return Caption != null && Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        protected override UIElement GetViewImpl()
        {
            return GridUiElement;
        }
    }
}