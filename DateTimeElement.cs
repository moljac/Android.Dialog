using System;
using Android.App;
using Android.Text.Format;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class DateTimeElement : ValueElement<DateTime>
    {
        protected TextView _caption;
        protected TextView _text;

        public int FontSize { get; set; }

        public int MinuteInterval { get; set; }

        protected override void UpdateDetailDisplay(Views.View cell)
        {
            if (_text != null)
                _text.Text = Format(Value);
        }

        protected override Views.View GetViewImpl(Content.Context context, Views.View convertView, Views.ViewGroup parent)
        {
            var view = DroidResources.LoadStringElementLayout(context, convertView, parent, LayoutId, out _caption, out _text);
            if (view != null && _caption != null && _text != null)
            {
                _caption.Text = Caption;
                _caption.Visibility = Caption == null ? ViewStates.Gone : ViewStates.Visible;
                _text.Text = Format(Value);
                if (FontSize > 0)
                {
                    _caption.TextSize = FontSize;
                    _text.TextSize = FontSize;
                }
            }
            return view;
        }

        public DateTimeElement(string caption, DateTime date)
            : base(caption)
        {
            Click = delegate { EditDate(); };
        }

        public DateTimeElement(string caption, DateTime date, int layoutId)
            : base(caption, layoutId)
        {
            Click = delegate { EditDate(); };
        }

        public virtual string Format(DateTime dt)
        {
            return dt.ToShortDateString() + " " + dt.ToShortTimeString();
        }

        protected void EditDate()
        {
            var context = GetContext();
            if (context == null)
            {
                Util.Log.Warn("DateElement", "No Context for Edit");
                return;
            }
            var val = Value;
            new DatePickerDialog(context, DateCallback ?? OnDateTimeSet, val.Year, val.Month - 1, val.Day).Show();
        }

        protected void EditTime()
        {
            var context = GetContext();
            if (context == null)
            {
                Util.Log.Warn("TimeElement", "No Context for Edit");
                return;
            }
            DateTime val = Value;
            new TimePickerDialog(context, OnTimeSet, val.Hour, val.Minute, DateFormat.Is24HourFormat(context)).Show();
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0));
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateTimeSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0));
            EditTime();
        }

        // the event received when the user "sets" the time in the dialog
        protected void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(current.Year, current.Month, current.Day, e.HourOfDay, e.Minute, 0));
        }

        protected EventHandler<DatePickerDialog.DateSetEventArgs> DateCallback = null;
    }

    public class DateElement : DateTimeElement
    {
        public DateElement(string caption, DateTime date)
            : base(caption, date)
        {
            DateCallback = OnDateSet;
        }

        public DateElement(string caption, DateTime date, int layoutId)
            : base(caption, date, layoutId)
        {
            DateCallback = OnDateSet;
        }

        public override string Format(DateTime dt)
        {
            return dt.ToShortDateString();
        }
    }

    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime date)
            : base(caption, date)
        {
            Click = delegate { EditTime(); };
        }

        public TimeElement(string caption, DateTime date, int layoutId)
            : base(caption, date, layoutId)
        {
            Click = delegate { EditTime(); };
        }

        public override string Format(DateTime dt)
        {
            return dt.ToShortTimeString();
        }
    }
}