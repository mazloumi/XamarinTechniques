using Xamarin.Forms;

namespace XamarinTechniques
{
    public partial class XamarinTechniquesPage : MasterDetailPage
    {
        public XamarinTechniquesPage()
        {
            InitializeComponent();

            string[] myPageNames = { "Event", "Event Trigger", "Property Trigger", "Data Trigger", "Implicit Style", "Behavior", "Attached Property", "Tap Gesture", "Renderer", "Effect" };
            examples.ItemsSource = myPageNames;

            examples.ItemTapped += (sender, e) =>
            {
                ContentPage gotoPage;
                switch (e.Item.ToString())
                {
                    case "Event":
                        gotoPage = new EventPage();
                        break;
                    case "Event Trigger":
                        gotoPage = new EventTriggerPage();
                        break;
                    case "Property Trigger":
                        gotoPage = new PropertyTriggerPage();
                        break;
                    case "Data Trigger":
                        gotoPage = new DataTriggerPage();
                        break;
                    case "Implicit Style":
                        gotoPage = new ImplicitStylePage();
                        break;
                    case "Behavior":
                        gotoPage = new BehaviorPage();
                        break;
                    case "Attached Property":
                        gotoPage = new AttachedPropertyPage();
                        break;
                    case "Tap Gesture":
                        gotoPage = new TapGesturePage();
                        break;
                    case "Renderer":
                        gotoPage = new RendererPage();
                        break;
                    case "Effect":
                        gotoPage = new EffectPage();
                        break;
                    case "Value Converter":
                        gotoPage = new ValueConverterPage();
                        break;
                    default:
                        gotoPage = new EventPage();
                        break;
                }
                Detail = new NavigationPage(gotoPage);
                ((ListView)sender).SelectedItem = null;
                this.IsPresented = false;
            };

            Detail = new NavigationPage(new EventPage());
        }
    }
}