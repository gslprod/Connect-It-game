using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Security.ConfidentialData;
using ConnectIt.Utilities;
using GameJolt.API;
using GameJolt.API.Objects;
using System;
using UnityEngine;
using Zenject;

namespace ConnectIt.ExternalServices.GameJolt
{
    public class GameJoltAPIProvider : IInitializable
    {
        public event Action<bool> LogInAttempt;
        public event Action<bool> UserInfoUpdateAttempt;

        public User User => _gjAPI.CurrentUser;
        public bool UserExistsAndLoggedIn => _gjAPI.HasSignedInUser;

        private readonly GameJoltAPI _gjAPI;
        private readonly Sessions _sessions;
        private readonly Scores _scores;

        private IExternalServerSaveProvider _externalServerSaveProvider;
        private readonly ConfidentialValues _confidentialValues;

        public GameJoltAPIProvider(GameJoltAPI gjAPI,
            Sessions sessions,
            IExternalServerSaveProvider externalServerSaveProvider,
            ConfidentialValues confidentialValues,
            Scores scores)
        {
            _gjAPI = gjAPI;
            _sessions = sessions;
            _externalServerSaveProvider = externalServerSaveProvider;
            _confidentialValues = confidentialValues;
            _scores = scores;
        }

        public void Initialize()
        {
            Application.quitting += () =>
            {
                if (UserExistsAndLoggedIn)
                    LogOut();
            };

            _gjAPI.Initialize(_confidentialValues.GameJoltAPIGameKey);

            AutoLogIn();
        }

        public void LogIn(string name, string token, Action<bool> signedInCallback = null, Action<bool> userFetchedCallback = null)
        {
            ThrowIfUserExistenceIs(true);

            var user = new User(name, token);

            Action<bool> finalSignedInCallback = SignInCallbackHandler;
            if (signedInCallback != null)
                finalSignedInCallback += signedInCallback;

            Action<bool> finalUserFetchedCallback = UserInfoUpdateCallbackHandler;
            if (userFetchedCallback != null)
                finalUserFetchedCallback += userFetchedCallback;

            user.SignIn(finalSignedInCallback, finalUserFetchedCallback);
        }

        public void LogOut()
        {
            ThrowIfUserExistenceIs(false);

            _sessions.Close();
            User.SignOut();
        }

        #region Scores

        public void AppendPlayerScore(Table table, Score toAppend, Action<bool> callback = null)
        {
            ThrowIfUserExistenceIs(false);

            _scores.AppendPlayerScore(table, toAppend, callback);
        }

        public void UpdateScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            ThrowIfUserExistenceIs(false);

            _scores.UpdateScoresForTable(table, onlyPlayerScores);
        }

        public void ClearScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            ThrowIfUserExistenceIs(false);

            _scores.ClearScoresForTable(table, onlyPlayerScores);
        }

        public void AddScoresForTable(Table table, bool onlyPlayerScores = false)
        {
            ThrowIfUserExistenceIs(false);

            _scores.AddScoresForTable(table, onlyPlayerScores);
        }

        #endregion

        private void AutoLogIn()
        {
            if (UserExistsAndLoggedIn)
                return;

            ExternalServerSaveData data = _externalServerSaveProvider.LoadExternalServerData();

            if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Token))
            {
                _gjAPI.StartAutoConnect();
                return;
            }

            IExternalServerSaveProvider gjAPISaveProvider = _externalServerSaveProvider;
            Action<bool> callback = success =>
            {
                if (success)
                    return;

                ExternalServerSaveData data = gjAPISaveProvider.LoadExternalServerData();

                data.Username = string.Empty;
                data.Token = string.Empty;

                _externalServerSaveProvider.SaveExtrenalServerData(data);
            };

            LogIn(data.Username, data.Token, callback);
        }

        private void ThrowIfUserExistenceIs(bool existenceStateToThrow)
        {
            Assert.That(UserExistsAndLoggedIn != existenceStateToThrow);
        }

        #region Callback handlers

        private void SignInCallbackHandler(bool success)
        {
            if (success)
            {
                _sessions.Open();
                _scores.LoadTables();
            }

            LogInAttempt?.Invoke(success);
        }

        private void UserInfoUpdateCallbackHandler(bool success)
        {
            UserInfoUpdateAttempt?.Invoke(success);
        }

        #endregion

    }
}