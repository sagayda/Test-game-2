using System.Collections.Generic;
using Assets.Scripts.WorldGeneration.Core.Chunks;
using UnityEngine;
using WorldGeneration.Core;
using WorldGeneration.Core.WaterBehavior;

namespace Assets.Scripts.WorldGeneration.Core.WaterBehavior
{
    public class WaterBehaviour
    {
        private float _oceanLevel;
        private List<River> _rivers;

        public WaterBehaviour(float oceanLevel)
        {
            _rivers = new List<River>();
            _oceanLevel = oceanLevel;
        }

        public float OceanLevel => _oceanLevel;

        public void CreateSource(Chunk chunk, float strength)
        {
            if (chunk.IsWaterChunk)
            {
                if (chunk.Water.IsSource)
                    return;

                chunk.Water.Source = new(strength);
                return;
            }

            chunk.Water = new(0, new(0, 0))
            {
                Source = new(strength)
            };
        }

        public void CreateRiver(Chunk sourceChunk)
        {
            _rivers.Add(new(sourceChunk));
        }

        private void IterateRiver(River river)
        {
            if (river.HasLeakage)
            {
                Debug.Log("Try to iterate river with leakege");
                return;
            }


        }

        public void CreateOcean(World world)
        {
            List<IMapArea> oceanChunks = new();

            foreach (var chunk in world.Chunks)
                if (chunk.Value.Values[MapValueType.Height] <= world.OceanLevel)
                    oceanChunks.Add(chunk.Value);

            world.TrySetOcean(new(oceanChunks));
        }

    }
}
