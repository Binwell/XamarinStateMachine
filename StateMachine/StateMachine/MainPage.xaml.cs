using System;
using Xamarin.Forms;

namespace StateMachine {
	public partial class MainPage : ContentPage {
		readonly PageStoryboard _pageStoryboard = new PageStoryboard();

		public MainPage() {
			InitializeComponent();

			_pageStoryboard.Add(States.Main, new[] {
				                                       new ViewTransition(MainView, AnimationType.Opacity, 1, 300), // Active and visible
													   new ViewTransition(EnterAddressButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 200),
				                                       new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
				                                       new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                       new ViewTransition(ShowRouteView, AnimationType.Opacity, 0),
				                                       new ViewTransition(DriveView, AnimationType.TranslationY, -80),
				                                       new ViewTransition(DriveView, AnimationType.Opacity, 0),
				                                       new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 0.8),
				                                       new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 0.8)
			                                       });
			_pageStoryboard.Add(States.FindAddress, new[] {
				                                              new ViewTransition(MainView, AnimationType.Opacity, 0, 100),
															  new ViewTransition(FindAddressView, AnimationType.Opacity, 1), // Active and visible
				                                              new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                              new ViewTransition(ShowRouteView, AnimationType.Opacity, 0),
				                                              new ViewTransition(DriveView, AnimationType.TranslationY, -80),
				                                              new ViewTransition(DriveView, AnimationType.Opacity, 0),
				                                              new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9),
				                                              new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 300),
				                                              new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 1, 300, Easing.BounceOut, 500)
			                                              });
			_pageStoryboard.Add(States.ShowRoute, new[] {
				                                            new ViewTransition(MainView, AnimationType.Opacity, 0),
															new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
				                                            new ViewTransition(ShowRouteView, AnimationType.TranslationY, 0, 300), // Active and visible
				                                            new ViewTransition(ShowRouteView, AnimationType.Opacity, 1, 300), // Active and visible
				                                            new ViewTransition(DriveView, AnimationType.TranslationY, -80),
				                                            new ViewTransition(DriveView, AnimationType.Opacity, 0),
				                                            new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9),
				                                            new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 0.8),
				                                            new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 0.8)
			                                            });
			_pageStoryboard.Add(States.Drive, new[] {
				                                        new ViewTransition(MainView, AnimationType.Opacity, 0),
														new ViewTransition(FindAddressView, AnimationType.Opacity, 0),
				                                        new ViewTransition(ShowRouteView, AnimationType.TranslationY, 200),
				                                        new ViewTransition(ShowRouteView, AnimationType.Opacity, 0),
				                                        new ViewTransition(DriveView, AnimationType.TranslationY, 0, 300), // Active and visible
				                                        new ViewTransition(DriveView, AnimationType.Opacity, 1, 300), // Active and visible
				                                        new ViewTransition(EnterAddressButton, AnimationType.Scale, 0.9),
				                                        new ViewTransition(ShowRouteGoButton, AnimationType.Scale, 0.8),
				                                        new ViewTransition(ShowRouteCancelButton, AnimationType.Scale, 0.8)
			                                        });

			_pageStoryboard.Go(States.Main, false);
		}

		void GotoFindAddressClicked(object sender, EventArgs e) {
			_pageStoryboard.Go(States.FindAddress);
			FinalAddressEntry.Focus();
		}

		void GotoShowRouteClicked(object sender, EventArgs e) {
			_pageStoryboard.Go(States.ShowRoute);
		}

		void GotoDriveClicked(object sender, EventArgs e) {
			_pageStoryboard.Go(States.Drive);
		}

		void GotoMainClicked(object sender, EventArgs e) {
			_pageStoryboard.Go(States.Main);
		}

		enum States {
			Main,
			FindAddress,
			ShowRoute,
			Drive
		}
	}
}