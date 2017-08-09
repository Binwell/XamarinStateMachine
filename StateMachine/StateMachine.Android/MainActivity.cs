using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace StateMachine.Droid {
	[Activity(Label = "StateMachine", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity {
		protected override void OnCreate(Bundle bundle) {
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);
			
			Forms.Init(this, bundle);
			FormsMaps.Init(this, bundle);
			LoadApplication(new App());

			Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
				Window.DecorView.SystemUiVisibility = 0;
				var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				statusBarHeightInfo?.SetValue(this, 0);
				Window.SetStatusBarColor(new Android.Graphics.Color(0, 0, 0, 255)); // Change color as required.
			}
		}
	}
}