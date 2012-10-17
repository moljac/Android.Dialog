using System.Globalization;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Android.Dialog
{
    public class FloatElement : ValueElement<float>, SeekBar.IOnSeekBarChangeListener
    {
        private const int precision = 10000000;
        public bool ShowCaption;
        private float _maxValue, _minValue;
        SeekBar slider;

        public float MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public float MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        protected override void UpdateDetailDisplay(View cell)
        {
            if (slider != null)
                slider.Progress = (int)(Value - _minValue) * precision;
        }

        public Bitmap Left;
        public Bitmap Right;

        public FloatElement(string caption)
            : this(caption, (int)DroidResources.ElementLayout.dialog_floatimage)
        {
            Value = 0;
            MinValue = 0;
            MaxValue = 1;
        }

        public FloatElement(string caption, int layoutId)
            : base(caption, layoutId)
        {
            Value = 0;
            MinValue = 0;
            MaxValue = 1;
        }

        public FloatElement(Bitmap left, Bitmap right, int value)
            : this(left, right, value, (int)DroidResources.ElementLayout.dialog_floatimage)
        {
        }

        public FloatElement(Bitmap left, Bitmap right, int value, int layoutId)
            : base(string.Empty, layoutId)
        {
            Left = left;
            Right = right;
            MinValue = 0;
            MaxValue = precision;
            Value = value;
        }

        protected override View GetViewImpl(Context context, View convertView, ViewGroup parent)
        {
            TextView label;
            ImageView left;
            ImageView right;

            View view = DroidResources.LoadFloatElementLayout(context, convertView, parent, LayoutId, out label, out slider, out left, out right);

            if (view != null)
            {
                if (left != null)
                {
                    if (Left != null)
                        left.SetImageBitmap(Left);
                    else
                        left.Visibility = ViewStates.Gone;
                }
                if (right != null)
                {
                    if (Right != null)
                        right.SetImageBitmap(Right);
                    else
                        right.Visibility = ViewStates.Gone;
                }
                slider.Max = (int)(_maxValue - _minValue) * precision;
                slider.Progress = (int)(Value - _minValue) * precision;
                slider.SetOnSeekBarChangeListener(this);
                if (label != null)
                {
                    if (ShowCaption)
                        label.Text = Caption;
                    else
                        label.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                Util.Log.Error("FloatElement", "GetViewImpl failed to load template view");
            }

            return view;
        }

        public override string Summary()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        void SeekBar.IOnSeekBarChangeListener.OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            OnUserValueChanged(((float)progress / (float)precision) - _minValue);
        }

        void SeekBar.IOnSeekBarChangeListener.OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        void SeekBar.IOnSeekBarChangeListener.OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}