using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldGeneration.Core.Maps;

namespace WorldGeneration.Core
{
    public class CompositeValueMap
    {
        private List<IValueMap> _maps;

        public CompositeValueMap(params IValueMap[] maps)
        {
            if (maps == null || maps.Length <= 0)
                throw new ArgumentException("Invalid maps list!", nameof(maps));

            _maps = maps.OrderByDescending((elem) => elem.Priority).ToList();
        }

        public CompositeValueMap(IValueMap map)
        {
            if (map == null)
                throw new ArgumentException("Invalid maps list!", nameof(map));

            _maps = new() { map };
        }

        public void SetSeed(int seed)
        {
            foreach (var kvp in _maps)
            {
                kvp.Seed = seed;
            }
        }

        public ValueMapPoint ComputeValue(ValueMapPoint mapPoint)
        {
            foreach (IValueMap map in _maps)
            {
                map.ComputeValue(mapPoint);
            }

            return mapPoint;
        }

        public ValueMapPoint ComputeValue(Vector2 position)
        {
            ValueMapPoint mapPoint = new(position);

            return ComputeValue(mapPoint);
        }

        public ValueMapPoint ComputeValue(float x, float y)
        {
            ValueMapPoint mapPoint = new(x, y);

            return ComputeValue(mapPoint);
        }

        public ValueMapPoint ComputeValueUpTo(ValueMapPoint mapPoint, MapValueType lastValueToCompute)
        {
            foreach (IValueMap map in _maps)
            {
                map.ComputeValue(mapPoint);

                if (map.ValueType == lastValueToCompute)
                    return mapPoint;
            }

            return mapPoint;
        }

        public ValueMapPoint ComputeValueUpTo(Vector2 position, MapValueType lastValueToCompute)
        {
            ValueMapPoint mapPoint = new(position);
            return ComputeValueUpTo(mapPoint, lastValueToCompute);
        }

        public ValueMapPoint ComputeValueUpTo(float x, float y, MapValueType lastValueToCompute)
        {
            ValueMapPoint mapPoint = new(x, y);
            return ComputeValueUpTo(mapPoint, lastValueToCompute);
        }

        public static CompositeValueMap CreateDefault(int seed)
        {
            List<IValueMap> maps = new List<IValueMap>
            {
                new HeightsValueMap(ParametersSave.LoadParametersOrDefault<HeightsMapParameters>(ParametersSave.SaveSlot.Default)),
                new TemperatureValueMap(ParametersSave.LoadParametersOrDefault<TemperatureMapParameters>(ParametersSave.SaveSlot.Default)),
                new ProgressValueMap(ParametersSave.LoadParametersOrDefault<ProgressMapParameters>(ParametersSave.SaveSlot.Default)),
                new PolutionValueMap(ParametersSave.LoadParametersOrDefault<PolutionMapParameters>(ParametersSave.SaveSlot.Default)),
            };

            var composite = new CompositeValueMap(maps.ToArray());
            composite.SetSeed(seed);

            return composite;
        }
    }
}
