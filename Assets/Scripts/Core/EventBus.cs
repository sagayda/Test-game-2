﻿using System;

namespace Core
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