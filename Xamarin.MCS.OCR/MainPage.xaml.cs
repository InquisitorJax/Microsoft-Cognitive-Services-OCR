using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.MCS.OCR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}