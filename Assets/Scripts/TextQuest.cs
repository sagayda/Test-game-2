using System.Collections;
using Assets.Scripts.Model.InGameScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniversalTools;
using WorldGeneration.Core;
using WorldGeneration.Core.Locations;

namespace Resources
{
    public class TextQuest : GenericSingleton<TextQuest>
    {
        [SerializeField] private TMP_Text _gameText;
        [SerializeField] private Button[] _actionButtons;
        [SerializeField] private TMP_Text[] _actionButtonsText;
        [SerializeField] private StatsController _statsController;
        [SerializeField] private WorldGenerator _worldGenerator;

        private GameWorld _world;
        private Player _player;

        public void Start()
        {

        }

        public void SaveGame()
        {
            SaveManager.SavePlayer(_player);
            SaveManager.SaveGameWorld(_world);
        }
        public void LoadGame()
        {
            var player = SaveManager.LoadPlayer();
            var world = SaveManager.LoadGameWorld();

            Load(world, player);
        }

        private void Load(GameWorld world, Player player)
        {
            _world = world;
            _player = player;
            _statsController.SetStats(player.Info);

            LoadActions();

            EventBus.WorldEvents.GameWorldLoaded?.Invoke();
        }

        private void LoadActions()
        {
            ClearActions();

            if (_player == null || _world == null)
                return;

        }

        private void ClearActions()
        {
            foreach (var actionBtn in _actionButtons)
            {
                actionBtn.gameObject.SetActive(false);
            }

            foreach (var actionBtnText in _actionButtonsText)
            {
                actionBtnText.text = string.Empty;
            }
        }

        public void MovePlayer(Vector2 coords)
        {
            StartCoroutine(MoveToLocation(coords));

            IEnumerator MoveToLocation(Vector2 targetPosition)
            {
                // Задержка в 1 секунду
                yield return new WaitForSeconds(0.5f);

                _player.GoToCoordinates(targetPosition);

            }
        }

        public void NextTimeTick()
        {
            _statsController.UpdateStats();
        }

        public GameWorld GetGameWorld()
        {
            return _world;
        }

    }
}
