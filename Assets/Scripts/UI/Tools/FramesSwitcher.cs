using ConnectIt.Utilities;
using ConnectIt.Utilities.Collections;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace ConnectIt.UI.Tools
{
    public class FramesSwitcher<T>
    {
        public event Action<T> FrameSwitched;

        private readonly Frame<T>[] _frames;
        private readonly History<int> _switchedFramesHistory;
        private readonly Action<T> _enableAction;
        private readonly Action<T> _disableAction;

        private int _currentFrameIndex = -1;

        public FramesSwitcher(Frame<T>[] frames,
            Action<T> enableAction,
            Action<T> disableAction)
        {
            Assert.ArgsIsNotNull(frames, enableAction, disableAction);

            _frames = frames;
            _enableAction = enableAction;
            _disableAction = disableAction;

            _switchedFramesHistory = new(_frames.Length);
        }

        public void SwitchTo(T element, bool saveToHistory = true)
        {
            Assert.IsNotNull(element);

            int foundIndex = FindFrameIndexByElement(element);
            Assert.That(foundIndex != -1);

            SwitchTo(foundIndex, saveToHistory);
        }

        public void SwitchBackOrToDefault(T defaultElement)
        {
            int finalElementIndex = _switchedFramesHistory.TryGetRecentAndForget(out int resultIndex) ?
                resultIndex :
                FindFrameIndexByElement(defaultElement);

            SwitchTo(finalElementIndex, false);
        }

        public bool TrySwitchBack()
        {
            if (_switchedFramesHistory.TryGetRecentAndForget(out int elementIndex))
                return false;

            SwitchTo(elementIndex, false);
            return true;
        }

        public void ClearSwitchedFramesHistory()
        {
            _switchedFramesHistory.ForgetAll();
        }

        private int FindFrameIndexByElement(T element)
            => _frames.FindIndex(frame => EqualityComparer<T>.Default.Equals(element, frame.Element));

        private void SwitchTo(int elementIndex, bool saveToHistory = true)
        {
            if (_currentFrameIndex != -1)
            {
                if (saveToHistory)
                    _switchedFramesHistory.Save(_currentFrameIndex);

                DisableTreeAndResetCurrent();
            }

            EnableTreeAndSetCurrent(elementIndex);

            FrameSwitched?.Invoke(_frames[elementIndex].Element);
        }

        private void EnableTreeAndSetCurrent(int topElementIndex)
        {
            SetEnableForTree(true, topElementIndex);
            _currentFrameIndex = topElementIndex;
        }

        private void DisableTreeAndResetCurrent()
        {
            SetEnableForTree(false, _currentFrameIndex);
            _currentFrameIndex = -1;
        }

        private void SetEnableForTree(bool isEnable, int topElementIndex)
        {
            int currentElementIndex = topElementIndex;
            SingleSetEnable(isEnable, currentElementIndex);

            while (_frames[currentElementIndex].HasParent)
            {
                currentElementIndex = FindFrameIndexByElement(_frames[currentElementIndex].Parent);

                SingleSetEnable(isEnable, currentElementIndex);
            }
        }

        private void SingleSetEnable(bool isEnable, int elementIndex)
        {
            if (isEnable)
                SingleEnable(elementIndex);
            else
                SingleDisable(elementIndex);
        }

        private void SingleEnable(int elementIndex)
        {
            _enableAction(_frames[elementIndex].Element);
        }

        private void SingleDisable(int elementIndex)
        {
            _disableAction(_frames[elementIndex].Element);
        }
    }
}
