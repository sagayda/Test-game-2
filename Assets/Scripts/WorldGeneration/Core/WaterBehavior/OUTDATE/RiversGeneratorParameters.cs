using WorldGeneration.Core;

namespace WorldGeneration.Core.Outdate
{
    public class RiversGeneratorParameters
    {
        private readonly WorldGenerator _worldGenerator;
        private readonly int _seed;
        private readonly int _riversLength;
        private readonly float _maximasButtom;
        private readonly float _minimasTop;
        private readonly float _radius;

        public RiversGeneratorParameters(WorldGenerator worldGenerator, int seed, int riversLength, float maximasButtom, float minimasTop, float radius)
        {
            _worldGenerator = worldGenerator;
            _riversLength = riversLength;
            _maximasButtom = maximasButtom;
            _minimasTop = minimasTop;
            _radius = radius;
        }

        public WorldGenerator WorldGenerator => _worldGenerator;
        public int Seed => _seed;
        public int RiversLength => _riversLength;
        public float MaximasButtom => _maximasButtom;
        public float MinimasTop => _minimasTop; 
        public float Radius => _radius;
        public uint WorldWidth => (uint)_worldGenerator.Width;
        public uint WorldHeight => (uint)_worldGenerator.Height;
    }
}
