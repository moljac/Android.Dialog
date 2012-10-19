namespace Android.Dialog
{
    public class StyledMultilineElement : StringElement
    {
        public StyledMultilineElement(string caption) : base(caption) { }
        public StyledMultilineElement(string caption, string value) : base(caption, value) { }
        public StyledMultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName) { }
    }
}