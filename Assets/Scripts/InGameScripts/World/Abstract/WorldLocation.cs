using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InGameScripts.World.Absctract
{
    [Serializable]
    public abstract class WorldLocation
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Color Color { get; }

        public int X { get; }
        public int Y { get; }

        //testing
        public float Noise { get; set; }

        public WorldSublocation Sublocation { get; protected set; }

        public List<WorldLocationConnector> Connectors { get; protected set; } = new List<WorldLocationConnector>();

        public int NeighboursCount => Connectors.Count;

        public WorldLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public WorldLocation(int x, int y, WorldSublocation sublocation)
        {
            if (sublocation == null) throw new ArgumentNullException(nameof(sublocation));

            Sublocation = sublocation;

            X = x;
            Y = y;
        }

        public WorldLocation(int x, int y, WorldSublocation sublocation, List<WorldLocationConnector> connectors)
        {
            if (sublocation == null)
                throw new ArgumentNullException(nameof(sublocation));

            if (connectors == null)
                throw new ArgumentNullException(nameof(connectors));

            X = x;
            Y = y;
            Connectors = connectors;
            Sublocation = sublocation;
        }

        public bool IsNeighbour(WorldLocation location)
        {
            foreach (var connector in Connectors)
            {
                if (connector.ToLocation == location)
                    return true;
            }

            return false;
        }

        public void SetConnectors(List<WorldLocationConnector> connectors)
        {
            if (connectors == null)
                throw new ArgumentNullException(nameof(connectors));

            Connectors = connectors;
        }

        public bool TryConnect(WorldLocation location, WorldLocationConnector connector)
        {
            if (location == null || connector == null)
                return false;

            if (Math.Abs(X - location.X) > 1 || Math.Abs(Y - location.Y) > 1)
                return false;

            connector.Connect(this, location);
            Connectors.Add(connector);
            return true;
        }

    }
}
