using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Interfaces;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;
    [SerializeField] private GameObject _map;
    [SerializeField] private GridLayoutGroup _mapLayoutGroup;
    [SerializeField] private GameObject _wastelandPrefab;
    [SerializeField] private GameObject _emptyPrefab;
    [SerializeField] private GameObject _mapComponent;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        var pl = CreatePlayer();

        Load(CreateWorld(pl,16), pl);
        CreateMap();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            if(_map.activeSelf)
                _map.SetActive(false);
            else
                _map.SetActive(true);
        }
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

    private void CreateMap()
    {
        if (_world == null)
            return;

        _mapLayoutGroup.constraintCount = _world.World.GetLength(0);

        for (int i = 0; i < _world.World.GetLength(0); i++)
        {
            for (int j = 0; j < _world.World.GetLength(1); j++)
            {
                if (_world.World[i, j] == null)
                {
                    Instantiate(_emptyPrefab, _mapComponent.transform);
                    continue;
                }

                Instantiate(_wastelandPrefab, _mapComponent.transform);
            }
        }
    }

    private GameWorld CreateWorld(Player player, byte size)
    {
        var world = new GameWorld(0, "FirstWorld", player);
        world.instantGameEvents.Add(new TestInstantGameEvent(world));
        world.instantGameEvents.Add(new TestInstantGameEvent(world));
        world.instantGameEvents.Add(new TestInstantGameEvent(world));

        world.World = CreateWorldMap(size);

        return world;
    }

    private IWorldLocation[,] CreateWorldMap(int size)
    {
        IWorldLocation[,] map = new IWorldLocation[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (UnityEngine.Random.Range(0, 100) >= 80)
                    continue;

                map[i, j] = new WorldLocation_Wasteland();
                map[i,j].X = i;
                map[i,j].Y = j;
            }
        }

        for (int i = 0; i < size-1; i++)
        {
            for (int j = 0; j < size-1; j++)
            {
                if (map[i, j] == null)
                    continue;

                IWorldLocationConnector[] connectors;

                if (i == 0 &&  j == 0)
                {
                    connectors = new IWorldLocationConnector[2];

                    if(map[0, 1] != null)
                        connectors[0] = new WorldLocationConnector_Free(map[0, 0], map[0, 1]);

                    if (map[1,0] != null)
                        connectors[1] = new WorldLocationConnector_Free(map[0, 0], map[1, 0]);

                    map[0,0].Connectors = connectors;
                    continue;
                }

                if(i== 0)
                {
                    if(UnityEngine.Random.Range(0,100) >= 95)
                    {
                        connectors = new IWorldLocationConnector[2];
                    }
                    else
                    {
                        connectors = new IWorldLocationConnector[3];
                    }

                    if(map[i, j - 1]!= null)
                        connectors[0] = new WorldLocationConnector_Free(map[i,j], map[i,j-1]);
                    if (map[i, j + 1] != null)
                        connectors[1] = new WorldLocationConnector_Free(map[i, j], map[i, j + 1]);

                    if(connectors.Length > 2)
                    {
                        if (map[i + 1, j] != null)
                            connectors[2] = new WorldLocationConnector_Free(map[i, j], map[i + 1, j]);
                    }

                    map[i,j].Connectors = connectors;
                    continue;
                }

                if (j == 0)
                {
                    if (UnityEngine.Random.Range(0, 100) >= 95)
                    {
                        connectors = new IWorldLocationConnector[2];
                    }
                    else
                    {
                        connectors = new IWorldLocationConnector[3];
                    }

                    if (map[i - 1, j] != null)
                        connectors[0] = new WorldLocationConnector_Free(map[i, j], map[i - 1, j]);


                    if (map[i + 1, j] != null)
                        connectors[1] = new WorldLocationConnector_Free(map[i, j], map[i + 1, j]);

                    if (connectors.Length > 2)
                    {
                        if (map[i, j + 1] != null)
                            connectors[2] = new WorldLocationConnector_Free(map[i, j], map[i, j + 1]);
                    }

                    map[i, j].Connectors = connectors;
                    continue;
                }

                if (UnityEngine.Random.Range(0, 100) >= 95)
                {
                    connectors = new IWorldLocationConnector[3];
                }
                else
                {
                    connectors = new IWorldLocationConnector[4];
                }

                if (map[i - 1, j] != null)
                    connectors[0] = new WorldLocationConnector_Free(map[i, j], map[i - 1, j]);
                if (map[i + 1, j] != null)
                    connectors[1] = new WorldLocationConnector_Free(map[i, j], map[i + 1, j]);
                if (map[i, j + 1] != null)
                    connectors[2] = new WorldLocationConnector_Free(map[i, j], map[i, j + 1]);

                if(connectors.Length > 3)
                {
                    if (map[i, j - 1] != null)
                        connectors[3] = new WorldLocationConnector_Free(map[i, j], map[i, j - 1]);
                }

                map[i,j].Connectors = connectors;
            }
        }

        return map;
    }


    //private IWorldLocation[] CreateWorldLevel(int level, IWorldLocation[] previousLevel)
    //{
    //    if(level<1)
    //        return null;

    //    int prevLevelLocationsCount = 0;
    //    int maxConnectorsCount = 0;

    //    foreach(var location in previousLevel)
    //    {
    //        if(location == null)
    //            continue;

    //        prevLevelLocationsCount++;

    //        if (math.abs(location.X) == math.abs(location.Y))
    //            maxConnectorsCount += 2;
    //        else
    //            maxConnectorsCount++;
    //    }

    //    int locationsCount = UnityEngine.Random.Range(level * 6, level * 8);

    //    IWorldLocation[] worldLevel = new IWorldLocation[locationsCount];
    //    int locationsToSkip = level * 8 - locationsCount;

    //    for (int i = 0; i < worldLevel.Length; i++)
    //    {
    //        if (UnityEngine.Random.Range(0, 100) > 85)
    //        {
    //            if (locationsToSkip > 0)
    //            {
    //                locationsToSkip--;
    //                continue;
    //            }
    //        }

    //        worldLevel[i] = new WorldLocation_Wasteland();

    //        if(i < )

    //        float ortantCoeff = (((float)(8 * level) )/ 4);
    //        float ortant = i / ortantCoeff;
    //        if (ortant < ortantCoeff)
    //        {
    //            //1

    //        }
    //    }

    //}

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
