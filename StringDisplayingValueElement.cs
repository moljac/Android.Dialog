using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public abstract class StringDisplayingValueElement<T> : ValueElement<T>
    {
        public int FontSize { get; set; }

        protected StringDisplayingValueElement(string caption)
            : base(caption)
        {
        }

        protected StringDisplayingValueElement(string caption, int layoutId)
            : base(caption, layoutId)
        {
        }

        protected override void UpdateDetailDisplay(Views.View cell)
        {
            if (cell == null)
                return;

            TextView label;
            TextView value;
            DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);

            if (value != null)
                value.Text = Format(Value);
        }

        protected override void UpdateCaptionDisplay(View cell)
        {
            if (cell == null)
                return;

            TextView label;
            TextView value;
            DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);
            label.Text = Caption;
            label.Visibility = Caption == null ? ViewStates.Gone : ViewStates.Visible;
        }

        protected override void UpdateCellDisplay(View cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected abstract string Format(T value);

        protected override Views.View GetViewImpl(Content.Context context, Views.View convertView, Views.ViewGroup parent)
        {
            var view = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId);
            if (view != null)
            {
                if (FontSize > 0)
                {
                    TextView label;
                    TextView value;
                    DroidResources.DecodeStringElementLayout(Context, CurrentAttachedCell, out label, out value);
                    label.TextSize = FontSize;
                    value.TextSize = FontSize;
                }
            }
            return view;
        }
    }
}