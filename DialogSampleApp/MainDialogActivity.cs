using System;
using System.Windows.Input;
using Android.App;
using Android.Dialog.Builder;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Dialog;
using System.Linq;
using Dialog.Core.Descriptions;

namespace DialogSampleApp
{
    //
    // NOTE: with the new update you will have to add all the dialog_* prefixes to your main application.
    //       This is because the current version of Mono for Android will not add resources from assemblies
    //       to the main application like it does for libraries in Android/Java/Eclipse...  This could
    //       change in a future version (it's slated for 1.0 post release) but for now, just add them 
    //       as in this sample...
    //
    [Activity(Label = "Android.Dialog Sample", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainDialogActivity : DialogActivity
    {
        protected void StartNew()
        {
            StartActivity(typeof(DialogListViewActivity));
        }

        protected void ClickList()
        {
            StartActivity(typeof(NameActivity));
        }

        protected void ClickElementTest()
        {
            StartActivity(typeof(EntryActivity));
        }

        public static readonly string JsonText = @"
{
    'Key':'Root',
    'Properties':{
        'Caption':'TestRootElement'
    },
    'Sections':[
        {
            'Elements':[
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'Label',
                        'Value':'Only Element in a Blank Section'
                    }
                }
            ]
        },
        {
            'Properties':{
                'Header':'Test Header',
                'Footer':'Test Footer'
            },
            'Elements':[
                {
                    'Key':'Button',
                    'Properties':{
                        'Caption':'DialogActivity',
                        'Click':'@Action:ShowDialogActivity'
                    }
                },
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'DialogListViewActivity',
                        'Click':'@Action:ShowDialogListViewActivity',
                        'LayoutName':'dialog_labelfieldright'
                    }
                },
                {
                    'Key':'Boolean',
                    'Properties':{
                        'Caption':'Push my button',
                        'Value':true
                    }
                },
                {
                    'Key':'Boolean',
                    'Properties':{
                        'Caption':'Push this too',
                        'Value':false
                    }
                },
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'Click for EntryElement Test',
                        'Click':'@Action:ElementTest'
                    }
                }
            ]
        },
        {
            'Properties':{
                'Header':'Part II'
            },
            'Elements':[
                {
                    'Key':'String',
                    'Properties':{
                        'Caption':'This is the String Element',
                        'Value':'This is it\'s value'
                    }
                },
                {
                    'Key':'Checkbox',
                    'Properties':{
                        'Caption':'Check this out',
                        'Value':true
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Username',
                        'Value':'',
                        'Hint':'Enter Login'
                    }
                },
                {
                    'Key':'Entry',
                    'Properties':{
                        'Caption':'Password',
                        'Value':'',
                        'Hint':'Enter Password',
                        'Password':true
                    }
                }
            ]
        },
        {
            'HeaderElement': {
                'Key':'String',
                'Properties':{
                    'Caption':'Can Populate be done?',
                    'Value':'Need to look at how ViewElement works...'
                }
            },
            'Properties':{
                'Header':'Group'
            },
            'Elements':[
                {
                    'Key':'Root',
                    'Properties':{
                        'Caption':'Radio Group'
                    },
                    'Group':{
                        'Key':'Radio',
                        'Properties':{
                            'Key':'dessert',
                            'Selected':1
                        }
                    },
                    'Sections':[
                           {
                              'Elements':[
                                   {
                                        'Key':'Radio',
                                        'Properties':{
                                            'Caption':'Ice Cream Sandwich',
                                            'Group':'dessert'
                                        }
                                    },
                                    {
                                        'Key':'Radio',
                                        'Properties':{
                                            'Caption':'Honeycomb',
                                            'Group':'dessert'
                                        }
                                    },
                                    {
                                        'Key':'Radio',
                                        'Properties':{
                                            'Caption':'Gingerbread',
                                            'Group':'dessert'
                                        }
                                    }
                               ]
                           }
                       ]
                 }
            ]
        }
    ]
}";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DroidResources.Initialise(typeof(Resource.Layout));
            var parser = new DroidElementBuilder();

            var setter = new ExampleActionPropertySetter();
            setter.Actions["ShowDialogActivity"] = (sender, args) => StartNew();
            setter.Actions["ShowDialogListViewActivity"] = (sender, args) => ClickList();
            setter.Actions["ElementTest"] = (sender, args) => ClickElementTest();
            parser.CustomPropertySetters["Action"] = setter;
            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<ElementDescription>(JsonText);
            Root = parser.Build(description) as RootElement;

            /*
            Root = new RootElement("Test Root Elem")
            {
                new Section
                {
                    new StringElement("Label", "Only Element in a Blank Section"),
                },
                new Section("Test Header", "Test Footer")
                {
                    new ButtonElement("DialogActivity", (o, e) => StartNew()),
                    new StringElement("DialogListView Activity") 
                    {
                        Click = (o, e) => ClickList(),
                        LayoutName = "dialog_labelfieldright"
                    },
                    new BooleanElement("Push my button", true),
                    new BooleanElement("Push this too", false),
                    new StringElement("Text label", "Click for EntryElement Test")
                    {
                        Click = (o, e) => ClickElementTest(),
                    },
                },
                new Section("Part II")
                {
                    new StringElement("This is the String Element", "The Value"),
                    new CheckboxElement("Check this out", true),
                    new EntryElement("Username", string.Empty){ Hint = "Enter Login", },
                    new EntryElement("Password", string.Empty) {
                        Hint = "Enter Password",
                        Password = true,
                    },
                },
                new Section("Group", new ViewElement(Android.Resource.Layout.SimpleListItem1)
                    { Populate = view => { view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = "Custom footer view"; }, })
                {
                    new RootElement("Radio Group", new Android.Dialog.RadioGroup("dessert", 1))
                    {
                        new Section
                        {
                            new RadioElement("Ice Cream Sandwich", "dessert"),
                            new RadioElement("Honeycomb", "dessert"),
                            new RadioElement("Gingerbread", "dessert"),
                        },
                    },
                }
            };
            */

            ValueChanged += root_ValueChanged;
        }

        void root_ValueChanged(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Changed", ToastLength.Short).Show();
        }
    }
}