using Assets.Scripts.InGameScripts;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private Vector2 _borders = new Vector2(2.5f, 5f);

    [SerializeField] private TextQuest _textQuest;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Canvas _mainCanvas;

    [SerializeField] private Camera _mapCamera;
    [SerializeField] private Canvas _mapCanvas;

    [SerializeField] private GameObject _mapObject;

    [SerializeField] private Sprite[] _connectorsTextures;

    private bool _mapEnabled = false;
    private bool _mapCreated = false;

    private bool _worldLoaded = false;
    private GameWorld _gameWorld;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (_gameWorld == null)
                _gameWorld = _textQuest.GetGameWorld();

            if (_mapEnabled)
                DisableMap();
            else
                EnableMap();
        }

        if (_mapEnabled)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");


            if (horizontalMovement > 0 && _mapCamera.transform.localPosition.x >= _borders.x)
                horizontalMovement = 0;
            else if (horizontalMovement < 0 && _mapCamera.transform.localPosition.x <= _borders.x * -1)
                horizontalMovement = 0;

            if (verticalMovement > 0 && _mapCamera.transform.localPosition.y >= _borders.y)
                verticalMovement = 0;
            else if (verticalMovement < 0 && _mapCamera.transform.localPosition.y <= _borders.y * -1)
                verticalMovement = 0;

            Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0) * _moveSpeed * Time.deltaTime;
            _mapCamera.transform.Translate(movement);

            // Приближение/отдаление камеры
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            float zoomAmount = zoomInput * _zoomSpeed * Time.deltaTime;
            _mapCamera.orthographicSize -= zoomAmount;
            _mapCamera.orthographicSize = Mathf.Clamp(_mapCamera.orthographicSize, 1f, 10f);
        }
    }

    private void EnableMap()
    {
        if (!_mapCreated)
            if (!CreateMap())
                return;


        _mainCamera.gameObject.SetActive(false);
        _mainCanvas.gameObject.SetActive(false);

        _mapCamera.gameObject.SetActive(true);
        _mapCanvas.gameObject.SetActive(true);

        _mapEnabled = true;
    }

    private void DisableMap()
    {
        if (!_mapCreated)
            if (!CreateMap())
                return;

        _mapCamera.gameObject.SetActive(false);
        _mapCanvas.gameObject.SetActive(false);

        _mainCamera.gameObject.SetActive(true);
        _mainCanvas.gameObject.SetActive(true);

        _mapEnabled = false;
    }

    private bool CreateMap()
    {
        if (!_worldLoaded)
            if (!LoadWorld())
                return false;

        int worldSize = _gameWorld.WorldSize;

        Texture2D worldTexture = new(worldSize, worldSize);

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                if (_gameWorld.World[i, j] == null)
                {
                    worldTexture.SetPixel(i, j, Color.white);
                    continue;
                }

                worldTexture.SetPixel(i, j, _gameWorld.World[i, j].Color);
            }
        }

        worldTexture.filterMode = FilterMode.Point;
        worldTexture.Apply();

        Sprite sprite = Sprite.Create(worldTexture, new Rect(0, 0, worldSize, worldSize), new Vector2(0.5f, 0.5f));

        _mapObject.transform.localScale = new Vector2(1000 / (worldSize / 100f), 1000 / (worldSize / 100f));
        _mapObject.GetComponent<SpriteRenderer>().sprite = sprite;

        _mapCreated = true;
        return true;
    }

    private bool LoadWorld()
    {
        GameWorld gameWorld = _textQuest.GetGameWorld();

        if (gameWorld == null)
            return false;

        _gameWorld = gameWorld;
        _worldLoaded = true;
        return true;
    }
}
