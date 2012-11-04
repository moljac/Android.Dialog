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
using Dialog.Core.Elements;

namespace WP.Dialog
{
    /// <summary>
    /// Used by root elements to fetch information when they need to
    /// render a summary (Checkbox count or selected radio group).
    /// </summary>
    public class Group : IGroup
    {
        public string Key { get; set; }

        protected Group()
        {
        }

        public Group(string key)
        {
            Key = key;
        }
    }

}
