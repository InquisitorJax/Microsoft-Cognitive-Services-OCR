using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Wibci.Xamarin.Images;
using Wibci.Xamarin.Images.Droid;
using Xamarin.Forms;

namespace XamarinForms.Droid
{
    [Activity(Label = "Xamarin OCR", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();
        }
    }
}