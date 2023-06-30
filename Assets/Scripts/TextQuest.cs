using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Interfaces;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        var pl = CreatePlayer();

        Load(CreateWorld(pl), pl);
    }

    public void SaveGame()
    {
        SaveManager.SavePlayer(_player);
    }

    public void LoadGame()
    {
        var player = SaveManager.LoadPlayer();

        Load(_world, player);
    }

    private void Load(GameWorld world, Player player)
    {
        _world = world;
        _player = player;
        _statsController.SetStats(player.Info);

        LoadActions();
    }

    private void LoadActions()
    {
        if(_player == null || _world == null)
            return;

        ClearActions();

        for (int i = 0; i < _player.Location.NeighbourLocations.Length; i++)
        {
            var location = _player.Location.NeighbourLocations[i];

            if (location == null)
                continue;

            _actionButtons[i].gameObject.SetActive(true);
            _actionButtonsText[i].text = $"Go to [{location.Name}]";
            _actionButtons[i].onClick.AddListener(() => MovePlayer(location));
        }
    }

    private void MovePlayer(IWorldLocation location)
    {
        _player.GoToLocation(location);
        LoadActions();
    }

    private void ClearActions()
    {
        foreach(var actionBtn in _actionButtons)
        {
            actionBtn.gameObject.SetActive(false);
        }

        foreach(var actionBtnText in _actionButtonsText)
        {
            actionBtnText.text = string.Empty;
        }
    }

    public void NextTimeTick()
    {
        _world.TimeTickStep();
        _statsController.UpdateStats();
    }

    private GameWorld CreateWorld(Player player)
    {
        var world = new GameWorld(0, "FirstWorld", player);
        world.instantGameEvents.Add(new TestInstantGameEvent(world));
        world.instantGameEvents.Add(new TestInstantGameEvent(world));
        world.instantGameEvents.Add(new TestInstantGameEvent(world));

        return world;
    }

    private IPlayerInfo CreatePlayerInfo()
    {
        return new PlayerInfo(0, "TestPlayer", "TEST", 100, 85, 50, 1, 0, 100, 100, 100, 10, 200, 100);
    }

    private Player CreatePlayer()
    {
        WorldLocation_Wasteland mainLocation = new(new WorldSublocation_Wasteland());
        WorldLocation_Wasteland secondLocation = new(new WorldSublocation_Wasteland());
        WorldLocation_Wasteland thirdLocation = new(new WorldSublocation_Wasteland());

        thirdLocation.SetNeighbours(secondLocation);

        List<WorldLocation_Wasteland> tempList = new();

        tempList.Add(thirdLocation);
        tempList.Add(mainLocation);
        tempList.Add(new(new WorldSublocation_Wasteland()));

        secondLocation.SetNeighbours(tempList.ToArray());

        tempList.Clear();

        tempList.Add(secondLocation);
        tempList.Add(new(new WorldSublocation_Wasteland()));
        tempList.Add(new(new WorldSublocation_Wasteland()));

        mainLocation.SetNeighbours(tempList.ToArray());

        return new Player(CreatePlayerInfo(), mainLocation);
    }

}
