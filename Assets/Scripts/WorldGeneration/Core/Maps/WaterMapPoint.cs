﻿
using Assets.Scripts.WorldGeneration.Core.WaterBehavior;
using UnityEngine;

namespace Assets.Scripts.WorldGeneration.Core.Maps
{
    public class WaterMapPoint
    {
        public float Volume { get; private set; }
        public Vector2 Stream { get; internal set; }
#nullable enable
        public WaterSource? Source { get; internal set; }
#nullable restore
        public bool IsSource => Source != null;
        public WaterMapPoint(float volume, Vector2 stream)
        {
            Volume = volume;
            Stream = stream;
        }
    }
}