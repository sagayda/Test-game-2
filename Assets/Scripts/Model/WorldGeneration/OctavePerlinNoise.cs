﻿using UnityEngine;

namespace Assets.Scripts.Model.WorldGeneration
{
    public static class OctavePerlinNoise
    {
        public static float OctaveNoise(float x, float y, OctaveNoiseParameters parameters)
        {
            x *= parameters.Zoom;
            y *= parameters.Zoom;

            float amplitude = 1f;
            float frequency = 1f;

            float resultNoise = 0f;
            float amplitudeSum = 0f;
            
            for (int octave = 0; octave < parameters.Octaves; octave++)
            {
                float currentOctaveNoise = Mathf.PerlinNoise((parameters.SeedX + parameters.SeedStep + x) * frequency,
                    (parameters.SeedY + parameters.SeedStep + y) * frequency) * amplitude;

                resultNoise += currentOctaveNoise;

                amplitudeSum += amplitude;

                amplitude *= parameters.Persistance;
                frequency *= parameters.Lacunarity;
            }

            //back to 0...1
            resultNoise /= amplitudeSum;

            return parameters.EasingStrategy.Ease(resultNoise);
        }
    }
}
