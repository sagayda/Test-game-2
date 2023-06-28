using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;

    private World _world;

    public void Start()
    {
        _world = CreateWorld();

        _statsController.SetStats(_world.Players[0].PlayerInfo);
    }

    public void NextTimeTick()
    {
        _world.TimeTickStep();
        _statsController.UpdateStats();
    }

    private World CreateWorld()
    {
        var world = new World(0, "FirstWorld", CreatePlayer());
        world.instantGameEvents.Add(new TestWorldInstantGameEvent(world));
        return world;
    }

    private IPlayerInfo CreatePlayerInfo()
    {
        return new PlayerInfo(0, "TestPlayer", "TEST", 100, 85, 50, 1, 0, 100, 100, 100, 10, 200, 100);
    }

    private Player CreatePlayer()
    {
        return new Player(CreatePlayerInfo());
    }

}
