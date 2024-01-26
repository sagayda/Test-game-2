using UnityEngine;

namespace Map.Model
{
    public class MarkerModel
    {
        private DynamicMarker _playerMark;

        public MarkerModel(IDynamicMarkerData playerMarkData)
        {
            _playerMark = new DynamicMarker(playerMarkData);
        }
    }
}
