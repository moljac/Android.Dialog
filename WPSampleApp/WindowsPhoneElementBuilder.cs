using System;
using Dialog.Core.Builder;
using Dialog.Core.Descriptions;
using Dialog.Core.Elements;
using WP.Dialog;

namespace WPSampleApp
{
    public class WindowsPhoneElementBuilder : ElementBuilder
    {
        public WindowsPhoneElementBuilder(bool registerDefaultElements = true)
        {
            if (registerDefaultElements)
            {
                RegisterDefaultElements();
            }
        }

        public void RegisterDefaultElements()
        {
            RegisterConventionalElements(typeof(RootElement).Assembly);
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