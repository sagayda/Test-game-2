﻿using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class MapView : MonoBehaviour
    {
        public Action<float> Zoom;
        public Action<Vector2> MoveInDirection;
        public Action<Vector2> TranslateToPosition;

        [SerializeField] private Camera _mapCamera;
        [SerializeField] private Canvas _mapCanvas;
        [SerializeField] private Grid _mapGrid;

        [SerializeField] private GameObject _mainGameObject;

        private MapSpritesWrapper _sprites;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                MoveInDirection.Invoke(new Vector2(horizontal, vertical));
            }

            float zoom = Input.GetAxis("Mouse ScrollWheel");

            if (zoom != 0)
            {
                Vector3 mouseScreenCords = Input.mousePosition;
                mouseScreenCords.z = 100;
                Vector3 mouseWorldCords = _mapCamera.ScreenToWorldPoint(mouseScreenCords);

                Zoom.Invoke(zoom);

                mouseScreenCords = Input.mousePosition;
                mouseScreenCords.z = 100;
                Vector3 mouseWorldCordsAfterZoom = _mapCamera.ScreenToWorldPoint(mouseScreenCords);

                Vector3 delta = mouseWorldCords - mouseWorldCordsAfterZoom;

                Vector3 cameraPosition = new()
                {
                    x = _mapCamera.transform.position.x + delta.x,
                    y = _mapCamera.transform.position.y + delta.y,
                    z = _mapCamera.transform.position.z,
                };

                TranslateToPosition.Invoke(cameraPosition);
            }
        }

        public void Init(MapScaling scaling)
        {
            _sprites = new MapSpritesWrapper(_mapCanvas.transform, scaling.MaxScaleLevel);

            for (int scaleLevel = 1; scaleLevel <= scaling.MaxScaleLevel; scaleLevel++)
            {
                _sprites.AddSprite(scaling.CreateMapTexture(scaleLevel), scaleLevel);
            }
        }

        public void UpdatePosition(Vector2 position)
        {
            Vector3 cameraPosition = new(position.x, position.y, _mapCamera.transform.position.z);
            _mapCamera.transform.position = cameraPosition;
        }

        public void UpdateFov(float fov)
        {
            _mapCamera.fieldOfView = fov;
        }

        public void UpdateScale(int scaleLevel)
        {
            foreach (var sprite in _sprites)
                sprite.Disable();

            _sprites[scaleLevel].Enable();
        }

        public void UpdateGridCellSize(Vector2 gridCellSize)
        {
            _mapGrid.cellSize = gridCellSize;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            _mainGameObject.SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _mainGameObject.SetActive(true);
        }
    }
}
