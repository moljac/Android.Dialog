namespace Android.Dialog
{
    public class StringMultilineElement : StringElement
    {
        public StringMultilineElement(string caption) : base(caption) { }
        public StringMultilineElement(string caption, string value) : base(caption, value) { }
        public StringMultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName) { }
    }
}