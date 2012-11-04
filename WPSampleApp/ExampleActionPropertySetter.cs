using System;
using System.Collections.Generic;
using System.Reflection;
using Dialog.Core.Builder;

namespace WPSampleApp
{
    public class ExampleActionPropertySetter : IPropertySetter
    {
        public Dictionary<string, EventHandler> Actions { get; private set; }

        public ExampleActionPropertySetter()
        {
            Actions = new Dictionary<string, EventHandler>();
        }

        public void Set(object element, PropertyInfo property, string configuration)
        {
            var action = Actions[configuration];
            property.GetSetMethod().Invoke(element, new object[] { action });
        }
    }
}