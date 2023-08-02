using Assets.Scripts.InGameScripts;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapScaler
    {
        public float CameraZoomSpeed { get; set; }
        public float CameraMaxFov { get; set; }
        public float CameraMinFov { get; set; }

        public int ScaleLevel { get; set; }
        public float ScaleLevelStep { get; set; }
        public int MaxScaleLevel { get; set; }

        public GameWorld World { get; set; }
        public Camera Camera { get; set; }
        public Grid MapGrid { get; set; }

        public int GridCellsOnLevel { get => 64; }

        public MapScaler(GameWorld world, Camera camera, Grid mapGrid, float cameraZoomSpeed)
        {
            World = world;
            Camera = camera;

            int worldSize = World.WorldWidth > World.WorldHeight ? World.WorldWidth : World.WorldHeight;

            Camera.farClipPlane = worldSize * 2;
            CameraMinFov = worldSize / 100f;
            CameraMaxFov = worldSize / 4f;
            Camera.fieldOfView = CameraMinFov;

            MaxScaleLevel = Mathf.CeilToInt(worldSize / GridCellsOnLevel);
            //-1 что бы небыло зума на подлокации
            ScaleLevel = MaxScaleLevel - 1;
            ScaleLevelStep = (CameraMaxFov - CameraMinFov) / MaxScaleLevel;
            MapGrid = mapGrid;
            CameraZoomSpeed = cameraZoomSpeed;
        }

        public void Update() 
        {

            if((MaxScaleLevel - ScaleLevel) * ScaleLevelStep + CameraMinFov > Camera.fieldOfView && ScaleLevel != MaxScaleLevel)
            {
                IncreaseScaleLevel();
                Debug.Log($"Upscale: sc lvl: {ScaleLevel}, ({MaxScaleLevel} - {ScaleLevel}) * {ScaleLevelStep} + {CameraMinFov} > {Camera.fieldOfView}");
                return;
            }

            if((MaxScaleLevel - ScaleLevel + 1) * ScaleLevelStep + CameraMinFov < Camera.fieldOfView && ScaleLevel != 1)
            {
                DecreaseScaleLevel();
                Debug.Log($"Downscale: sc lvl: {ScaleLevel}, ({MaxScaleLevel} - {ScaleLevel} + 1) * {ScaleLevelStep} + {CameraMinFov} < {Camera.fieldOfView}");
                return;
            }
        }

        public void ChangeFov(float zoomInput)
        {
            float zoomAmount = zoomInput * CameraZoomSpeed * Time.deltaTime;
            Camera.fieldOfView -= zoomAmount;
            Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView, CameraMinFov, CameraMaxFov);

            Update();
        }

        private void IncreaseScaleLevel()
        {
            if (ScaleLevel >= MaxScaleLevel - 1)
            {
                return;
            }

            ScaleLevel++;
            MapGrid.cellSize = new Vector3(World.WorldWidth * (MaxScaleLevel - ScaleLevel) / 100f, World.WorldHeight * (MaxScaleLevel - ScaleLevel) / 100f);
            
            EventBus.MapEvents.onMapScaleChanged?.Invoke();
        }

        private void DecreaseScaleLevel()
        {
            if (ScaleLevel <= 1)
            {
                return;
            }

            ScaleLevel--;

            MapGrid.cellSize = new Vector3(World.WorldWidth / (MaxScaleLevel - ScaleLevel) / 100f, World.WorldHeight / (MaxScaleLevel - ScaleLevel) / 100f);
            //MapGrid.cellSize = new Vector3(World.WorldWidth / (MaxScaleLevel - ScaleLevel) / 100f, World.WorldHeight / (MaxScaleLevel - ScaleLevel) / 100f);
        
            EventBus.MapEvents.onMapScaleChanged?.Invoke();
        }
    }
}
