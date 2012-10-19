using System;

namespace Android.Dialog
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class RadioSelectionAttribute : Attribute
    {
        public string Target;

        public RadioSelectionAttribute(string target)
        {
            Target = target;
        }
    }
}