using System;

namespace Assets.Scripts.Model.InGameScripts.World
{
    public class LocationBarriers
    {
        public bool IsHasBarrier => _barriers != null;

        public Barrier North => IsHasBarrier ? _barriers[0] : null;
        public Barrier East => IsHasBarrier ? _barriers[1] : null;
        public Barrier South => IsHasBarrier ? _barriers[2] : null;
        public Barrier West => IsHasBarrier ? _barriers[3] : null;

        private Barrier[] _barriers;

        public LocationBarriers()
        {

        }

        public void SetBarrier(Direction direction, Barrier barrier)
        {
            if (!IsHasBarrier)
            {
                _barriers = new Barrier[4];
            }

            switch (direction)
            {
                case Direction.North:
                    _barriers[0] = barrier;
                    break;
                case Direction.East:
                    _barriers[1] = barrier;
                    break;
                case Direction.South:
                    _barriers[2] = barrier;
                    break;
                case Direction.West:
                    _barriers[3] = barrier;
                    break;
                default:
                    break;
            }
        }

        public void SetBarrier(Direction[] directions, Barrier[] barriers)
        {
            if (directions == null)
                throw new ArgumentNullException(nameof(directions));

            if (barriers == null)
                throw new ArgumentNullException(nameof(barriers));

            if (barriers.Length != directions.Length)
                throw new ArgumentException("Barriers and directions count must be equal!");

            if (directions.Length > 4)
                throw new ArgumentException("Maximum number of directions: 4!");

            for (int i = 0; i < directions.Length; i++)
            {
                SetBarrier(directions[i], barriers[i]);
            }
        }

        public Barrier GetBarrier(Direction direction)
        {
            return direction switch
            {
                Direction.North => North,
                Direction.South => South,
                Direction.West => West,
                Direction.East => East,
                _ => null,
            };
        }

    }

    public enum Direction
    {
        North,
        East,
        South,
        West,
    }

}
