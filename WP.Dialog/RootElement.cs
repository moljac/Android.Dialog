﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dialog.Core.Elements;

namespace WP.Dialog
{
    public class RootElement : StringDisplayingValueElement<string>, IEnumerable<Section>, IRootElement
    {
        private Group _group;
        public IGroup Group { get { return _group; } set { _group = value as Group; } }

        public bool UnevenRows { get; set; }

        private ScrollViewer _scrollViewer;
        private StackPanel _stackPanel;

        //public Func<RootElement, View> _createOnSelected;
        public event EventHandler RadioSelectionChanged;

        public RootElement()
            : this(null)
        {
        }

        public RootElement(string caption, Group group = null, string layoutRoot = null)
            : base(caption, null, layoutRoot ?? "dialog_root")
        {
            this._group = group;
            Sections = new List<Section>();
            //Click = (o, e) => SelectRadio();
        }

        protected override UIElement GetViewImpl()
        {
            RefreshValue();

            if(_scrollViewer == null)
            {
                _scrollViewer = new ScrollViewer();
                _stackPanel = new StackPanel();
                _scrollViewer.Content = _stackPanel;
            }

            _stackPanel.Children.Clear();

            foreach (var section in Sections)
            {
                _stackPanel.Children.Add(section.GetView());
            }

            return _scrollViewer;
        }

        private void RefreshValue()
        {
            Value = GetSelectedValue() ?? Caption;
        }

        internal List<Section> Sections = new List<Section>();

        public int Count
        {
            get
            {
                return Sections.Count;
            }
        }

        public Section this[int idx]
        {
            get
            {
                return Sections[idx];
            }
        }

        private void HandleValueChangedEvent(object sender, EventArgs args)
        {
            base.FireValueChanged();
        }

        internal int IndexOf(Section target)
        {
            int idx = 0;
            foreach (Section s in Sections)
            {
                if (s == target)
                    return idx;
                idx++;
            }
            return -1;
        }

        internal void Prepare()
        {
            int current = 0;
            foreach (var element in Sections.SelectMany(s => s))
            {
                var re = element as RadioElement;
                if (re != null)
                    re.RadioIdx = current++;
                if (UnevenRows == false && element is IElementSizing)
                    UnevenRows = true;
            }
        }

        public override string Summary()
        {
            return GetSelectedValue();
        }

        /// <summary>
        /// Adds a new section to this RootElement
        /// </summary>
        /// <param name="section">
        /// The section to add, if the root is visible, the section is inserted with no animation
        /// </param>
        public void Add(ISection section)
        {
            if (section == null)
                return;

            var realSection = section as Section;
            if (realSection == null)
                throw new ArgumentException("Android RootElement should be passed a Section!");

            Sections.Add(realSection);
            realSection.Parent = this;
            realSection.ValueChanged += HandleValueChangedEvent;
            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        //
        // This makes things LINQ friendly;  You can now create RootElements
        // with an embedded LINQ expression, like this:
        // new RootElement ("Title") {
        //     from x in names
        //         select new Section (x) { new StringElement ("Sample") }
        //
        public void Add(IEnumerable<Section> sections)
        {
            foreach (var s in sections)
                Add(s);
        }

        /// <summary>
        /// Inserts a new section into the RootElement
        /// </summary>
        /// <param name="idx">
        /// The index where the section is added <see cref="System.Int32"/>
        /// </param>
        /// <param name="newSections">
        /// A <see cref="Section[]"/> list of sections to insert
        /// </param>
        /// <remarks>
        ///    This inserts the specified list of sections (a params argument) into the
        ///    root using the specified animation.
        /// </remarks>
        public void Insert(int idx, params Section[] newSections)
        {
            if (idx < 0 || idx > Sections.Count)
                return;
            if (newSections == null)
                return;

            //if (Table != null)
            //    Table.BeginUpdates();

            int pos = idx;
            foreach (var s in newSections)
            {
                s.Parent = this;
                s.ValueChanged += HandleValueChangedEvent;
                Sections.Insert(pos++, s);
            }

            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        /// <summary>
        /// Removes a section at a specified location
        /// </summary>
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= Sections.Count)
                return;

            Sections.RemoveAt(idx);
            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        public void Remove(Section s)
        {
            if (s == null)
                return;
            int idx = Sections.IndexOf(s);
            if (idx == -1)
                return;
            RemoveAt(idx);
            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        public void Clear()
        {
            foreach (var s in Sections)
                s.Dispose();
            Sections = new List<Section>();
            ActOnCurrentAttachedCell(UpdateDetailDisplay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Sections == null)
                    return;
                Clear();
                Sections = null;
            }
        }

        /// <summary>
        /// The currently selected Radio item in the whole Root.
        /// </summary>
        public int RadioSelected
        {
            get
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
                var radio = _group as RadioGroup;
                if (radio != null)
                    radio.Selected = value;
            }
        }

        private string GetSelectedValue()
        {
            var radio = _group as RadioGroup;
            if (radio == null)
                return string.Empty;

            int selected = radio.Selected;
            int current = 0;
            foreach (RadioElement e in Sections.SelectMany(s => s).OfType<RadioElement>())
            {
                if (current == selected)
                    return e.Summary();

                current++;
            }

            return string.Empty;
        }

        protected override string Format(string value)
        {
            return value;
        }

        public void SelectRadio()
        {
            var radio = _group as RadioGroup;

            if (radio == null)
                return;

            //var dialog = new AlertDialog.Builder(Context);
            //dialog.SetSingleChoiceItems(Sections.SelectMany(s => s).OfType<RadioElement>().Select(e => e.Summary()).ToArray(), RadioSelected, this);
            //dialog.SetTitle(Caption);
            //dialog.SetNegativeButton("Cancel", this);
            //dialog.Create().Show();
        }

        //void IDialogInterfaceOnClickListener.OnClick(IDialogInterface dialog, int which)
        //{
        //    if (which >= 0 && RadioSelected != which)
        //    {
        //        RadioSelected = which;
        //        var radioValue = GetSelectedValue();
        //        OnUserValueChanged(radioValue);
        //    }

        //    dialog.Dismiss();
        //}

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator"/>
        /// </returns>
        public IEnumerator<Section> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <summary>
        /// Enumerator that returns all the sections in the RootElement.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

    }
}
