using System.Collections.Generic;
using UnityEngine;
using WorldGeneration.Core.Outdate;

namespace WorldGeneration.Editor
{
    public class FloodTestVisualizer : MonoBehaviour
    {
        private List<Transform> heightVoxels = new List<Transform>();

        public FloodTest FloodTest;

        public GameObject VoxelPrefab;

        public SpriteRenderer HeightRenderer;
        public SpriteRenderer WaterRenderer;
        public SpriteRenderer VisitedRenderer;

        public bool RepaintHeight = false;
        public bool RepaintWater = false;
        public bool RepaintVisited = false;
        public bool CreateHeightMesh = false;

        private void Update()
        {
            if (RepaintHeight)
                PaintHeight();

            if (RepaintWater)
                PaintWater();

            if (RepaintVisited)
                PaintVisited();

            if (CreateHeightMesh)
            {
                CreateHeightVoxels();
                CreateHeightMesh = false;
            }
        }

        private void CreateHeightVoxels()
        {
            for (int x = 0; x < FloodTest.heightsMap.GetLength(0); x++)
            {
                for (int y = 0; y < FloodTest.heightsMap.GetLength(1); y++)
                {
                    var voxel = Instantiate(VoxelPrefab);
                    voxel.transform.position = new Vector3(x, 0, y);
                    voxel.transform.localScale = new(1, FloodTest.heightsMap[x,y], 1);
                }
            }
        }

        private void PaintHeight()
        {
            Texture2D texture = new(FloodTest.heightsMap.GetLength(0), FloodTest.heightsMap.GetLength(1));
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < FloodTest.heightsMap.GetLength(0); x++)
            {
                for (int y = 0; y < FloodTest.heightsMap.GetLength(1); y++)
                {
                    Color color = Color.Lerp(Color.white, Color.black, FloodTest.heightsMap[x, y] / 100f);

                    //if (FloodTest.heightsMap[x, y] == 0)
                    //    color = Color.white;
                    //else
                    //    color = Color.black;

                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, FloodTest.heightsMap.GetLength(0), FloodTest.heightsMap.GetLength(1)), new Vector2(0, 0));

            HeightRenderer.sprite = sprite;
        }

        private void PaintWater()
        {
            Texture2D texture = new(FloodTest.waterMap.GetLength(0), FloodTest.waterMap.GetLength(1));
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < FloodTest.waterMap.GetLength(0); x++)
            {
                for (int y = 0; y < FloodTest.waterMap.GetLength(1); y++)
                {
                    Color color = Color.Lerp(new Color(0, 0, 1, 0), new Color(0, 0, 1, 1), FloodTest.waterMap[x, y] / 100f);

                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, FloodTest.waterMap.GetLength(0), FloodTest.waterMap.GetLength(1)), new Vector2(0, 0));

            WaterRenderer.sprite = sprite;
        }

        private void PaintVisited()
        {
            Texture2D texture = new(FloodTest.lastVisited.GetLength(0), FloodTest.lastVisited.GetLength(1));
            texture.filterMode = FilterMode.Point;

            for (int x = 0; x < FloodTest.lastVisited.GetLength(0); x++)
            {
                for (int y = 0; y < FloodTest.lastVisited.GetLength(1); y++)
                {
                    Color color;

                    if (FloodTest.lastVisited[x, y])
                        color = new Color(0, 1, 0, 0.5f);
                    else
                        color = new Color(0, 0, 0, 0);

                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, FloodTest.lastVisited.GetLength(0), FloodTest.lastVisited.GetLength(1)), new Vector2(0, 0));

            VisitedRenderer.sprite = sprite;
        }
    }
}
