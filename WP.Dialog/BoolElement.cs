using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WP.Dialog
{
    public abstract class BoolElement : ValueElement<bool>
    {
        public string TextOff { get; set; }
        public string TextOn { get; set; }

        protected BoolElement(string caption, bool value, string layoutName = null)
            : base(caption, value, layoutName)
        {
        }

        public override string Summary()
        {
            return Value ? TextOn : TextOff;
        }
    }
}
