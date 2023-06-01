using ConnectIt.Save.Names;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Save.Savers;
using ConnectIt.Save.Serializers;
using ConnectIt.Security.Encryption;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConnectIt.Save.SaveProviders
{
    public class GameSaveProvider :
        IInitializable,
        IGameplaySaveProvider, IShopSaveProvider, IStatsSaveProvider,
        IExternalServerSaveProvider, ISettingsSaveProvider, ISecuritySaveProvider
    {
        public event Action<Type, Exception> LoadFail;
        public event Action<Type, Exception> SaveFail;

        public event Action GameplaySaveDataChanged;
        public event Action ShopSaveDataChanged;
        public event Action StatsSaveDataChanged;
        public event Action ExternalServerSaveDataChanged;
        public event Action SettingsSaveDataChanged;
        public event Action SecuritySaveDataChanged;

        private readonly ISaver _saver;
        private readonly ISerializer _serializer;
        private readonly ISymmetricEncryptor _encryptor;
        private readonly IKeyGenerator _keyGenerator;

        private GameplaySaveData _gameplaySaveData;
        private ShopSaveData _shopSaveData;
        private StatsSaveData _statsSaveData;
        private ExternalServerSaveData _externalServerSaveData;
        private SettingsSaveData _settingsSaveData;
        private SecuritySaveData _securitySaveData;

        private bool _loadError = false;

        public GameSaveProvider(
            ISaver saver,
            ISerializer serializer,
            ISymmetricEncryptor encryptor,
            IKeyGenerator keyGenerator)
        {
            _saver = saver;
            _serializer = serializer;
            _encryptor = encryptor;
            _keyGenerator = keyGenerator;
        }

        public void Initialize()
        {
            LoadAllData();
        }

        public void SaveGameplayData(GameplaySaveData saveData)
        {
            if (!TrySaveData(saveData, ref _gameplaySaveData, GameplaySaveData.SaveKey))
                return;

            GameplaySaveDataChanged?.Invoke();
        }

        public GameplaySaveData LoadGameplayData()
            => _gameplaySaveData.Clone();

        public void SaveShopData(ShopSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _shopSaveData, ShopSaveData.SaveKey))
                return;

            ShopSaveDataChanged?.Invoke();
        }

        public ShopSaveData LoadShopData()
            => _shopSaveData.Clone();

        public void SaveStatsData(StatsSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _statsSaveData, StatsSaveData.SaveKey))
                return;

            StatsSaveDataChanged?.Invoke();
        }

        public StatsSaveData LoadStatsData()
            => _statsSaveData.Clone();

        public void SaveExternalServerData(ExternalServerSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _externalServerSaveData, ExternalServerSaveData.SaveKey))
                return;

            ExternalServerSaveDataChanged?.Invoke();
        }

        public ExternalServerSaveData LoadExternalServerData()
            => _externalServerSaveData.Clone();

        public void SaveSettingsData(SettingsSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _settingsSaveData, SettingsSaveData.SaveKey))
                return;

            SettingsSaveDataChanged?.Invoke();
        }

        public SettingsSaveData LoadSettingsData()
            => _settingsSaveData.Clone();

        public void SaveSecurityData(SecuritySaveData saveData)
        {
            if (!TrySaveData(saveData, ref _securitySaveData, SecuritySaveData.SaveKey))
                return;

            SecuritySaveDataChanged?.Invoke();
        }

        public SecuritySaveData LoadSecurityData()
            => _securitySaveData.Clone();

        public void ResetSaveData(bool hard = false)
        {
            if (hard)
            {
                HardDelete();
                return;
            }

            SoftReset();
        }

        private void SoftReset()
        {
            SaveGameplayData(new());
            SaveShopData(new());
            SaveSettingsData(new());
            SaveExternalServerData(new());
            SaveStatsData(new());
            SaveSecurityData(new());
        }

        private void HardDelete()
        {
            _saver.Delete(GameplaySaveData.SaveKey);
            _saver.Delete(ShopSaveData.SaveKey);
            _saver.Delete(SettingsSaveData.SaveKey);
            _saver.Delete(ExternalServerSaveData.SaveKey);
            _saver.Delete(StatsSaveData.SaveKey);
            _saver.Delete(SecuritySaveData.SaveKey);
        }

        private void LoadAllData()
        {
            _securitySaveData = LoadAndDeserialize<SecuritySaveData>(SecuritySaveData.SaveKey);

            _gameplaySaveData = LoadAndDeserialize<GameplaySaveData>(GameplaySaveData.SaveKey);
            _shopSaveData = LoadAndDeserialize<ShopSaveData>(ShopSaveData.SaveKey);
            _statsSaveData = LoadAndDeserialize<StatsSaveData>(StatsSaveData.SaveKey);
            _externalServerSaveData = LoadAndDeserialize<ExternalServerSaveData>(ExternalServerSaveData.SaveKey);
            _settingsSaveData = LoadAndDeserialize<SettingsSaveData>(SettingsSaveData.SaveKey);

            if (_loadError)
            {
                _gameplaySaveData = new();
                _shopSaveData = new();
                _statsSaveData = new();
                _externalServerSaveData = new();
                _settingsSaveData = new();
            }
        }

        private T LoadAndDeserialize<T>(string loadKey) where T : class, new()
        {
            try
            {
                bool isSecurity = typeof(T) == typeof(SecuritySaveData);

                string loadedData = _saver.Load(loadKey);
                if (string.IsNullOrEmpty(loadedData))
                {
                    if (isSecurity)
                        return CreateSecuritySaveData<T>();

                    if (WasSaveDataCreatedBefore<T>())
                        Assert.Fail("Missing save file");

                    return new T();
                }

                string decryptedData = isSecurity ?
                    _encryptor.Decrypt(loadedData) :
                    _encryptor.Decrypt(_securitySaveData.GeneratedEncryptionKey, loadedData);

                return _serializer.Deserialize<T>(decryptedData);
            }
            catch (Exception ex)
            {
                _loadError = true;
                LoadFail?.Invoke(typeof(T), ex);
                Debug.LogException(ex);

                return new T();
            }
        }

        private bool WasSaveDataCreatedBefore<T>()
            =>
            _securitySaveData.SaveDataStates?.Count > 0 &&
            _securitySaveData.SaveDataStates.Exists(item => item.SaveDataSaveName == SaveNames.GetSaveName(typeof(T)) && item.WasCreated);

        private T CreateSecuritySaveData<T>() where T : class, new()
        {
            SecuritySaveData data = new()
            {
                GeneratedEncryptionKey = _keyGenerator.Generate(32),
                SaveDataStates = new()
            };

            T result = data as T;
            Assert.IsNotNull(result);

            return result;
        }

        private bool TrySaveData<T>(T data, ref T savedData, string saveKey)
        {
            try
            {
                Assert.IsNotNull(data);
                Assert.That(!_loadError);

                if (EqualityComparer<T>.Default.Equals(data, savedData))
                    return false;

                string serializedObj = _serializer.Serialize(data);
                string encryptedData = typeof(T) == typeof(SecuritySaveData) ?
                    _encryptor.Encrypt(serializedObj) :
                    _encryptor.Encrypt(_securitySaveData.GeneratedEncryptionKey, serializedObj);

                _saver.Save(encryptedData, saveKey);

                if (typeof(T) != typeof(SecuritySaveData))
                    UpdateSecuriryData<T>();

                savedData = data;

                return true;
            }
            catch (Exception ex)
            {
                SaveFail?.Invoke(typeof(T), ex);
                Debug.LogException(ex);

                return false;
            }
        }

        private void UpdateSecuriryData<T>()
        {
            SecuritySaveData securityData = LoadSecurityData();

            string saveName = SaveNames.GetSaveName(typeof(T));
            int index = securityData.SaveDataStates.FindIndex(item => item.SaveDataSaveName == saveName);

            if (index < 0)
                securityData.SaveDataStates.Add(new()
                {
                    SaveDataSaveName = saveName,
                    WasCreated = true
                });
            else
                securityData.SaveDataStates[index].WasCreated = true;

            SaveSecurityData(securityData);
        }
    }
}
