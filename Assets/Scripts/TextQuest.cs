using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.World.Absctract;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest : MonoBehaviour
{
    //[SerializeField] private int _worldSize = 128;

    [SerializeField] private TMP_Text _gameText;
    [SerializeField] private Button[] _actionButtons;
    [SerializeField] private TMP_Text[] _actionButtonsText;
    [SerializeField] private StatsController _statsController;
    [SerializeField] private WorldGenerator _worldGenerator;

    private GameWorld _world;
    private Player _player;

    public void Start()
    {
        var world = TestingTool.CreateWorld(_worldGenerator);

        var player = TestingTool.CreatePlayer(world);

        world.AddPlayer(player);

        Load(world, player);
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    MovePlayer(_player.VectorPosition + new Vector2(2, 0));
        //}

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

        //int i = 0;
        //foreach (var connector in _player.Location.Connectors)
        //{
        //    if (connector == null)
        //        continue;

        //    _actionButtons[i].gameObject.SetActive(true);
        //    _actionButtonsText[i].text = $"Go to {connector.ToLocation.Name} ({connector.Direction.HumanName()})";

        //    _actionButtons[i].onClick.RemoveAllListeners();
        //    _actionButtons[i].onClick.AddListener(() => MovePlayer(connector.ToLocation));

        //    i++;
        //}
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

    //private void MovePlayer(Location location)
    //{
    //    _player.GoToLocation(location);
    //    LoadActions();
    //}

    public void MovePlayer(Vector2 coords)
    {
        StartCoroutine(MoveToLocation(coords));

        IEnumerator MoveToLocation(Vector2 targetPosition)
        {
            // �������� � 1 �������
            yield return new WaitForSeconds(0.5f);

            _player.GoToCoordinates(targetPosition);

            //// ����������� ������ � ��������� �����������
            //while (VectorPosition != targetPosition)
            //{
            //    VectorPosition = Vector2.MoveTowards(VectorPosition, targetPosition, 1 * Time.deltaTime);
            //    yield return null;
            //}
        }
    }



    public void NextTimeTick()
    {
        _world.TimeTickStep();
        _statsController.UpdateStats();
    }

    public GameWorld GetGameWorld()
    {
        return _world;
    }

}
