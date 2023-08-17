using System;
using UnityEngine;

namespace Core
{
    public struct BoundedValue<T> where T : IComparable
    {
        private T _value;
        private T _upperBound;
        private T _lowerBound;

        public BoundedValue(T value, T lowerBound, T upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                throw new ArgumentException("Bounds are invalid");

            _value = value;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            ValidateValue();
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ValidateValue();
            }
        }
        public T UpperBound
        {
            get => _upperBound;
            set => SetUpperBound(value);
        }
        public T LowerBound
        {
            get => _lowerBound;
            set => SetLowerBound(value);
        }
        public bool IsOnMinimum => _value.Equals(_lowerBound);
        public bool IsOnMaximum => _value.Equals(_upperBound);

        public void Maximise()
        {
            Value = UpperBound;
        }

        public void Minimise()
        {
            Value = LowerBound;
        }

        public void ChangeBounds(T lowerBound, T upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                throw new InvalidOperationException("Bounds are invalid");

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();
        }

        public bool TryChangeBounds(T lowerBound, T upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                return false;

            _lowerBound = lowerBound;
            _upperBound = upperBound;
            
            ValidateValue();

            return true;
        }

        public void SetUpperBound(T upperBound)
        {
            if (AreBoundsValid(_lowerBound, upperBound) == false)
                throw new InvalidOperationException("The upper bound is less than the lower");

            _upperBound = upperBound;
            ValidateValue();
        }

        public void SetLowerBound(T lowerBound)
        {
            if (AreBoundsValid(lowerBound, _upperBound) == false)
                throw new InvalidOperationException("The lower bound is greater than the upper");

            _lowerBound = lowerBound;
            ValidateValue();
        }

        private void ValidateValue()
        {
            _value = _value.CompareTo(_lowerBound) < 0 ? _lowerBound : _value.CompareTo(_upperBound) > 0 ? _upperBound : _value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static bool AreBoundsValid(T lowerBound, T upperBound)
        {
            return lowerBound.CompareTo(upperBound) <= 0;
        }
    }

    public struct BoundedVector2
    {
        private Vector2 _value;
        private Vector2 _upperBound;
        private Vector2 _lowerBound;

        public BoundedVector2(Vector2 value, Vector2 lowerBound, Vector2 upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                throw new ArgumentException("Bounds are invalid");

            _value = value;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            ValidateValue();
        }

