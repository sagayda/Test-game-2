using System.Collections.Generic;
using UnityEngine;
using UniversalTools;

namespace WorldGeneration.Core.Outdate
{
    public class WaterBehaviorData
    {
        public Vector2 StartPosition { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Inertia { get; set; }
        public float Velocity { get; set; }
        public float ImpulseModule { get; set; }
        public BoundedValue<float> Weight { get; set; }
        public List<WaterBehaviorSegment> Segments { get; set; } = new();

        public WaterBehaviorData(Vector2 startPosition, BoundedValue<float> weight)
        {
            StartPosition = startPosition;
            Position = startPosition;
            Direction = Vector2.zero;
            Inertia = Vector2.zero;
            Weight = weight;
            Velocity = 50f;
        }

        public void Step(Vector2 surfaceNormal)
        {
            Segments.Add(new(Position, Direction, Inertia, Weight.Value));

            float x = Position.x + surfaceNormal.x;
            float y = Position.y + surfaceNormal.y;
            Position = new(x, y);


            //var resultImpulseDirection = (Direction * ImpulseModule) + (forceDirection * forceModulus);
            //resultImpulseDirection = resultImpulseDirection.normalized;

            //var resultImpulse = ImpulseModule + forceModulus;

            //var deltaImpulseVector = (resultImpulseDirection * resultImpulse) - (Direction * ImpulseModule);

            //ImpulseModule = resultImpulse;

            //Direction += deltaImpulseVector;
            //Direction = Direction.normalized;

            //Position += Direction * ImpulseModule;

            ////////

            ////Debug.Log(direction);

            //Direction = Direction * 0.7f + direction;

            //Vector2 oldInertia = Inertia;

            //Inertia += direction.normalized;

            //float inertiaMagnitudeChange = Inertia.magnitude - oldInertia.magnitude;
            //InertiaMagnitude += inertiaMagnitudeChange;

            //Inertia = Inertia.normalized;

            //Position += Direction.normalized;
        }
    }

    public struct WaterBehaviorSegment
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Inertia { get; set; }
        public float Weight { get; set; }

        public WaterBehaviorSegment(Vector2 position, Vector2 direction, Vector2 inertia, float weight)
        {
            Position = position;
            Direction = direction;
            Inertia = inertia;
            Weight = weight;
        }
    }
}
