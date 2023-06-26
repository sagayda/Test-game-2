using Assets.Scripts.InGameScripts;
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

    public void Start()
    {
        World world = CreateWorld();

        _statsController.SetStats(world.Players[0].PlayerInfo);
    }

    private World CreateWorld()
    {
        return new World(0, "FirstWorld", CreatePlayer());
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
