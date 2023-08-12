using UnityEngine;

namespace Assets.Scripts.Model.Map
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
