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

namespace WP.Dialog.Helpers
{
    public static class InputScopeExtensions
    {
        public static InputScope ToInputScope(this InputScopeNameValue inputScopeNameValue)
        {
            var inputScope = new InputScope();
            var inputScopeName = new InputScopeName { NameValue = inputScopeNameValue };
            inputScope.Names.Add(inputScopeName);
            return inputScope;
        }
    }
}
