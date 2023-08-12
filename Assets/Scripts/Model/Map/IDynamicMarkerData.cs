using UnityEngine;

namespace Assets.Scripts.Model.Map
{
    public interface IDynamicMarkerData
    {
        public string Name { get; }
        public Sprite Sprite { get; }
    }
}
