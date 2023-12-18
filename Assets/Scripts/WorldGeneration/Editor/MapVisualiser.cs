using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.WorldGeneration.Editor
{
    public class MapVisualiser
    {
        private int _width;
        private int _height;
        private int _pixelSize;

        private Texture2D _map;
        private Texture2D _overpaintedMap;

        public MapVisualiser(int width, int height, int pixelSize = 1)
        {
            _width = width;
            _height = height;
            _pixelSize = pixelSize;
        }


        public Sprite Paint(ValueDelegate valueDelegate, ColorMap colorMap)
        {
            Texture2D texture = new(_width * _pixelSize, _height * _pixelSize);

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float value = valueDelegate(x, y);

                    Color color = colorMap.GetColor(value);

                    PaintPixel(x, y, color, texture);

                    //texture.SetPixel(x, y, color);

                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, _width * _pixelSize, _height * _pixelSize), new Vector2(0, 0));
            _map = texture;
            _overpaintedMap = texture;
            return sprite;
        }

        public Sprite Overpaint(BoolDelegate boolDelegate, Color color)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (boolDelegate(x, y) == false)
                        continue;

                    PaintPixel(x, y, color, _overpaintedMap);

                }
            }


            _overpaintedMap.Apply();

            Sprite sprite = Sprite.Create(_overpaintedMap, new Rect(0, 0, _width * _pixelSize, _height * _pixelSize), new Vector2(0, 0));

            return sprite;
        }

        public Sprite Overpaint(Vector2Int[] pixels, Color color)
        {
            foreach (var pixel in pixels)
            {
                PaintPixel(pixel.x, pixel.y, color, _overpaintedMap);
            }

            _overpaintedMap.Apply();

            Sprite sprite = Sprite.Create(_overpaintedMap, new Rect(0, 0, _width * _pixelSize, _height * _pixelSize), new Vector2(0, 0));

            return sprite;
        }

        public void ClearOverpaint()
        {
            _overpaintedMap = new(_map.width, _map.height);
            _overpaintedMap.filterMode = FilterMode.Point;
            _overpaintedMap.LoadRawTextureData(_map.GetRawTextureData());
        }

        private void PaintPixel(int x, int y, Color color, Texture2D texture)
        {
            if(_pixelSize == 1)
            {
                texture.SetPixel(x, y, color);
                return;
            }

            for (int i = x * _pixelSize; i < (x+1) * _pixelSize; i++)
            {
                for (int j = y * _pixelSize; j < (y+1) * _pixelSize; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
        }

        public delegate float ValueDelegate(int x, int y);
        public delegate bool BoolDelegate(int x, int y);
    }

    public class ColorMap
    {
        private List<Color> _colors;
        private List<float> _maximas;

        public ColorMap(List<float> maximas, List<Color> colors)
        {
            _maximas = maximas;
            _colors = colors;
        }

        public Color GetRawColor(float maxima, bool nearestLarger, out float realMaxima)
        {
            if(nearestLarger)
            {
                for (int i = 0; i < _maximas.Count; i++)
                {
                    if (maxima < _maximas[i])
                    {
                        realMaxima = _maximas[i];
                        return _colors[i];
                    }
                }

                realMaxima = _maximas.Last();
                return _colors.Last();
            }
            else
            {
                for (int i = _maximas.Count - 1; i >= 0 ; i--)
                {
                    if (maxima >= _maximas[i])
                    {
                        realMaxima = _maximas[i];
                        return _colors[i];
                    }
                }

                realMaxima = _maximas.First();
                return _colors.First();
            }

        }

        public Color GetColor(float maxima)
        {
            float smallerMaxima;
            float largerMaxima;

            Color smallerColor = GetRawColor(maxima, false, out smallerMaxima);
            Color largerColor = GetRawColor(maxima, true, out largerMaxima);

            float lerpRate = (maxima - smallerMaxima) / (largerMaxima - smallerMaxima);

            return Color.Lerp(smallerColor, largerColor, lerpRate);
        }
    }
}
