using Xamarin.Forms;
using Xamarin.MCS.OCR;
using Xamarin.MCS.OCR.MCS;
using Xamarin.MCS.OCR.MCS.OCR;
using Xamarin.MCS.OCR.Media;

namespace XamarinForms
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new MainPage();

            //MEDIA
            DependencyService.Register<ITakePictureCommand, TakePictureCommand>();
            DependencyService.Register<IChoosePictureCommand, ChoosePictureCommand>();

            //VISION API
            DependencyService.Register<IApiKeyProvider, ApiKeyProvider>();
            //DependencyService.Register<IApiKeyProvider, YourApiKeyProvider>();
            DependencyService.Register<IVisionClientFactory, VisionClientFactory>();
            DependencyService.Register<IRecognizeTextFromImageCommand, RecognizeTextFromImageCommand>();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}