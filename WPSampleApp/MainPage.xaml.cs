using System.Windows;
using Microsoft.Phone.Controls;
using Dialog.Core.Descriptions;
using Newtonsoft.Json;
using WP.Dialog;

namespace WPSampleApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
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
                        'Caption':'Caption field here',
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
        }
    ]
}";

        private void PhoneApplicationPage_Loaded(object s, RoutedEventArgs e)
        {
            var parser = new WindowsPhoneElementBuilder();

            var setter = new ExampleActionPropertySetter();
            setter.Actions["ShowDialogActivity"] = (sender, args) => MessageBox.Show("Button click: ShowDialogActivity");
            setter.Actions["ShowDialogListViewActivity"] = (sender, args) => MessageBox.Show("Button click: ShowDialogListViewActivity");
            setter.Actions["ElementTest"] = (sender, args) => MessageBox.Show("Button click: ElementTest");
            parser.CustomPropertySetters["Action"] = setter;
            var description = JsonConvert.DeserializeObject<ElementDescription>(JsonText);
            var root = (RootElement)parser.Build(description);

            LayoutRoot.Children.Add(root.GetView());


        }
    }
}