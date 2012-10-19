using System;
using Dialog.Core.Builder;
using Dialog.Core.Descriptions;
using Dialog.Core.Elements;

namespace Android.Dialog.Builder
{
    public class DroidElementBuilder : ElementBuilder
    {
        public DroidElementBuilder(bool registerDefaultElements = true)
            : base()
        {
            if (registerDefaultElements)
            {
                RegisterDefaultElements();
            }
        }

        public void RegisterDefaultElements()
        {
            RegisterConventionalElements(typeof(DroidResources).Assembly);
        }

        protected override ISection CreateNewSection(SectionDescription sectionDescription)
        {
            return new Section();
        }

        protected override IGroup CreateNewGroup(GroupDescription groupDescription)
        {
            if (groupDescription.Key != null && groupDescription.Key != "Radio")
            {
                throw new ArgumentException("We only know about RadioGroups at present, not: " + groupDescription.Key);
            }

            return new RadioGroup();
        }
    }
}