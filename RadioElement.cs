using System;
using Android.Content;
using Android.Views;

namespace Android.Dialog
{
    public class RadioElement : StringElement
    {
        public string Group;
        internal int RadioIdx;

        public RadioElement(string caption, string group)
            : base(caption)
        {
            Group = group;
        }

        public RadioElement(string caption)
            : base(caption)
        {
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            if (!(((RootElement)Parent.Parent)._group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            return base.GetViewImpl(context, convertView, parent);
        }

        public override string Summary()
        {
            return Caption;
        }
    }
}