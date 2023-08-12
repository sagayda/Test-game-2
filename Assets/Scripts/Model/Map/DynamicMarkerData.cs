using UnityEngine;

namespace Assets.Scripts.Model.Map
{
    [CreateAssetMenu(fileName = "DynamicMarkerData", menuName = "Map/Markers/Create dynamic marker data")]
    public class DynamicMarkerData : ScriptableObject, IDynamicMarkerData
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;

        public string Name => _name;
        public Sprite Sprite => _sprite;
    }
}
