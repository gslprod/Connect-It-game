using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Save.Savers;
using ConnectIt.Save.Serializers;
using ConnectIt.Utilities;
using System;
using System.Collections.Generic;
using Zenject;

namespace ConnectIt.Save.SaveProviders
{
    public class GameSaveProvider :
        IInitializable,
        IGameplaySaveProvider, IShopSaveProvider, IStatsSaveProvider, IExternalServerSaveProvider, ISettingsSaveProvider
    {
        public event Action GameplaySaveDataChanged;
        public event Action ShopSaveDataChanged;
        public event Action StatsSaveDataChanged;
        public event Action ExternalServerSaveDataChanged;
        public event Action SettingsSaveDataChanged;

        private readonly ISaver _saver;
        private readonly ISerializer _serializer;

        private GameplaySaveData _gameplaySaveData;
        private ShopSaveData _shopSaveData;
        private StatsSaveData _statsSaveData;
        private ExternalServerSaveData _externalServerSaveData;
        private SettingsSaveData _settingsSaveData;

        public GameSaveProvider(
            ISaver saver,
            ISerializer serializer)
        {
            _saver = saver;
            _serializer = serializer;
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

        public void SaveExtrenalServerData(ExternalServerSaveData saveData)
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

        private void LoadAllData()
        {
            _gameplaySaveData = LoadAndDeserialize<GameplaySaveData>(GameplaySaveData.SaveKey);
            _shopSaveData = LoadAndDeserialize<ShopSaveData>(ShopSaveData.SaveKey);
            _statsSaveData = LoadAndDeserialize<StatsSaveData>(StatsSaveData.SaveKey);
            _externalServerSaveData = LoadAndDeserialize<ExternalServerSaveData>(ExternalServerSaveData.SaveKey);
            _settingsSaveData = LoadAndDeserialize<SettingsSaveData>(SettingsSaveData.SaveKey);
        }

        private T LoadAndDeserialize<T>(string loadKey) where T : new()
        {
            string serializedObj = _saver.Load(loadKey);
            if (string.IsNullOrEmpty(serializedObj))
                return new T();

            return _serializer.Deserialize<T>(serializedObj);
        }

        private bool TrySaveData<T>(T data, ref T savedData, string saveKey)
        {
            Assert.IsNotNull(data);

            if (EqualityComparer<T>.Default.Equals(data, savedData))
                return false;

            string serializedObj = _serializer.Serialize(data);
            _saver.Save(serializedObj, saveKey);

            savedData = data;

            return true;
        }
    }
}
