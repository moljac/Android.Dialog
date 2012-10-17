using System;
using Android.Views;

namespace Android.Dialog
{
    public abstract class ValueElement : Element
    {
        protected ValueElement(string caption) : base(caption)
        {
        }

        protected ValueElement(string caption, int layoutId) : base(caption, layoutId)
        {
        }

        public event EventHandler ValueChanged;

        protected virtual void FireValueChanged()
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }

    public abstract class ValueElement<TValueType> : ValueElement
    {
        protected ValueElement(string caption) : base(caption)
        {
        }

        protected ValueElement(string caption, int layoutId) : base(caption, layoutId)
        {
        }

        public TValueType Value
        {
            get { return _value; }
            set { _value = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private TValueType _value;

        protected void OnUserValueChanged(TValueType newValue)
        {
            Value = newValue;
            FireValueChanged();
        }

        protected abstract void UpdateDetailDisplay(View cell);
    }
}