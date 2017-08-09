using Foundation;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace StateMachine.iOS {
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate {
		public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
			Forms.Init();
			FormsMaps.Init();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}