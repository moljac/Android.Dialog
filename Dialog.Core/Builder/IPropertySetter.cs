using System.Reflection;

namespace Dialog.Core.Builder
{
    public interface IPropertySetter
    {
        void Set(object element, PropertyInfo property, string configuration);
    }
}