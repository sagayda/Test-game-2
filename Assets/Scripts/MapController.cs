using Assets.Scripts;
using Assets.Scripts.InGameScripts;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    #region map settings
    [SerializeField] private float _cameraMoveSpeed = 50f;
    [SerializeField] private float _cameraZoomSpeed = 4000f;
    [SerializeField] private Vector2 _borders = new Vector2(2.5f, 5f);

    private float _cameraMinFov;
    private readonly float _cameraMaxFov = 52;

    private const int LOCATION_CELL_SIZE = 16;
    #endregion

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

    private bool _mapEnabled = false;
    private bool _mapLoaded = false;
    private bool _settingsLoaded = false;
    private bool _worldLoaded = false;

    private GameWorld _gameWorld;

    private int _scaleLevel;
    private int _maxScaleLevel;
    private float _fovStepPerScaleLevel;
    private int _cellsCountStepPerScaleLevel = 64;

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
            { 
                Load();
                return;
            }

            if (_mapEnabled)
                DisableMap();
            else
                EnableMap();
        }

        if (_mapEnabled)
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

            Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0) * _cameraMoveSpeed * Time.deltaTime;
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
        if(!_mapLoaded)
        {
            Debug.Log("Map is not loaded!");
            return;
        }

        _mainCamera.gameObject.SetActive(false);
        _mainCanvas.gameObject.SetActive(false);

        _mapCamera.gameObject.SetActive(true);
        _mapCanvas.gameObject.SetActive(true);

        EventBus.WorldEvents.onPlayerPositionChanged += MovePlayerMark;
        _mapEnabled = true;
    }

    private void DisableMap()
    {
        _mapCamera.gameObject.SetActive(false);
        _mapCanvas.gameObject.SetActive(false);

        _mainCamera.gameObject.SetActive(true);
        _mainCanvas.gameObject.SetActive(true);

        Assets.Scripts.EventBus.WorldEvents.onPlayerPositionChanged -= MovePlayerMark;
        _mapEnabled = false;
    }

    private Texture2D PaintMap(Texture2D texture, GameWorld world, int width, int height)
    {
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
            Dictionary<Color,float> colorCounts = new Dictionary<Color,float>();

            for (int i = xStart; i <= xFinish; i++)
            {
                for (int j = yStart; j <= yFinish; j++)
                {
                    Color locationColor = world.World[i, j] == null ? Color.white : world.World[i,j].Color;

                    float increment = 1f;

                    if (world.World[i, j].Id == 3)
                        increment = 1.65f;

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
                if(pair.Value > maxCount)
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

    private bool LoadWorld()
    {
        GameWorld gameWorld = _textQuest.GetGameWorld();

        if (gameWorld == null)
            return false;

        _gameWorld = gameWorld;
        _worldLoaded = true;
        return true;
    }

    private bool LoadMap()
    {
        if (!_worldLoaded || !_settingsLoaded)
            return false;

        _mapSprites = new GameObject[_maxScaleLevel + 1];

        //Debug.Log($"{_gameWorld.WorldWidth}, {_gameWorld.WorldHeight}");

        //_mapTextures[0] = PaintMap(new Texture2D(_gameWorld.WorldWidth * LOCATION_CELL_SIZE, _gameWorld.WorldHeight * LOCATION_CELL_SIZE), _gameWorld, _gameWorld.WorldWidth, _gameWorld.WorldHeight);

        //for (int i = 1; i < _mapTextures.Length; i++)
        //{
        //    _mapTextures[i] = _mapTextures[0];
        //}

        for (int i = 0; i < _mapSprites.Length; i++)
        {
            if (i == _maxScaleLevel)
                continue;

            //int j = i;
            int j = _maxScaleLevel - i;

            int textureSize = j * _cellsCountStepPerScaleLevel * LOCATION_CELL_SIZE;

            Debug.Log($" i : {i}, txtr size : {textureSize}, width : {j * _cellsCountStepPerScaleLevel}");

            Texture2D texture = PaintMap(new Texture2D(textureSize, textureSize), _gameWorld, j * _cellsCountStepPerScaleLevel, j * _cellsCountStepPerScaleLevel);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            GameObject mapObject = Instantiate(_mapSpriteObjectPrefab, _mapParentTransform);
            mapObject.transform.localScale = new Vector2(100f / texture.width * 100f, 100f / texture.height * 100f);
            mapObject.GetComponent<SpriteRenderer>().sprite = sprite;
            mapObject.SetActive(false);
            mapObject.name = $"Map texture {_maxScaleLevel - i - 1} [{texture.width}x{texture.height}]";

            _mapSprites[_maxScaleLevel - i - 1] = mapObject;

            //_mapTextures[_maxScaleLevel - i - 1] = PaintMap(new Texture2D(textureSize, textureSize), _gameWorld, j * _cellsCountStepPerScaleLevel, j * _cellsCountStepPerScaleLevel);


        }

        SetMapTexture(1);

        //Texture2D worldTexture = new(worldSize * LOCATION_CELL_SIZE, worldSize * LOCATION_CELL_SIZE);

        //PaintMap(worldTexture);

        //worldTexture.filterMode = FilterMode.Point;
        //worldTexture.Apply();


        //Sprite sprite = Sprite.Create(worldTexture, new Rect(0, 0, worldSize * LOCATION_CELL_SIZE, worldSize * LOCATION_CELL_SIZE), new Vector2(0, 0));

        //_mapObject.transform.localScale = new Vector2((100f / (worldSize * LOCATION_CELL_SIZE)) * 100f, (100f / (worldSize * LOCATION_CELL_SIZE)) * 100f);
        //_mapObject.GetComponent<SpriteRenderer>().sprite = sprite;

        //_mapGrid.GetComponentInChildren<Tilemap>().SetTile(new Vector3Int(Mathf.RoundToInt(_gameWorld.Players[0].VectorPosition.x), Mathf.RoundToInt(_gameWorld.Players[0].VectorPosition.y) + 2, 0), _playerMarkTile);

        _mapLoaded = true;
        return true;
    }

    private bool LoadSettings()
    {
        if(!_worldLoaded)
            return false;

        _mapGrid.cellSize = new Vector3(100f / _gameWorld.WorldWidth, 100f / _gameWorld.WorldHeight);

        int worldSize = _gameWorld.WorldWidth > _gameWorld.WorldHeight ? _gameWorld.WorldWidth : _gameWorld.WorldHeight;

        _mapCamera.farClipPlane = worldSize * 2;
        _cameraMinFov = 200f / worldSize;
        _mapCamera.fieldOfView = _cameraMaxFov;

        _cellsCountStepPerScaleLevel = worldSize / 4;

        _maxScaleLevel = Mathf.CeilToInt(worldSize / _cellsCountStepPerScaleLevel);
        _scaleLevel = 0;
        _fovStepPerScaleLevel= (_cameraMaxFov - _cameraMinFov) / _maxScaleLevel;

        _settingsLoaded = true;
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

        return true;
    }

    private void SetMapTexture(int scaleLevel)
    {
        Debug.Log($"Setting texture {scaleLevel}");

        foreach (var map in _mapSprites)
        {
            if(map == null)
                continue;

            map.SetActive(false);
        }

        _mapSprites[scaleLevel].SetActive(true);
    }

    private void CheckScale()
    {
        if ((_maxScaleLevel - (_scaleLevel +1)) * _fovStepPerScaleLevel + _cameraMinFov > _mapCamera.fieldOfView && _scaleLevel != _maxScaleLevel)
        {
            IncreaseScaleLevel();
            Debug.Log($"Upscale: sc lvl: {_scaleLevel}, ({_maxScaleLevel} - {_scaleLevel}) * {_fovStepPerScaleLevel} + {_cameraMinFov} > {_mapCamera.fieldOfView}");
            return;
        }

        if ((_maxScaleLevel - (_scaleLevel +1) + 1) * _fovStepPerScaleLevel + _cameraMinFov < _mapCamera.fieldOfView && _scaleLevel != 0)
        {
            DecreaseScaleLevel();
            Debug.Log($"Downscale: sc lvl: {_scaleLevel}, ({_maxScaleLevel} - {_scaleLevel} + 1) * {_fovStepPerScaleLevel} + {_cameraMinFov} < {_mapCamera.fieldOfView}");
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
        Vector2 gridCellSize = new()
        {
            x = 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.width / 16f),
            y = 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.height / 16f),
        };
        //_mapGrid.cellSize = new Vector3(100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.width / 16f), 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.height / 16f));

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

        Vector2 gridCellSize = new()
        {
            x = 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.width / 16f),
            y = 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.height / 16f),
        };
        //_mapGrid.cellSize = new Vector3(100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.width / 16f), 100f / (_mapSprites[_scaleLevel].GetComponent<SpriteRenderer>().sprite.texture.height / 16f));

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
        Vector2 gameCoords = new();

        gameCoords.x = math.floor(worldCoords.x / (100f / _gameWorld.WorldWidth));
        gameCoords.y = math.floor((worldCoords.y * -1) / (100f / _gameWorld.WorldWidth));

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
