using System;
using System.Drawing;
using System.Linq;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using System.Collections.Generic;

namespace Sample
{
	public partial class AppDelegate
	{
		public void DemoContainerStyle () 
		{
			var root = new RootElement ("Container Style") {
				new Section ("A") {
					new StringElement ("Another kind of"),
					new StringElement ("TableView, just by setting the"),
					new StringElement ("Style property"),
				},
				new Section ("C"){
					new StringElement ("Chaos"),
					new StringElement ("Corner"),
				},
#warning inline LINQ source removed
                /*
                 * 
				new Section ("Style"){
					from a in "Hello there, this is a long text that I would like to split in many different nodes for the sake of all of us".Split (' ')
						select (Element) new StringElement (a)
				}
                */
			};
			var dvc = new DialogViewController (root, true) {
				Style = UITableViewStyle.Plain
			};
			navigation.PushViewController (dvc, true);
		}

	}
}
