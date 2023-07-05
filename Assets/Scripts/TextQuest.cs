using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.World.Absctract;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;
    [SerializeField] private GameObject _map;
    [SerializeField] private GridLayoutGroup _locationsLayoutGroup;
    [SerializeField] private GridLayoutGroup _connectorsLayoutGroup;
    [SerializeField] private GameObject _mapComponent_Locations;
    [SerializeField] private GameObject _mapComponent_Connectors;
    [SerializeField] private GameObject _locationPrefab;
    [SerializeField] private GameObject _connectorPrefab;
    [SerializeField] private Sprite[] _connectorsTextures;

    [SerializeField] private SpriteRenderer _sprtRend;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        int seed = 0;
        float zoom = 10f;

        var world = TestingTool.CreateWorld(seed, zoom, 64);

        var player = TestingTool.CreatePlayer(world.World[0, 0]);

        world.AddPlayer(player);

        Load(world, player);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (_map.activeSelf)
                _map.SetActive(false);
            else
                _map.SetActive(true);
        }
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
        CreateMap();
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

    private void MovePlayer(WorldLocation location)
    {
        _player.GoToLocation(location);
        LoadActions();
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

    public void NextTimeTick()
    {
        _world.TimeTickStep();
        _statsController.UpdateStats();
    }

    private void CreateMap()
    {
        foreach (Transform child in _mapComponent_Locations.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _mapComponent_Connectors.transform)
        {
            Destroy(child.gameObject);
        }

        if (_world == null)
            return;

        _locationsLayoutGroup.constraintCount = _world.World.GetLength(0);
        _connectorsLayoutGroup.constraintCount = _world.World.GetLength(0);

        Vector2 cellSize = new(1080f / _world.World.GetLength(0), 1000f / _world.World.GetLength(0));
        _locationsLayoutGroup.cellSize = cellSize;
        _connectorsLayoutGroup.cellSize = cellSize;

        for (int i = 0; i < _world.World.GetLength(0); i++)
        {
            for (int j = 0; j < _world.World.GetLength(1); j++)
            {
                if (_world.World[i, j] == null)
                {
                    InstantiateMapLocation(Color.white, i, j);
                    InstantiateMapConnector(Color.white, null);
                    continue;
                }

                InstantiateMapLocation(_world.World[i, j].Color, i, j);
                InstantiateMapConnector(_world.World[i, j].Color, _world.World[i, j]);

            }
        }
    }

    private Sprite GetConnectorSprite(WorldLocation location)
    {
        if (location == null)
            return _connectorsTextures[15];

        List<string> directions = new List<string>();

        foreach (var connector in location.Connectors)
        {
            directions.Add(connector.Direction.HumanName());
        }

        directions.Sort();

        string direction = string.Empty;

        foreach (string directionName in directions)
        {
            direction += directionName;
        }

        switch (direction)
        {
            case "EastSouthWest"://North
                return _connectorsTextures[0];
            case "NorthSouthWest"://East
                return _connectorsTextures[1];
            case "EastNorthWest"://South
                return _connectorsTextures[2];
            case "EastNorthSouth"://West
                return _connectorsTextures[3];
            case "SouthWest"://EastNorth
                return _connectorsTextures[4];
            case "EastWest"://NorthSouth
                return _connectorsTextures[5];
            case "EastSouth"://NorthWest
                return _connectorsTextures[6];
            case "NorthWest"://EastSouth
                return _connectorsTextures[7];
            case "NorthSouth"://EastWest
                return _connectorsTextures[8];
            case "EastNorth"://SouthWest
                return _connectorsTextures[9];
            case "West"://EastNorthSouth
                return _connectorsTextures[10];
            case "South"://EastNorthWest
                return _connectorsTextures[11];
            case "East"://NorthSouthWest
                return _connectorsTextures[12];
            case "North"://EastSouthWest
                return _connectorsTextures[13];
            case "":
                return _connectorsTextures[14];
            case "EastNothSouthWest":
                return _connectorsTextures[15];
            default:
                return null;
        }
    }

    private void InstantiateMapLocation(Color color, int x, int y)
    {
        var location = Instantiate(_locationPrefab, _mapComponent_Locations.transform);
        location.GetComponentInChildren<Image>().color = color;
        var locButton = location.GetComponentInChildren<Button>();
        locButton.onClick.AddListener(() => PrintLocationInfo(x, y));
    }
    private void InstantiateMapConnector(Color color, WorldLocation location)
    {
        var sprite = GetConnectorSprite(location);

        var connector = Instantiate(_connectorPrefab, _mapComponent_Connectors.transform);

        var conImage = connector.GetComponent<Image>();
        conImage.sprite = sprite;
        conImage.color = color;
    }

    private void PrintLocationInfo(int x, int y)
    {
        string res = string.Empty;

        res += "Locations: ";

        foreach (var item in _world.World[x, y].Connectors)
        {
            res += item.Direction.HumanName() + ", ";
        }

        res += $"\nNoise : {_world.World[x, y].Noise}";

        Debug.Log(res);
    }


}
