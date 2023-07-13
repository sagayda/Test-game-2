using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.World.Absctract;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private int _worldSize = 128;

    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        int seed = 0;
        float zoom = 10f;

        var world = TestingTool.CreateWorld(seed, zoom, _worldSize);

        var player = TestingTool.CreatePlayer(world.World[0, 0]);

        world.AddPlayer(player);

        Load(world, player);
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
        world.AddPlayer(player);

        Load(world, player);
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
        ClearActions();

        if (_player == null || _world == null)
            return;

        int i = 0;
        foreach (var connector in _player.Location.Connectors)
        {
            if (connector == null)
                continue;

            _actionButtons[i].gameObject.SetActive(true);
            _actionButtonsText[i].text = $"Go to {connector.ToLocation.Name} ({connector.Direction.HumanName()})";

            _actionButtons[i].onClick.RemoveAllListeners();
            _actionButtons[i].onClick.AddListener(() => MovePlayer(connector.ToLocation));

            i++;
        }
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

    private void MovePlayer(Location location)
    {
        _player.GoToLocation(location);
        LoadActions();
    }

    public void NextTimeTick()
    {
        _world.TimeTickStep();
        _statsController.UpdateStats();
    }

    //private Sprite GetConnectorSprite(WorldLocation location)
    //{
    //    if (location == null)
    //        return _connectorsTextures[15];

    //    List<string> directions = new List<string>();

    //    foreach (var connector in location.Connectors)
    //    {
    //        directions.Add(connector.Direction.HumanName());
    //    }

    //    directions.Sort();

    //    string direction = string.Empty;

    //    foreach (string directionName in directions)
    //    {
    //        direction += directionName;
    //    }

    //    switch (direction)
    //    {
    //        case "EastSouthWest"://North
    //            return _connectorsTextures[0];
    //        case "NorthSouthWest"://East
    //            return _connectorsTextures[1];
    //        case "EastNorthWest"://South
    //            return _connectorsTextures[2];
    //        case "EastNorthSouth"://West
    //            return _connectorsTextures[3];
    //        case "SouthWest"://EastNorth
    //            return _connectorsTextures[4];
    //        case "EastWest"://NorthSouth
    //            return _connectorsTextures[5];
    //        case "EastSouth"://NorthWest
    //            return _connectorsTextures[6];
    //        case "NorthWest"://EastSouth
    //            return _connectorsTextures[7];
    //        case "NorthSouth"://EastWest
    //            return _connectorsTextures[8];
    //        case "EastNorth"://SouthWest
    //            return _connectorsTextures[9];
    //        case "West"://EastNorthSouth
    //            return _connectorsTextures[10];
    //        case "South"://EastNorthWest
    //            return _connectorsTextures[11];
    //        case "East"://NorthSouthWest
    //            return _connectorsTextures[12];
    //        case "North"://EastSouthWest
    //            return _connectorsTextures[13];
    //        case "":
    //            return _connectorsTextures[14];
    //        case "EastNothSouthWest":
    //            return _connectorsTextures[15];
    //        default:
    //            return null;
    //    }
    //}

    public GameWorld GetGameWorld()
    {
        return _world;
    }



}
