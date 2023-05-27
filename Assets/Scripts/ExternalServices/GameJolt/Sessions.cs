using ConnectIt.Coroutines;
using GameJolt.API;
using System.Collections;
using UnityEngine;
using GJAPI = GameJolt.API;

namespace ConnectIt.ExternalServices.GameJolt
{
    public class Sessions
    {
        private ICoroutinesGlobalContainer _coroutinesContainer;

        private Coroutine _pingCoroutine;
        private short _tryOpenCount = -1;

        private const short MaxTryOpenCount = 5;

        public Sessions(
            ICoroutinesGlobalContainer coroutinesContainer)
        {
            _coroutinesContainer = coroutinesContainer;
        }

        public void Open()
        {
            if (_tryOpenCount > -1)
                return;

            OpenInternal();
        }

        public void Close()
        {
            GJAPI.Sessions.Close();

            StopPing();
        }

        public void StartPing()
        {
            if (_pingCoroutine != null)
                StopPing();

            _pingCoroutine = _coroutinesContainer.StartCoroutine(Ping());
        }

        public void StopPing()
        {
            if (_pingCoroutine == null)
                return;

            if (_coroutinesContainer.IsAlive())
                _coroutinesContainer.StopCoroutine(_pingCoroutine);
        }

        private void OpenInternal()
        {
            _tryOpenCount++;
            if (_tryOpenCount >= MaxTryOpenCount)
            {
                _tryOpenCount = -1;
                return;
            }

            GJAPI.Sessions.Open(success =>
            {
                if (success)
                {
                    StartPing();
                    _tryOpenCount = -1;
                }
                else
                {
                    OpenInternal();
                }
            });
        }

        private IEnumerator Ping()
        {
            bool abort = false;

            while (!abort)
            {
                GJAPI.Sessions.Ping(SessionStatus.Active, success =>
                {
                    if (!success)
                    {
                        Open();

                        abort = true;
                    }
                });

                yield return new WaitForSecondsRealtime(15);
            }

            _pingCoroutine = null;
        }
    }
}
