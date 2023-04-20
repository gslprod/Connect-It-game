﻿using ConnectIt.Save.SaveProviders.SaveData;
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
        IGameplaySaveProvider, IShopSaveProvider,
        IStatsSaveProvider, IExternalServerSaveProvider
    {
        public event Action GameplaySaveDataChanged;
        public event Action ShopSaveDataChanged;
        public event Action StatsSaveDataChanged;
        public event Action ExternalServerSaveDataChanged;

        private readonly ISaver _saver;
        private readonly ISerializer _serializer;

        private GameplaySaveData _gameplaySaveData;
        private ShopSaveData _shopSaveData;
        private StatsSaveData _statsSaveData;
        private ExternalServerSaveData _externalServerSaveData;

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
            => (GameplaySaveData)_gameplaySaveData.Clone();

        public void SaveShopData(ShopSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _shopSaveData, ShopSaveData.SaveKey))
                return;

            ShopSaveDataChanged?.Invoke();
        }

        public ShopSaveData LoadShopData()
            => (ShopSaveData)_shopSaveData.Clone();

        public void SaveStatsData(StatsSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _statsSaveData, StatsSaveData.SaveKey))
                return;

            StatsSaveDataChanged?.Invoke();
        }

        public StatsSaveData LoadStatsData()
            => (StatsSaveData)_statsSaveData.Clone();

        public void SaveExtrenalServerData(ExternalServerSaveData saveData)
        {
            if (!TrySaveData(saveData, ref _externalServerSaveData, ExternalServerSaveData.SaveKey))
                return;

            ExternalServerSaveDataChanged?.Invoke();
        }

        public ExternalServerSaveData LoadExternalServerData()
            => (ExternalServerSaveData)_externalServerSaveData.Clone();

        private void LoadAllData()
        {
            _gameplaySaveData = LoadAndDeserialize<GameplaySaveData>(GameplaySaveData.SaveKey);
            _shopSaveData = LoadAndDeserialize<ShopSaveData>(ShopSaveData.SaveKey);
            _statsSaveData = LoadAndDeserialize<StatsSaveData>(StatsSaveData.SaveKey);
            _externalServerSaveData = LoadAndDeserialize<ExternalServerSaveData>(ExternalServerSaveData.SaveKey);
        }

        private T LoadAndDeserialize<T>(string loadKey)
            => _serializer.Deserialize<T>(_saver.Load(loadKey));

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
