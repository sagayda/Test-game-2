
using System;
using WorldGeneration.Core.WaterBehavior;
using UnityEngine;

namespace WorldGeneration.Core.Maps
{
	public class WaterMapPoint
	{
		public float Volume { get; internal set; }
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
