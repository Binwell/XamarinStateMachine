using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StateMachine {
	public class Storyboard {
		readonly Dictionary<string,ViewTransition[]> _stateTransitions = new Dictionary<string, ViewTransition[]>();

		public void Add(object state, ViewTransition[] viewTransitions) {
			var stateStr = state?.ToString().ToUpperInvariant();

			if( string.IsNullOrEmpty(stateStr) || viewTransitions == null )
				throw new NullReferenceException("Value of 'state', 'viewTransitions' cannot be null");

			if( _stateTransitions.ContainsKey(stateStr) )
				throw new ArgumentException($"State {state} already added");

			_stateTransitions.Add(stateStr, viewTransitions);
		}

		public void Go(object newState, bool withAnimation = true) {
			var newStateStr = newState?.ToString().ToUpperInvariant();

			if( string.IsNullOrEmpty(newStateStr) )
				throw new NullReferenceException("Value of newState cannot be null");
			
			if( !_stateTransitions.ContainsKey(newStateStr))
				throw new KeyNotFoundException($"There is no state {newState}");

			Task.WhenAll(_stateTransitions[newStateStr].Select(viewTransition => viewTransition.GetTransition(withAnimation)));
		}
	}

	public enum AnimationType {
		Scale,
		Opacity,
		TranslationX,
		TranslationY,
		Rotation
	}

	public class ViewTransition {
		readonly AnimationType _animationType;
		readonly int _delay;
		readonly uint _duration;
		readonly Easing _easing;
		readonly double _endValue;
		readonly WeakReference<VisualElement> _targetElementReference;

		public ViewTransition(VisualElement targetElement, AnimationType animationType, double endValue, uint duration = 250, Easing easing = null, int delay = 0) {
			_targetElementReference = new WeakReference<VisualElement>(targetElement);
			_animationType = animationType;
			_duration = duration;
			_endValue = endValue;
			_delay = delay;
			_easing = easing;
		}

		public async Task GetTransition(bool withAnimation) {
			VisualElement targetElement;
			if( !_targetElementReference.TryGetTarget(out targetElement) )
				throw new ObjectDisposedException("ViewTransition target view was disposed");
			
			if ( _delay > 0 )
				await Task.Delay(_delay);

			withAnimation &= _duration > 0;

			switch ( _animationType ) {
				case AnimationType.Scale:
					if( withAnimation )
						await targetElement.ScaleTo(_endValue, _duration, _easing);
					else
						targetElement.Scale = _endValue;
					break;

				case AnimationType.Opacity:
					if( withAnimation ) {
						if( !targetElement.IsVisible && _endValue <= 0 )
							break;

						if( targetElement.IsVisible && _endValue < targetElement.Opacity ) {
							await targetElement.FadeTo(_endValue, _duration, _easing);
							targetElement.IsVisible = _endValue > 0;
						}
						else if( targetElement.IsVisible && _endValue > targetElement.Opacity ) {
							await targetElement.FadeTo(_endValue, _duration, _easing);
						}
						else if( !targetElement.IsVisible && _endValue > targetElement.Opacity ) {
							targetElement.Opacity = 0;
							targetElement.IsVisible = true;
							await targetElement.FadeTo(_endValue, _duration, _easing);
						}
					}
					else {
						targetElement.Opacity = _endValue;
						targetElement.IsVisible = _endValue > 0;
					}
					break;

				case AnimationType.TranslationX:
					if( withAnimation )
						await targetElement.TranslateTo(_endValue, targetElement.TranslationY, _duration, _easing);
					else
						targetElement.TranslationX = _endValue;
					break;

				case AnimationType.TranslationY:
					if( withAnimation )
						await targetElement.TranslateTo(targetElement.TranslationX, _endValue, _duration, _easing);
					else
						targetElement.TranslationY = _endValue;
					break;

				case AnimationType.Rotation:
					if( withAnimation )
						await targetElement.RotateTo(_endValue, _duration, _easing);
					else
						targetElement.Rotation = _endValue;
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}