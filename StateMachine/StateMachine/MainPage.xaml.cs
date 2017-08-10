using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StateMachine {
	public partial class MainPage : ContentPage {
		enum States {
			Main,
			FindAddress,
			ShowRoute,
			Drive
		}

		readonly Storyboard _storyboard = new Storyboard();

		public MainPage() {
			InitializeComponent();

			_storyboard.Add(States.Main, new[] {
													   new ViewTransition(MainView, AnimationType.Opacity, 1, 300), // Active and visible
				                                       new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9, 0),
													   new ViewTransition(EnterAddressButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 200),
				                                       new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
				                                       new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                       new ViewTransition(ShowRouteView, AnimationType.Opacity, 0),
				                                       new ViewTransition(DriveView, AnimationType.TranslationY, -80),
				                                       new ViewTransition(DriveView, AnimationType.Opacity, 0),
			                                       });
			_storyboard.Add(States.FindAddress, new[] {
															  new ViewTransition(MainView, AnimationType.Opacity, 0, 100),
															  new ViewTransition(FindAddressView, AnimationType.Opacity, 1, delay: 100), // Active and visible
				                                              new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                              new ViewTransition(ShowRouteView, AnimationType.Opacity, 0, 200),
				                                              new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 0.8, 0),
															  new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 300),
				                                              new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 0.8, 0),
															  new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 500)
			                                              });
			_storyboard.Add(States.ShowRoute, new[] {
															new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
				                                            new ViewTransition(ShowRouteView, AnimationType.TranslationY, 0, 300, delay: 250), // Active and visible
				                                            new ViewTransition(ShowRouteView, AnimationType.Opacity, 1, 0), // Active and visible
				                                            new ViewTransition(DriveView, AnimationType.TranslationY, -80),
				                                            new ViewTransition(DriveView, AnimationType.Opacity, 0)
			                                            });
			_storyboard.Add(States.Drive, new[] {
				                                        new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                        new ViewTransition(ShowRouteView, AnimationType.Opacity, 0, 0, delay: 250),
				                                        new ViewTransition(DriveView, AnimationType.TranslationY, 0, 300, delay: 250), // Active and visible
				                                        new ViewTransition(DriveView, AnimationType.Opacity, 1, 0) // Active and visible
													});

			_storyboard.Go(States.Main, false);
		}

		async void GotoFindAddressClicked(object sender, EventArgs e) {
			_storyboard.Go(States.FindAddress);
			await Task.Delay(500);
			FinalAddressEntry.Focus();
		}

		void GotoShowRouteClicked(object sender, EventArgs e) {
			FinalAddressEntry.Unfocus();
			_storyboard.Go(States.ShowRoute);
		}

		void GotoDriveClicked(object sender, EventArgs e) {
			_storyboard.Go(States.Drive);
		}

		void GotoMainClicked(object sender, EventArgs e) {
			FinalAddressEntry.Unfocus();
			_storyboard.Go(States.Main);
		}
	}
}