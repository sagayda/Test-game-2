using System.Numerics;

namespace Assets.Scripts.Model.Map
{
    public class DynamicMarker
    {
        private readonly IDynamicMarkerData _data;
        private Vector2 _position;
        private Vector2 _size;

        public DynamicMarker(IDynamicMarkerData markerData)
        {
            _data = markerData;
            _position = new Vector2(0, 0);
            _size = new Vector2(1, 1);
        }
    }
}
