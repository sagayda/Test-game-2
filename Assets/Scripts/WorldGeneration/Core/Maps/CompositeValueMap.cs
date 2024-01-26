using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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

        public ValueMapPoint ComputeValues(ValueMapPoint mapPoint, Vector2 position)
        {
            foreach (IValueMap map in _maps)
            {
                map.ComputeValue(mapPoint, position);
            }

            return mapPoint;
        }

        public ValueMapPoint ComputeValues(Vector2 position)
        {
            ValueMapPoint mapPoint = new();

            return ComputeValues(mapPoint, position);
        }

        public ValueMapPoint ComputeValueUpTo(ValueMapPoint mapPoint, Vector2 position, MapValueType lastValueToCompute)
        {
            foreach (IValueMap map in _maps)
            {
                map.ComputeValue(mapPoint, position);

                if (map.ValueType == lastValueToCompute)
                    return mapPoint;
            }

            return mapPoint;
        }

        public ValueMapPoint ComputeValueUpTo(Vector2 position, MapValueType lastValueToCompute)
        {
            ValueMapPoint mapPoint = new();
            return ComputeValueUpTo(mapPoint, position, lastValueToCompute);
        }

        public static ValueMapPoint GetAvarageValue(params ValueMapPoint[] points)
        {
            ValueMapPoint avarageValue = new ();

            foreach (int valueType in Enum.GetValues(typeof(MapValueType)))
            {
                float currentValue = 0;

                int initializedValuesCount = 0;

                foreach (var point in points)
                {
                    if (float.IsNaN(point[valueType]))
                        continue;

                    currentValue += point[valueType];
                    initializedValuesCount++;
                }

                avarageValue[valueType] = currentValue / initializedValuesCount;
            }

            return avarageValue;
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
