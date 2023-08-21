using System;
using UnityEngine;

namespace UniversalTools
{
    public static class EventBus
    {
        public static class WorldEvents
        {
            public static Action GameWorldLoaded;
            public static Action onPlayerPositionChanged;
        }

        public static class MapEvents
        {
            public static Action onMapScaleChanged;
        }

    }
}
