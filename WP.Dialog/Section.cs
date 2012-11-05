using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dialog.Core.Elements;

namespace WP.Dialog
{
    public class Section : Element, IEnumerable<Element>, ISection
    {
#warning More to do here!

        public List<Element> Elements = new List<Element>();

        private readonly List<string> ElementTypes = new List<string>();

        private object footer;
        private object header;

        private TextBlock _headerTextBlock;
        private TextBlock _footerTextBlock;

        /// <summary>
        ///  Constructs a Section without header or footers and an hidden section block
        /// </summary>
        public Section()
            : this((string)null) { }

        /// <summary>
        ///  Constructs a Section with the specified header
        /// </summary>
        /// <param name="caption">
        /// The header to display
        /// </param>
        public Section(string caption)
            : base(caption)
        {
        }

        /// <summary>
        /// Constructs a Section with a header and a footer
        /// </summary>
        /// <param name="caption">
        /// The caption to display (or null to not display a caption)
        /// </param>
        /// <param name="footer">
        /// The footer to display.
        /// </param>
        public Section(string caption, string footer)
            : this(caption)
        {
            Footer = footer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="header">The header, either a <see cref="String"/> for a simple header, or a custom <see cref="Element"/> (likely a <see cref="ViewElement"/>).</param>
        /// <param name="footer">The footer, either a <see cref="String"/> for a simple footer, or a custom <see cref="Element"/> (likely a <see cref="ViewElement"/>).</param>
        public Section(object header, object footer)
            : this()
        {
            this.header = header;
            this.footer = footer;
        }

        /// <summary>
        ///    The section header, as a string
        /// </summary>
        public string Header
        {
            get { return Caption; }
            set { Caption = value; }
        }

        /// <summary>
        /// The section footer, as a string.
        /// </summary>
        public string Footer
        {
            get { return footer == null ? null : footer.ToString(); }
            set { footer = value; }
        }

        ///// <summary>
        ///// The section's header view.  
        ///// </summary>
        //public Element HeaderView
        //{
        //    get { return header as Element; }
        //    set { header = value; }
        //}

        ///// <summary>
        ///// The section's footer view.
        ///// </summary>
        //public Element FooterView
        //{
        //    get { return footer as Element; }
        //    set { footer = value; }
        //}

        public int Count
        {
            get { return Elements.Count; }
        }

        public Element this[int idx]
        {
            get { return Elements[idx]; }
        }

        public event EventHandler ValueChanged;

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(sender, args);
        }

        /// <summary>
        /// Adds a new child Element to the Section
        /// </summary>
        /// <param name="element">
        /// An element to add to the section.
        /// </param>
        public void Add(IElement element)
        {
            if (element == null)
                return;

            var realElement = element as Element;
            if (realElement == null)
                throw new ArgumentException("Android needs real Element instances");

            var elementType = element.GetType().FullName;

            if (!ElementTypes.Contains(elementType))
                ElementTypes.Add(elementType);

            Elements.Add(realElement);
            realElement.Parent = this;

            // bind value changed to our local handler so section itself is aware of events, allows cascacding upward notifications
            if (realElement is ValueElement)
                (realElement as ValueElement).ValueChanged += HandleValueChangedEvent;
        }

        /// <summary>Add version that can be used with LINQ</summary>
        /// <param name="elements">
        /// An enumerable list that can be produced by something like:
        ///    from x in ... select (Element) new MyElement (...)
        /// </param>
        public int Add(IEnumerable<Element> elements)
        {
            int count = 0;
            foreach (Element e in elements)
            {
                Add(e);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Inserts a series of elements into the Section
        /// </summary>
        /// <param name="idx">
        /// The index where the elements are inserted
        /// </param>
        /// <param name="newElements">
        /// A series of elements.
        /// </param>
        public void Insert(int idx, params Element[] newElements)
        {
            if (newElements == null)
                return;

            foreach (var e in newElements)
            {
                Elements.Insert(idx++, e);
                e.Parent = this;
            }
        }

        /// <summary>
        /// Inserts a <see cref="IEnumerable{T}"/> of Elements into the Section
        /// </summary>
        /// <param name="idx">The index to insert the elements.</param>
        /// <param name="newElements">A series of elements.</param>
        /// <returns></returns>
        public int Insert(int idx, IEnumerable<Element> newElements)
        {
            if (newElements == null)
                return 0;

            int count = 0;
            foreach (var e in newElements)
            {
                Elements.Insert(idx++, e);
                e.Parent = this;
                count++;
            }

            return count;
        }

        public void Remove(Element e)
        {
            if (e == null)
                return;
            for (int i = Elements.Count; i > 0; )
            {
                i--;
                if (Elements[i] != e) continue;
                RemoveRange(i, 1);
                return;
            }
        }

        public void Remove(int idx)
        {
            RemoveRange(idx, 1);
        }

        /// <summary>
        /// Removes a range of elements from the Section
        /// </summary>
        /// <param name="start">
        /// Starting position
        /// </param>
        /// <param name="count">
        /// Number of elements to remove from the section
        /// </param>
        public void RemoveRange(int start, int count)
        {
            if (start < 0 || start >= Elements.Count)
                return;
            if (count == 0)
                return;


            if (start + count > Elements.Count)
                count = Elements.Count - start;

            Elements.RemoveRange(start, count);

            //var root = Parent as RootElement;
            //if (root == null)
            //    return;

            //int sidx = root.IndexOf(this);
            //var paths = new NSIndexPath[count];
            //for (int i = 0; i < count; i++)
            //    paths[i] = NSIndexPath.FromRowSection(start + i, sidx);
            //root.TableView.DeleteRows(paths, anim);
        }

        public void Clear()
        {
            foreach (Element e in Elements)
                e.Dispose();
            Elements = new List<Element>();

            //var root = Parent as RootElement;
            //if (root != null && root.TableView != null)
            //    root.TableView.ReloadData();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            Parent = null;
            Clear();
            Elements = null;
        }

        public int GetElementViewType(Element e)
        {
            var elementType = e.GetType().FullName;

            for (int i = 0; i < ElementTypes.Count; i++)
            {
                if (ElementTypes[i].Equals(elementType))
                    return i + 1;
            }

            return 0;
        }

        public int ElementViewTypeCount
        {
            get { return ElementTypes.Count; }
        }

        private StackPanel _stackPanel;

        protected override UIElement GetViewImpl()
        {
            if (_stackPanel == null)
            {
                _stackPanel = new StackPanel();

                _headerTextBlock = new TextBlock { Text = Header, HorizontalAlignment = HorizontalAlignment.Center};
                _stackPanel.Children.Add(_headerTextBlock);

                foreach (var element in Elements)
                {
                    _stackPanel.Children.Add(element.GetView());
                }

                _footerTextBlock = new TextBlock { Text = Footer, HorizontalAlignment = HorizontalAlignment.Center };
                _stackPanel.Children.Add(_footerTextBlock);
            }

            return _stackPanel;
        }

        /// <summary>
        /// Enumerator to get all the elements in the Section.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/>
        /// </returns>
        public IEnumerator<Element> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        /// <summary>
        /// Enumerator to get all the elements in the Section.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/>
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IElement ISection.HeaderView { get; set; }
        IElement ISection.FooterView { get; set; }

        void ISection.Add(IElement element)
        {
            Elements.Add((Element)element);
        }
    }

    public enum UITableViewCellAccessory
    {
        DisclosureIndicator,
    }

}