using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class EventBus
    {
        public static class WorldEvents
        {
            public static Action onPlayerPositionChanged;
        }

        public static class MapEvents
        {
            public static Action onMapScaleChanged;
        }
    }
}
