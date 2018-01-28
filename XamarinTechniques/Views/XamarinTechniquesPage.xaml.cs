using Xamarin.Forms;

namespace XamarinTechniques
{
    public partial class XamarinTechniquesPage : MasterDetailPage
    {
        public XamarinTechniquesPage()
        {
            InitializeComponent();

            string[] myPageNames = { "Event", "Property Trigger", "Behavior", "Attached Property","Tap Gesture", "Renderer", "Effect" };
            examples.ItemsSource = myPageNames;

            examples.ItemTapped += (sender, e) =>
            {
                ContentPage gotoPage;
                switch (e.Item.ToString())
                {
                    case "Event":
                        gotoPage = new EventPage();
                        break;
                    case "Property Trigger":
                        gotoPage = new PropertyTriggerPage();
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