        public Vector2 Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ValidateValue();
            }
        }
        public Vector2 UpperBound
        {
            get => _upperBound;
            set => SetUpperBound(value);
        }
        public Vector2 LowerBound
        {
            get => _lowerBound;
            set => SetLowerBound(value);
        }
        public bool IsOnMinimum => _value.Equals(_lowerBound);
        public bool IsOnMaximum => _value.Equals(_upperBound);

        public void Maximise()
        {
            Value = UpperBound;
        }

        public void Minimise()
        {
            Value = LowerBound;
        }

        public void ChangeBounds(Vector2 lowerBound, Vector2 upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                throw new InvalidOperationException("Bounds are invalid");

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();
        }

        public bool TryChangeBounds(Vector2 lowerBound, Vector2 upperBound)
        {
            if (AreBoundsValid(lowerBound, upperBound) == false)
                return false;

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();

            return true;
        }

        public void SetUpperBound(Vector2 upperBound)
        {
            if (AreBoundsValid(_lowerBound, upperBound) == false)
                throw new InvalidOperationException("The upper bound is less than the lower");

            _upperBound = upperBound;
            ValidateValue();
        }

        public void SetLowerBound(Vector2 lowerBound)
        {
            if (AreBoundsValid(lowerBound, _upperBound) == false)
                throw new InvalidOperationException("The lower bound is greater than the upper");

            _lowerBound = lowerBound;
            ValidateValue();
        }

        private void ValidateValue()
        {
            _value.x = Mathf.Clamp(_value.x, _lowerBound.x, _upperBound.x);
            _value.y = Mathf.Clamp(_value.y, _lowerBound.y, _upperBound.y);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static bool AreBoundsValid(Vector2 lowerBound, Vector2 upperBound)
        {
            return lowerBound.x <= upperBound.x
                && lowerBound.y <= upperBound.y;
        }
    }

    public struct BoundedVector3
    {
        private Vector3 _value;
        private Vector3 _upperBound;
        private Vector3 _lowerBound;

        public BoundedVector3(Vector3 value, Vector3 lowerBound, Vector3 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                throw new ArgumentException("Bounds are invalid");

            _value = value;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            ValidateValue();
        }

        public Vector3 Value
        {
            get => _value;
            set
            {
                _value = value;
                ValidateValue();
            }
        }

        public Vector3 UpperBound
        {
            get => _upperBound;
            set => SetUpperBound(value);
        }

        public Vector3 LowerBound
        {
            get => _lowerBound;
            set => SetLowerBound(value);
        }

        public bool IsOnMinimum => _value.Equals(_lowerBound);
        public bool IsOnMaximum => _value.Equals(_upperBound);

        public void Maximise()
        {
            Value = UpperBound;
        }

        public void Minimise()
        {
            Value = LowerBound;
        }

        public void ChangeBounds(Vector3 lowerBound, Vector3 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                throw new InvalidOperationException("Bounds are invalid");

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();
        }

        public bool TryChangeBounds(Vector3 lowerBound, Vector3 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                return false;

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();

            return true;
        }

        public void SetUpperBound(Vector3 upperBound)
        {
            if (!AreBoundsValid(_lowerBound, upperBound))
                throw new InvalidOperationException("The upper bound is less than the lower");

            _upperBound = upperBound;
            ValidateValue();
        }

        public void SetLowerBound(Vector3 lowerBound)
        {
            if (!AreBoundsValid(lowerBound, _upperBound))
                throw new InvalidOperationException("The lower bound is greater than the upper");

            _lowerBound = lowerBound;
            ValidateValue();
        }

        private void ValidateValue()
        {
            _value.x = Mathf.Clamp(_value.x, _lowerBound.x, _upperBound.x);
            _value.y = Mathf.Clamp(_value.y, _lowerBound.y, _upperBound.y);
            _value.z = Mathf.Clamp(_value.z, _lowerBound.z, _upperBound.z);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static bool AreBoundsValid(Vector3 lowerBound, Vector3 upperBound)
        {
            return lowerBound.x <= upperBound.x
                && lowerBound.y <= upperBound.y
                && lowerBound.z <= upperBound.z;
        }
    }

    public struct BoundedVector4
    {
        private Vector4 _value;
        private Vector4 _upperBound;
        private Vector4 _lowerBound;

        public BoundedVector4(Vector4 value, Vector4 lowerBound, Vector4 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                throw new ArgumentException("Bounds are invalid");

            _value = value;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            ValidateValue();
        }

        public Vector4 Value
        {
            get => _value;
            set
            {
                _value = value;
                ValidateValue();
            }
        }

        public Vector4 UpperBound
        {
            get => _upperBound;
            set => SetUpperBound(value);
        }

        public Vector4 LowerBound
        {
            get => _lowerBound;
            set => SetLowerBound(value);
        }

        public bool IsOnMinimum => _value.Equals(_lowerBound);
        public bool IsOnMaximum => _value.Equals(_upperBound);

        public void Maximise()
        {
            Value = UpperBound;
        }

        public void Minimise()
        {
            Value = LowerBound;
        }

        public void ChangeBounds(Vector4 lowerBound, Vector4 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                throw new InvalidOperationException("Bounds are invalid");

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();
        }

        public bool TryChangeBounds(Vector4 lowerBound, Vector4 upperBound)
        {
            if (!AreBoundsValid(lowerBound, upperBound))
                return false;

            _lowerBound = lowerBound;
            _upperBound = upperBound;

            ValidateValue();

            return true;
        }

        public void SetUpperBound(Vector4 upperBound)
        {
            if (!AreBoundsValid(_lowerBound, upperBound))
                throw new InvalidOperationException("The upper bound is less than the lower");

            _upperBound = upperBound;
            ValidateValue();
        }

        public void SetLowerBound(Vector4 lowerBound)
        {
            if (!AreBoundsValid(lowerBound, _upperBound))
                throw new InvalidOperationException("The lower bound is greater than the upper");

            _lowerBound = lowerBound;
            ValidateValue();
        }

        private void ValidateValue()
        {
            _value.x = Mathf.Clamp(_value.x, _lowerBound.x, _upperBound.x);
            _value.y = Mathf.Clamp(_value.y, _lowerBound.y, _upperBound.y);
            _value.z = Mathf.Clamp(_value.z, _lowerBound.z, _upperBound.z);
            _value.w = Mathf.Clamp(_value.w, _lowerBound.w, _upperBound.w);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private static bool AreBoundsValid(Vector4 lowerBound, Vector4 upperBound)
        {
            return lowerBound.x <= upperBound.x
                && lowerBound.y <= upperBound.y
                && lowerBound.z <= upperBound.z
                && lowerBound.w <= upperBound.w;
        }
    }
}
