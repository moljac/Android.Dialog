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

namespace WP.Dialog
{
    public class RadioElement : StringElement
    {
        public string Group { get; set; }
        internal int RadioIdx;

        public RadioElement(string caption = null, string group = null)
            : base(caption)
        {
            Group = group;
        }

        protected override UIElement GetViewImpl()
        {
            if (!(((RootElement)Parent.Parent).Group is RadioGroup))
                throw new Exception("The RootElement's Group is null or is not a RadioGroup");

            return base.GetViewImpl();
        }

        public override string Summary()
        {
            return Caption;
        }
    }

    /// <summary>
    /// Captures the information about mutually exclusive elements in a RootElement
    /// </summary>
    public class RadioGroup : Group
    {
        public int Selected { get; set; }

        public RadioGroup()
        {
        }

        public RadioGroup(string key, int selected)
            : base(key)
        {
            Selected = selected;
        }

        public RadioGroup(int selected)
            : base(null)
        {
            Selected = selected;
        }
    }
}
