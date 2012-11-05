using System;
using System.Windows;
using System.Windows.Input;
using Dialog.Core.Elements;

namespace WP.Dialog
{
    public abstract class Element : IElement, IDisposable
    {
        public static readonly DependencyProperty TagProperty =
            DependencyProperty.Register("Tag", typeof (int), typeof (Element), new PropertyMetadata(0));

        public string LayoutName { get; set; }
        
        private static int _currentElementID = 1;
        private string _caption;

        /// <summary>
        /// An app unique identifier for this element. 
        /// Note that it is expected that Elements will always created on the UI thread - so no locking is used on CurrentElementID
        /// </summary>
        private readonly int _elementID = _currentElementID++;

        protected Element(string caption)
        {
            Caption = caption;
        }

        protected Element(string caption, string layoutName)
            : this(caption)
        {
            LayoutName = layoutName;
        }

        /// <summary>
        ///  The caption to display for this given element
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; UpdateCaptionDisplay(); }
        }

        /// <summary>
        ///  Handle to the container object.
        /// </summary>
        /// <remarks>
        /// For sections this points to a RootElement, for every other object this points to a Section and it is null
        /// for the root RootElement.
        /// </remarks>
        public Element Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Override for click the click event
        /// </summary>
        public EventHandler Click { get; set; }

        /// <summary>
        /// Override for long click events, some elements use this for action
        /// </summary>
        public EventHandler LongClick { get; set; }

        public virtual void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Override this method if you want some other action to be taken when
        /// a cell view is set
        /// </summary>
        protected virtual void UpdateCellDisplay()
        {
            UpdateCaptionDisplay();
        }

        protected virtual void UpdateCaptionDisplay()
        {
            // by default do nothing!
        }

        private ICommand _selectedCommand;
        public ICommand SelectedCommand
        {
            get { return _selectedCommand; }
            set { _selectedCommand = value; }
        }


        /// <summary>
        /// The last cell attached to this Element
        /// Use the Tag property of the Cell to determine of this cell is still attached to this Element
        /// </summary>
        private UIElement _lastAttachedCell;

        protected UIElement CurrentAttachedCell
        {
            get
            {
                if (_lastAttachedCell == null)
                    return null;

                if (!_elementID.Equals(_lastAttachedCell.GetValue(TagProperty)))
                    _lastAttachedCell = null;

                return _lastAttachedCell;
            }
            private set
            {
                _lastAttachedCell = value;
                _lastAttachedCell.SetValue(TagProperty, _elementID);
            }
        }

        /// <summary>
        /// Returns a summary of the value represented by this object, suitable 
        /// for rendering as the result of a RootElement with child objects.
        /// </summary>
        /// <returns>
        /// The return value must be a short description of the value.
        /// </returns>
        public virtual string Summary()
        {
            return string.Empty;
        }


        /// <summary>
        /// Overriden by most derived classes, creates an UIElement with the contents for display
        /// </summary>
        /// <returns></returns>
        protected virtual UIElement GetViewImpl()
        {
            throw new NotImplementedException("GetViewImpl should be implemented in derived Element classes");
        }
        
        public virtual UIElement GetView()
        {
            return GetViewImpl();
        }

        public virtual void Selected() { }

        public virtual bool Matches(string text)
        {
            return Caption != null && Caption.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        #region MonoTouch Dialog Mimicry

        // Not used in any way, just there to match MT Dialog api.
        public UITableViewCellAccessory Accessory
        {
            get { return accessory; }
            set { accessory = value; }
        }
        private UITableViewCellAccessory accessory;
        private Element _parent;


        public void ActOnCurrentAttachedCell(Action updateAction)
        {
            //var cell = CurrentAttachedCell;
            // note that we call the update action even if the attached cell is null
            // - as some elements use fixed UIViews (e.g. sliders) which are independent of the cell
            updateAction();
        }

        #endregion

    }
    
}