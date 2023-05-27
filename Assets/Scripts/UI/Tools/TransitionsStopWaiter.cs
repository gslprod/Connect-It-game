using ConnectIt.Utilities;
using System;
using System.Linq;
using UnityEngine.UIElements;

namespace ConnectIt.UI.Tools
{
    public class TransitionsStopWaiter
    {
        public bool Waiting => _elements != null;

        private VisualElement[] _elements;
        private int _totalTransitionsCount;
        private int _stoppedTransitionsCount;
        private Action _onTransitionStop;

        public void Wait(Action onStop, params VisualElement[] elementsWithTransitions)
        {
            Assert.ArgsIsNotNull(elementsWithTransitions, onStop);
            Assert.That(!Waiting);

            _stoppedTransitionsCount = 0;
            _onTransitionStop = onStop;
            _elements = elementsWithTransitions;

            foreach (VisualElement element in elementsWithTransitions)
            {
                element.RegisterCallback<TransitionRunEvent>(OnTransitionRun);

                element.RegisterCallback<TransitionEndEvent>(OnTransitionStopped);
                element.RegisterCallback<TransitionCancelEvent>(OnTransitionStopped);
            }
        }

        public void AbortCurrentAndWait(Action onStop, params VisualElement[] elementsWithTransitions)
        {
            if (Waiting)
                AbortWaiting();

            Wait(onStop, elementsWithTransitions);
        }

        public void AbortWaiting()
        {
            Assert.That(Waiting);

            UnregisterCallbacks();
        }

        public void AbortIfWaiting()
        {
            if (!Waiting)
                return;

            AbortWaiting();
        }

        private void OnTransitionStopped<T>(TransitionEventBase<T> stopEvent) where T : TransitionEventBase<T>, new()
        {
            VisualElement target = stopEvent.target as VisualElement;
            if (!_elements.Contains(target))
                return;

            _stoppedTransitionsCount++;
            if (_stoppedTransitionsCount < _totalTransitionsCount)
                return;

            UnregisterCallbacks();

            _onTransitionStop();
        }

        private void OnTransitionRun(TransitionRunEvent runEvent)
        {
            VisualElement target = runEvent.target as VisualElement;
            if (!_elements.Contains(target))
                return;

            foreach (VisualElement element in _elements)
            {
                _totalTransitionsCount += element.resolvedStyle.transitionProperty.Count();

                element.UnregisterCallback<TransitionRunEvent>(OnTransitionRun);
            }
        }

        private void UnregisterCallbacks()
        {
            foreach (VisualElement element in _elements)
            {
                element.UnregisterCallback<TransitionEndEvent>(OnTransitionStopped);
                element.UnregisterCallback<TransitionCancelEvent>(OnTransitionStopped);

                element.UnregisterCallback<TransitionRunEvent>(OnTransitionRun);
            }

            _elements = null;
        }
    }
}
