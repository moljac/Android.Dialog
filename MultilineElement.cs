using Android.Content;
using Android.Views;

namespace Android.Dialog
{
    public class MultilineEntryElement : EntryElement
    {
        public int MaxLength { get; set; }
        public MultilineEntryElement(string caption, string value)
            : base(caption, value, (int)DroidResources.ElementLayout.dialog_textfieldbelow)
        {
            Lines = 3;
        }

        public override void OnTextChanged(string newText)
        {
            if (MaxLength > 0 && newText.Length > MaxLength)
                Value = newText.Substring(0, MaxLength);
        }
    }
}