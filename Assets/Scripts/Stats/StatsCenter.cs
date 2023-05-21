using ConnectIt.Coroutines;
using ConnectIt.Save.SaveNames;
using ConnectIt.Save.SaveProviders;
using ConnectIt.Save.SaveProviders.SaveData;
using ConnectIt.Stats.Data;
using ConnectIt.Stats.Modules;
using ConnectIt.Utilities;
using ConnectIt.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static ConnectIt.Save.SaveProviders.SaveData.StatsSaveData;

namespace ConnectIt.Stats
{
    public class StatsCenter : IStatsCenter, IInitializable, IDisposable
    {
        public IEnumerable<IStatsData> StatsData => _data;

        private const float OftenUpdatingDataSaveDelaySec = 10f;

        private readonly List<IStatsModule> _modules = new();
        private readonly List<IStatsData> _data = new();
        private readonly Type[] _statsDataTypesToCreate = new Type[]
        {
            typeof(ApplicationRunningTimeStatsData),
            typeof(MovesCountStatsData)
        };

        private readonly IStatsSaveProvider _saveProvider;
        private readonly ICoroutinesGlobalContainer _coroutinesGlobalContainer;
        private readonly ApplicationRunningTimeStatsData.Factory _applicationRunningTimeStatsDataFactory;
        private readonly MovesCountStatsData.Factory _movesCountStatsDataFactory;

        private bool _oftenUpdatingDataSavingRequested = false;
        private bool _savingRequested = false;

        public StatsCenter(
            IStatsSaveProvider saveProvider,
            ICoroutinesGlobalContainer coroutinesGlobalContainer,
            ApplicationRunningTimeStatsData.Factory applicationRunningTimeStatsDataFactory,
            MovesCountStatsData.Factory movesCountStatsDataFactory)
        {
            _saveProvider = saveProvider;
            _coroutinesGlobalContainer = coroutinesGlobalContainer;
            _applicationRunningTimeStatsDataFactory = applicationRunningTimeStatsDataFactory;
            _movesCountStatsDataFactory = movesCountStatsDataFactory;
        }

        public void Initialize()
        {
            if (TryLoadAnyData())
                return;

            CreateMissingStatsData();
        }

        public void Dispose()
        {
            SaveAllStatsData();
        }

        public void RegisterModule(IStatsModule module)
        {
            Assert.ArgIsNotNull(module);

            _modules.Add(module);
        }

        public void UnregisterModule(IStatsModule module)
        {
            Assert.ArgIsNotNull(module);

            Assert.That(
                _modules.Remove(module));
        }

        public T GetData<T>() where T : IStatsData
            => (T)GetData(typeof(T));

        public IStatsData GetData(Type type)
        {
            int index = GetDataIndex(type);

            return _data[index];
        }

        private void AddStatsData(IStatsData data)
        {
            data.ValueChanged += OnDataValueChanged;
            _data.Add(data);
        }

        private void OnDataValueChanged(IStatsData data)
        {
            if (_savingRequested)
                return;

            if (data.OftenUpdating)
            {
                OftenUpdatingDataSaveHandling();
                return;
            }

            _savingRequested = true;
            _coroutinesGlobalContainer.DelayedAction(SaveHandling, new WaitForEndOfFrame());
        }

        private void OftenUpdatingDataSaveHandling()
        {
            if (_oftenUpdatingDataSavingRequested)
                return;

            _oftenUpdatingDataSavingRequested = true;
            _coroutinesGlobalContainer.DelayedAction(SaveHandling, OftenUpdatingDataSaveDelaySec);
        }

        private void SaveHandling()
        {
            if (!_savingRequested && !_oftenUpdatingDataSavingRequested)
                return;

            SaveAllStatsData();

            _savingRequested = false;
            _oftenUpdatingDataSavingRequested = false;
        }

        private int GetDataIndex(Type type)
            => _data.FindIndex(item => item.GetType() == type);

        private bool TryLoadAnyData()
        {
            StatsSaveData saveData = _saveProvider.LoadStatsData();

            bool loaded =
                TryLoadData(saveData.DoubleStats) |
                TryLoadData(saveData.LongStats) |
                TryLoadData(saveData.StringStats);

            CreateMissingStatsData();
            SortData();

            return loaded;
        }

        private void SortData()
        {
            _data.Sort(
                (first, second) =>
                {
                    int firstIndex = _statsDataTypesToCreate.IndexOf(first.GetType());
                    int secondIndex = _statsDataTypesToCreate.IndexOf(second.GetType());

                    return firstIndex.CompareTo(secondIndex);
                });
        }

        private bool TryLoadData<T>(IEnumerable<StatsElementSaveData<T>> saveData)
        {
            if (saveData == null)
                return false;

            foreach (StatsElementSaveData<T> data in saveData)
            {
                Type type = SaveNames.GetTypeBySaveName(data.StatsElementSaveName);

                StatsDataBase<T> statsData = (StatsDataBase<T>)CreateStatsDataByType(type);
                statsData.RawValue = data.Value;

                AddStatsData(statsData);
            }

            return true;
        }

        private void CreateMissingStatsData()
        {
            bool missingDataDetected = false;
            foreach (Type type in _statsDataTypesToCreate)
            {
                if (_data.Exists(item => item.GetType() == type))
                    continue;

                missingDataDetected = true;

                IStatsData statsData = CreateStatsDataByType(type);
                AddStatsData(statsData);
            }

            if (missingDataDetected)
                SaveAllStatsData();
        }

        private void SaveAllStatsData()
        {
            StatsSaveData saveData = _saveProvider.LoadStatsData();

            SaveStatsDataTo(ref saveData.DoubleStats);
            SaveStatsDataTo(ref saveData.LongStats);
            SaveStatsDataTo(ref saveData.StringStats);

            _saveProvider.SaveStatsData(saveData);
        }

        private void SaveStatsDataTo<T>(ref StatsElementSaveData<T>[] saveDataArray)
        {
            List<StatsElementSaveData<T>> saveDataList = new();
            foreach (IStatsData data in _data)
            {
                if (data is not StatsDataBase<T> genericData)
                    continue;

                StatsElementSaveData<T> saveData = new()
                {
                    StatsElementSaveName = SaveNames.GetSaveName(genericData.GetType()),
                    Value = genericData.RawValue
                };

                saveDataList.Add(saveData);
            }

            saveDataArray = saveDataList.ToArray();
        }

        private IStatsData CreateStatsDataByType(Type type)
        {
            if (type == typeof(ApplicationRunningTimeStatsData))
                return _applicationRunningTimeStatsDataFactory.Create();

            if (type == typeof(MovesCountStatsData))
                return _movesCountStatsDataFactory.Create();

            throw Assert.GetFailException();
        }
    }
}
