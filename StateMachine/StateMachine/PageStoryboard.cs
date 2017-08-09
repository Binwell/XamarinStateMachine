using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StateMachine {
	public class PageStoryboard {
		readonly List<StateTransition> _stateTransitions = new List<StateTransition>();

		public void Add(object state, ViewTransition[] viewTransitions) {
			var stateStr = state?.ToString().ToUpperInvariant();

			if( string.IsNullOrEmpty(stateStr) || viewTransitions == null )
				throw new NullReferenceException("Value of 'state', 'viewTransitions' cannot be null");

			if( _stateTransitions.Any(t => t.State == stateStr) )
				throw new ArgumentException($"State {state} already added");

			_stateTransitions.Add(new StateTransition(stateStr, viewTransitions));
		}

		public void Go(object newState, bool withAnimation = true) {
			var newStateStr = newState?.ToString().ToUpperInvariant();

			if( string.IsNullOrEmpty(newStateStr) )
				throw new NullReferenceException("Value of newState cannot be null");

			var state = _stateTransitions.FirstOrDefault(t => t.State == newStateStr);
			if( state == null )
				throw new KeyNotFoundException($"There is no state {newState}");

			state.Start(withAnimation);
		}

		class StateTransition {
			readonly ViewTransition[] _viewTransitions;

			public string State { get; }

			public StateTransition(string state, ViewTransition[] viewTransitions) {
				State = state;
				_viewTransitions = viewTransitions;
			}

			public void Start(bool withAnimation) {
				Task.WhenAll(_viewTransitions.Select(animationInfo => animationInfo.GetAnimationTask(withAnimation)));
			}
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

		public async Task GetAnimationTask(bool withAnimation) {
			VisualElement targetElement;
			if( !_targetElementReference.TryGetTarget(out targetElement) )
				throw new ObjectDisposedException("ViewTransition target view was disposed");

			if( withAnimation && _delay > 0 )
				await Task.Delay(_delay);

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