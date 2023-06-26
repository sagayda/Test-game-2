using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Interfaces;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;

    public void Start()
    {
        _actionButtonsText[0].text += "asd";
    }

    private World CreateWorld()
    {
        return new World(0, "FirstWorld", CreatePlayer());
    }

    private IPlayerInfo CreatePlayerInfo()
    {
        return new PlayerInfo(0, "TestPlayer", "TEST", 100, 50, 1, 0, 100, 100, 100);
    }

    private Player CreatePlayer()
    {
        return new Player(CreatePlayerInfo());
    }

}
