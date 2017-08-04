using Android.App;
using Android.Content.PM;
using Android.OS;
using TK.CustomMap.Api.Google;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace StateMachine.Droid {
	[Activity(Label = "StateMachine", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity {
		protected override void OnCreate(Bundle bundle) {
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);
			GmsDirection.Init("AIzaSyCmyRs7zWkzCBxVmtq3PjVj6H25fz4xarg");
			LoadApplication(new App());
		}
	}
}