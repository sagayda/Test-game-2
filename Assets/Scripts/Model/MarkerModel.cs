using UnityEngine;

namespace Assets.Scripts.Model
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
