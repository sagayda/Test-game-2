using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.Events;
using Assets.Scripts.InGameScripts.Interfaces;
using Assets.Scripts.InGameScripts.World;
using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.Mathematics;
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
    [SerializeField] private GridLayoutGroup _mapLayoutGroup;
    [SerializeField] private GameObject _mapComponent;
    [SerializeField] private GameObject _locationPrefab;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        var world = TestingTool.CreateWorld(16);

        var player = TestingTool.CreatePlayer(world.World[0,0]);

        world.AddPlayer(player);

        Load(world, player);
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
            _actionButtonsText[i].text = $"Go to [{connector.ToLocation.Name}]";

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
        foreach (Transform child in _mapComponent.transform)
        {
            Destroy(child.gameObject);
        }

        if (_world == null)
            return;

        _mapLayoutGroup.constraintCount = _world.World.GetLength(0);

        for (int i = 0; i < _world.World.GetLength(0); i++)
        {
            for (int j = 0; j < _world.World.GetLength(1); j++)
            {
                if (_world.World[i, j] == null)
                {
                    InstantiateMapLocation(Color.white);
                    continue;
                }

                InstantiateMapLocation(_world.World[i, j].Color);
            }
        }
    }

    private void InstantiateMapLocation(Color color)
    {
        var location = Instantiate(_locationPrefab, _mapComponent.transform);
        location.GetComponentInChildren<Image>().color = color;
    }

}
