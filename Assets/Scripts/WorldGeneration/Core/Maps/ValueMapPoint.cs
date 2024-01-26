using System;
using UnityEngine;

namespace WorldGeneration.Core
{
    //public class MapPoint
    //{
    //    private float _height;
    //    private float _temperature;
    //    private float _progress;
    //    private float _polution;

    //    public MapPoint(Vector2 position)
    //    {
    //        Position = position;
    //    }

    //    public readonly Vector2 Position;

    //    public bool IsHeightComputed { get; private set; } = false;
    //    public float Height => IsHeightComputed ? _height : throw new InvalidOperationException("Height is not computed");
    //    public bool IsTemperatureComputed { get; private set; } = false;
    //    public float Temperature => IsTemperatureComputed ? _temperature : throw new InvalidOperationException("Temperature is not computed");
    //    public bool IsProgressComputed { get; private set; } = false;
    //    public float Progress => IsProgressComputed ? _progress : throw new InvalidOperationException("Progress is not computed");
    //    public bool IsPolutionComputed { get; private set; } = false;
    //    public float Polution => IsPolutionComputed ? _polution : throw new InvalidOperationException("Polution is not computed");

    //    public MapPoint SetHeight(float height)
    //    {
    //        _height = height;
    //        IsHeightComputed = true;
    //        return this;
    //    }

    //    public MapPoint SetTemperature(float temperature)
    //    {
    //        _temperature = temperature;
    //        IsTemperatureComputed = true;
    //        return this;
    //    }

    //    public MapPoint SetProgress(float progress)
    //    {
    //        _progress = progress;
    //        IsProgressComputed = true;
    //        return this;
    //    }

    //    public MapPoint SetPolution(float polution)
    //    {
    //        _polution = polution;
    //        IsPolutionComputed = true;
    //        return this;
    //    }
    //}

    public class ValueMapPoint
    {
        private static readonly int MAPS_COUNT = Enum.GetValues(typeof(MapValueType)).Length;
        private readonly float[] _values;

        public float this[MapValueType type]
        {
            get
            {
                return _values[(int)type];

                //return float.IsNaN(value) ? throw new InvalidOperationException($"Selected type {type} is not set!") : value;
            }

            set
            {
                _values[(int)type] = value;
            }
        }

        public float this[int mapValueTypeCode]
        {
            get
            {
                if (Enum.IsDefined(typeof(MapValueType), mapValueTypeCode) == false)
                    throw new ArgumentException($"MapValueType with code {mapValueTypeCode} does not exist.", nameof(mapValueTypeCode));

                return _values[mapValueTypeCode];
            }

            set
            {
                if (Enum.IsDefined(typeof(MapValueType), mapValueTypeCode) == false)
                    throw new ArgumentException($"MapValueType with code {mapValueTypeCode} does not exist.", nameof(mapValueTypeCode));

                _values[mapValueTypeCode] = value;
            }
        }

        public ValueMapPoint()
        {
            _values = new float[MAPS_COUNT];

            InitializeValues();
        }

        private void InitializeValues()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = float.NaN;
            }
        }

        public ValueMapPoint SetValue(MapValueType type, float value)
        {
            _values[(int)type] = value;
            return this;
        }
    }

    public enum MapValueType
    {
        Height = 0,
        Temperature = 1,
        Progress = 2,
        Polution = 3,
    }

}
