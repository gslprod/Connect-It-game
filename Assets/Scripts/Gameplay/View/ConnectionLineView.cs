﻿using ConnectIt.Audio.Sounds;
using ConnectIt.Config;
using ConnectIt.Config.Wrappers;
using ConnectIt.Gameplay.Model;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ConnectIt.Gameplay.View
{
    public class ConnectionLineView : MonoBehaviour
    {
        private ConnectionLine _connectionLineModel;
        private GameplayViewConfig _gameplayViewConfig;
        private SoundsPlayer _soundsPlayer;
        private AudioConfig _audioConfig;
        private ConnectionLineViewSounds _sounds => _audioConfig.ConnectionLineViewSounds;

        private LineRenderer _lineRenderer;

        [Inject]
        public void Constructor(
            ConnectionLine model,
            GameplayViewConfig gameplayViewConfig,
            SoundsPlayer soundsPlayer,
            AudioConfig audioConfig)
        {
            _connectionLineModel = model;
            _gameplayViewConfig = gameplayViewConfig;
            _soundsPlayer = soundsPlayer;
            _audioConfig = audioConfig;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _connectionLineModel.Disposing += OnModelDisposing;

            UpdateColor();

            _soundsPlayer.Play(_sounds.Creating, SoundMixerGroup.Gameplay);
        }

        private void OnEnable()
        {
            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;

            UpdateView();
        }

        private void OnDisable()
        {
            _connectionLineModel.UsingTilesChanged += OnUsingTilesChanged;
        }

        private void OnDestroy()
        {
            _connectionLineModel.Disposing -= OnModelDisposing;
        }

        private void UpdateView()
        {
            UpdateLineVerticles();
            UpdateColor();
        }

        private void UpdateLineVerticles()
        {
            int usingTilesCount = _connectionLineModel.UsingTiles.Count();

            _lineRenderer.positionCount = usingTilesCount;

            for (int i = 0; i < usingTilesCount; i++)
            {
                Vector3 position = _connectionLineModel.UsingTiles.ElementAt(i).Tile.GetWorldPosition();

                _lineRenderer.SetPosition(i, position);
            }
        }

        private void UpdateColor()
        {
            Color startColor = _gameplayViewConfig.GetColorByCompatibilityIndex(_connectionLineModel.FirstCompatibilityIndex);
            Color endColor = _gameplayViewConfig.GetColorByCompatibilityIndex(
                _connectionLineModel.ConnectionCompleted ? 
                _connectionLineModel.SecondCompatibilityIndex :
                _connectionLineModel.FirstCompatibilityIndex); 

            _lineRenderer.startColor = startColor;
            _lineRenderer.endColor = endColor;
        }

        private void OnUsingTilesChanged(ConnectionLine line)
        {
            UpdateView();

            _soundsPlayer.Play(line.ConnectionCompleted ? _sounds.Completing : _sounds.Expanding, SoundMixerGroup.Gameplay);
        }

        private void OnModelDisposing(ConnectionLine obj)
        {
            _soundsPlayer.Play(_sounds.Destroying, SoundMixerGroup.Gameplay);

            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<ConnectionLine, ConnectionLineView> { }
    }
}
