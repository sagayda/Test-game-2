using UnityEngine;

namespace Assets.Scripts.Model
{
    public interface IDynamicMarkerData
    {
        public string Name { get; }
        public Sprite Sprite { get; }
    }
}
