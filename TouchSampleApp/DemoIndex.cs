//
// This sample shows how to present an index.
//
// This requires the user to create two subclasses for the
// internal model used in DialogViewController and a new
// subclass of DialogViewController that activates it.
//
// See the source in IndexedViewController
//
// The reason for Source and SourceSizing derived classes is
// that MonoTouch.Dialog will create one or the other based on
// whether there are elements with uniform sizes or not.  This
// imrpoves performance by avoiding expensive computations.
//
using System;
using System.Drawing;
using System.Linq;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using System.Collections.Generic;

namespace Sample
{
	class IndexedViewController : DialogViewController {
		public IndexedViewController (RootElement root, bool pushing) : base (root, pushing)
		{
			// Indexed tables require this style.
			Style = UITableViewStyle.Plain;
			EnableSearch = true;
			SearchPlaceholder = "Find item";
			AutoHideSearch = true;
		}
		
		string [] GetSectionTitles ()
		{
#warning inline LINQ source removed
            //var array = from section in Root select section.Caption.Substring(0, 1);
            //return array.ToArray ();
            return new string[0];
		}
		
		class IndexedSource : Source {
        	IndexedViewController parent;

	        public IndexedSource (IndexedViewController parent) : base (parent)
	        {
	            this.parent = parent;
	        }
	
	        public override string[] SectionIndexTitles (UITableView tableView)
	        {
				var j = parent.GetSectionTitles ();
				return j;
	        }
	    }

		class SizingIndexedSource : Source {
        	IndexedViewController parent;

	        public SizingIndexedSource (IndexedViewController parent) : base (parent)
	        {
	            this.parent = parent;
	        }
	
	        public override string[] SectionIndexTitles (UITableView tableView)
	        {
				var j = parent.GetSectionTitles ();
				return j;
	        }
	    }

		public override Source CreateSizingSource (bool unevenRows)
	    {
			if (unevenRows)
				return new SizingIndexedSource (this);
			else
	        	return new IndexedSource (this);
	    }
	}
	
	public partial class AppDelegate
	{
		public void DemoIndex () 
		{
#warning DemoIndex removed
            /*
			var root = new RootElement ("Container Style") {
				from sh in "ABCDEFGHIJKLMNOPQRSTUVWXYZ" 
				    select new Section (sh + " - Section") {
					   from filler in "12345" 
						select (Element) new StringElement (sh + " - " + filler)
				}
			};
			var dvc = new IndexedViewController (root, true);
			navigation.PushViewController (dvc, true);
                */
        }

	}
}

