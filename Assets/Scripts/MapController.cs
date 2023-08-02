using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed = 50f;
    [SerializeField] private float _cameraZoomSpeed = 4000f;
    [SerializeField] private Vector2 _borders = new Vector2(2.5f, 5f);

    [SerializeField] private TextQuest _textQuest;

    [SerializeField] private GameObject _playerMarkPrefab;
    [SerializeField] private GameObject _locationSelectionPrefab;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Canvas _mainCanvas;

    [SerializeField] private Camera _mapCamera;
    [SerializeField] private Canvas _mapCanvas;

    [SerializeField] private Transform _mapParentTransform;

    [SerializeField] private GameObject _mapSpriteObjectPrefab;

    [SerializeField] private Grid _mapGrid;
    [SerializeField] private AnimatedTile _playerMarkTile;

    public bool IsMapEnabled { get; private set; } = false;
    public bool IsMapLoaded { get; private set; } = false;

    private GameWorld _gameWorld;

    private int _scaleLevel;
    private int _maxScaleLevel;
    private float _minToMaxResolutionRatio = 1f / 4f;

    private float _cameraMinFov;
    private float _cameraMaxFov;

    private const int LOCATION_CELL_SIZE = 16;

    private GameObject[] _mapSprites;

    private void Awake()
    {
        //Не работает
        Load();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (_gameWorld == null)
                if (!Load())
                    return;

            if (IsMapEnabled)
                DisableMap();
            else
                EnableMap();
        }

        if (IsMapEnabled)
        {
            #region cameraMoving
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");


            if (horizontalMovement > 0 && _mapCamera.transform.localPosition.x - 50 >= _borders.x)
                horizontalMovement = 0;
            else if (horizontalMovement < 0 && _mapCamera.transform.localPosition.x - 50 <= _borders.x * -1)
                horizontalMovement = 0;

            if (verticalMovement > 0 && _mapCamera.transform.localPosition.y + 50 >= _borders.y)
                verticalMovement = 0;
            else if (verticalMovement < 0 && _mapCamera.transform.localPosition.y + 50 <= _borders.y * -1)
                verticalMovement = 0;

            Vector3 movement = _cameraMoveSpeed * Time.deltaTime * new Vector3(horizontalMovement, verticalMovement, 0);
            _mapCamera.transform.Translate(movement);

            float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * _cameraZoomSpeed * Time.deltaTime;
            _mapCamera.fieldOfView -= zoomAmount;
            _mapCamera.fieldOfView = Mathf.Clamp(_mapCamera.fieldOfView, _cameraMinFov, _cameraMaxFov);

            CheckScale();

            #endregion

            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                mousePosition.z = 100;

                var worldCoords = _mapCamera.ScreenToWorldPoint(mousePosition);
                var locationCoords = GetLocationCoordinates(worldCoords);

                SelectLocation(locationCoords);
            }
        }
    }

    private void MovePlayerMark()
    {
        //_playerMarkTransform.position = GameCoordinatesToWorld(_gameWorld.Players[0].VectorPosition);
    }

    private void EnableMap()
    {
        if (!IsMapLoaded)
        {
            Debug.Log("Map is not loaded! Trying to load...");

            if (!Load())
            {
                Debug.Log("Failed to load map!");
                return;
            }

            Debug.Log("Successfully loaded map!");
        }

        _mainCamera.gameObject.SetActive(false);
        _mainCanvas.gameObject.SetActive(false);

        _mapCamera.gameObject.SetActive(true);
        _mapCanvas.gameObject.SetActive(true);

        EventBus.WorldEvents.onPlayerPositionChanged += MovePlayerMark;
        IsMapEnabled = true;
    }

    private void DisableMap()
    {
        if (!IsMapLoaded)
        {
            Debug.Log("Map is not loaded! Trying to load...");

            if (!Load())
            {
                Debug.Log("Failed to load map!");
                return;
            }

            Debug.Log("Successfully loaded map!");
        }

        _mapCamera.gameObject.SetActive(false);
        _mapCanvas.gameObject.SetActive(false);

        _mainCamera.gameObject.SetActive(true);
        _mainCanvas.gameObject.SetActive(true);

        Assets.Scripts.EventBus.WorldEvents.onPlayerPositionChanged -= MovePlayerMark;
        IsMapEnabled = false;
    }


    private bool LoadWorld()
    {
        GameWorld gameWorld = _textQuest.GetGameWorld();

        if (gameWorld == null)
            return false;

        _gameWorld = gameWorld;

        return true;
    }

    private bool LoadMap()
    {
        _mapSprites = new GameObject[_maxScaleLevel + 1];

        for (int scaleLevel = 0; scaleLevel < _mapSprites.Length; scaleLevel++)
        {
            if (scaleLevel == _maxScaleLevel)
            {
                Debug.Log("Max scale is not implemented");
                continue;
            }

            int mapWidth = Mathf.RoundToInt(_gameWorld.WorldWidth * ((_maxScaleLevel - scaleLevel) * _minToMaxResolutionRatio));
            int mapHeight = Mathf.RoundToInt(_gameWorld.WorldHeight * ((_maxScaleLevel - scaleLevel) * _minToMaxResolutionRatio));

            Texture2D texture = PaintMap(_gameWorld, mapWidth, mapHeight);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            GameObject mapObject = Instantiate(_mapSpriteObjectPrefab, _mapParentTransform);

            int largerEdge = texture.width >= texture.height ? texture.width : texture.height;

            mapObject.transform.localScale = new Vector2(100f / largerEdge * 100f, 100f / largerEdge * 100f);
            mapObject.GetComponent<SpriteRenderer>().sprite = sprite;
            mapObject.SetActive(false);
            mapObject.name = $"Map texture {_maxScaleLevel - scaleLevel - 1} [{texture.width}x{texture.height}]";

            _mapSprites[_maxScaleLevel - scaleLevel - 1] = mapObject;
        }

        SetMapTexture(0);

        return true;
    }

    private bool LoadSettings()
    {
        _mapGrid.cellSize = new Vector3(100f / _gameWorld.WorldWidth, 100f / _gameWorld.WorldHeight);

        int worldSize = _gameWorld.WorldWidth > _gameWorld.WorldHeight ? _gameWorld.WorldWidth : _gameWorld.WorldHeight;

        _mapCamera.farClipPlane = worldSize * 2;
        _cameraMinFov = 200f / worldSize;
        _cameraMaxFov = _gameWorld.WorldWidth > _gameWorld.WorldHeight ? 34f : 61f;
        _mapCamera.fieldOfView = _cameraMaxFov;

        _maxScaleLevel = Mathf.CeilToInt(1f / _minToMaxResolutionRatio);
        _scaleLevel = 0;

        return true;
    }

    private bool Load()
    {
        if (!LoadWorld())
        {
            Debug.Log("Failed to load world!");
            return false;
        }

        if (!LoadSettings())
        {
            Debug.Log("Failed to load settings!");
            return false;
        }

        if (!LoadMap())
        {
            Debug.Log("Failed to load map!");
            return false;
        }

        IsMapLoaded = true;

        return true;
    }

    private void SetMapTexture(int scaleLevel)
    {
        foreach (var map in _mapSprites)
        {
            if (map == null)
                continue;

            map.SetActive(false);
        }

        _mapSprites[scaleLevel].SetActive(true);
    }

    private Texture2D PaintMap(GameWorld world, int width, int height)
    {
        Texture2D texture = new(width * LOCATION_CELL_SIZE, height * LOCATION_CELL_SIZE);

        float horizontalRatio = world.WorldWidth / (float)width;
        float verticalRatio = world.WorldHeight / (float)height;

        Debug.Log($"width {width}, height {height}, ration h {horizontalRatio}, ration v {verticalRatio}");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int xStart = Mathf.RoundToInt(i * horizontalRatio);
                int yStart = Mathf.RoundToInt(j * verticalRatio);

                int xFinish = Mathf.RoundToInt((i + 1) * horizontalRatio) - 1;
                int yFinish = Mathf.RoundToInt((j + 1) * verticalRatio) - 1;

                Color color = GetColorInSquare(world, xStart, yStart, xFinish, yFinish);

                if (i == height - 1)
                {
                    SetCell(texture, i, j, Color.black);
                }

                SetCell(texture, i, j, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;

        Color GetColorInSquare(GameWorld world, int xStart, int yStart, int xFinish, int yFinish)
        {
            Dictionary<Color, float> colorCounts = new Dictionary<Color, float>();

            for (int i = xStart; i <= xFinish; i++)
            {
                for (int j = yStart; j <= yFinish; j++)
                {
                    Color locationColor = world.World[i, j] == null ? Color.white : world.World[i, j].Color;

                    float increment = 1f;

                    if (world.World[i, j].Id == 3)
                        increment = 3.5f;

                    if (colorCounts.ContainsKey(locationColor))
                    {
                        colorCounts[locationColor] += increment;
                    }
                    else
                    {
                        colorCounts[locationColor] = increment;
                    }
                }

            }

            Color resultColor = Color.black;
            float maxCount = 0;

            foreach (var pair in colorCounts)
            {
                if (pair.Value > maxCount)
                {
                    resultColor = pair.Key;
                    maxCount = pair.Value;
                }
            }

            return resultColor;

        }

        void SetCell(Texture2D texture, int x, int y, Color color)
        {
            for (int i = x * LOCATION_CELL_SIZE; i < (x * LOCATION_CELL_SIZE) + LOCATION_CELL_SIZE; i++)
            {
                for (int j = y * LOCATION_CELL_SIZE; j < (y * LOCATION_CELL_SIZE) + LOCATION_CELL_SIZE; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }

    }

    private void CheckScale()
    {
        float fovStepPerScaleLevel = (_cameraMaxFov - _cameraMinFov) / _maxScaleLevel;

        float lowerBound = (_maxScaleLevel - (_scaleLevel + 1)) * fovStepPerScaleLevel + _cameraMinFov;
        float upperBound = (_maxScaleLevel - (_scaleLevel + 1) + 1) * fovStepPerScaleLevel + _cameraMinFov;

        if (_mapCamera.fieldOfView < lowerBound && _scaleLevel != _maxScaleLevel)
        {
            IncreaseScaleLevel();
            Debug.Log($"Upscale: sc lvl: {_scaleLevel}, ({_maxScaleLevel} - {_scaleLevel}) * {fovStepPerScaleLevel} + {_cameraMinFov} > {_mapCamera.fieldOfView}");
            return;
        }

        if (_mapCamera.fieldOfView > upperBound && _scaleLevel != 0)
        {
            DecreaseScaleLevel();
            Debug.Log($"Downscale: sc lvl: {_scaleLevel}, ({_maxScaleLevel} - {_scaleLevel} + 1) * {fovStepPerScaleLevel} + {_cameraMinFov} < {_mapCamera.fieldOfView}");
            return;
        }
    }

    private void IncreaseScaleLevel()
    {
        if (_scaleLevel >= _maxScaleLevel)
        {
            return;
        }

        _scaleLevel++;

        Texture2D texture = _mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture;
        int textureSize = texture.width >= texture.height ? texture.width : texture.height;

        Vector2 gridCellSize = new()
        {
            x = 100f / (textureSize / 16f),
            y = 100f / (textureSize / 16f),
        };

        _mapGrid.cellSize = gridCellSize;

        SetMapTexture(_scaleLevel);
        EventBus.MapEvents.onMapScaleChanged?.Invoke();
    }

    private void DecreaseScaleLevel()
    {
        if (_scaleLevel <= 0)
        {
            return;
        }

        _scaleLevel--;

        Texture2D texture = _mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture;
        int textureSize = texture.width >= texture.height ? texture.width : texture.height;

        Vector2 gridCellSize = new()
        {
            x = 100f / (textureSize / 16f),
            y = 100f / (textureSize / 16f),
        };

        _mapGrid.cellSize = gridCellSize;

        SetMapTexture(_scaleLevel);
        EventBus.MapEvents.onMapScaleChanged?.Invoke();
    }

    private Vector2 GetLocationCoordinates(Vector2 worldCoordinates)
    {
        Debug.Log($"WC: {worldCoordinates.x}, {worldCoordinates.y}");

        Vector2 locationCellSize = new Vector2(100f / _gameWorld.WorldWidth, 100f / _gameWorld.WorldHeight);

        Vector2 spacing = new Vector2(worldCoordinates.x % locationCellSize.x, worldCoordinates.y % locationCellSize.y);

        Debug.Log($"LC: {worldCoordinates.x - spacing.x}, {worldCoordinates.y + spacing.y}");


        return new Vector2(worldCoordinates.x - spacing.x, worldCoordinates.y - spacing.y);
    }

    private Vector2 ScreenCoordinatesToWorld(Vector3 screenCoordinates)
    {
        screenCoordinates.z = Mathf.Abs(_mapCamera.transform.position.z - _mapSpriteObjectPrefab.transform.position.z);

        return _mapCamera.ScreenToWorldPoint(screenCoordinates);
    }

    private Vector2 WorldCoordinatesToGame(Vector2 worldCoords)
    {
        Vector2 gameCoords = new()
        {
            x = math.floor(worldCoords.x / (100f / _gameWorld.WorldWidth)),
            y = math.floor((worldCoords.y * -1) / (100f / _gameWorld.WorldWidth))
        };

        return gameCoords;
    }

    private Vector2 GameCoordinatesToWorld(Vector2 gameCoords)
    {
        Vector2 worldCoords = new();

        float locationCellSize = 100f / _gameWorld.WorldWidth;

        worldCoords.x = gameCoords.x * locationCellSize + (locationCellSize / 2f);
        worldCoords.y = gameCoords.y * -1 * locationCellSize - (locationCellSize / 2f);

        return worldCoords;
    }

    private void SelectLocation(Vector2 locationCoords)
    {
        ////Vector2 locationCellCoords = new(Mathf.Floor(screenCoords.x), Mathf.Floor(screenCoords.y));

        //_locationSelectionTransform = Instantiate(_locationSelectionPrefab, _mapObject.transform).transform;

        //_locationSelectionTransform.localPosition = locationCoords;
    }

    private void PrintLocationInfo(Vector2 coords)
    {
        int x = (int)math.floor(coords.x);
        int y = (int)math.floor(coords.y);

        string res = string.Empty;

        res += $"Name: {_gameWorld.World[x, y].Name}";
        res += $"\nX: {coords.x} Y:{coords.y}";

        res += $"\nNoise : {_gameWorld.World[x, y].Noise}";

        Debug.Log(res);
    }
}
