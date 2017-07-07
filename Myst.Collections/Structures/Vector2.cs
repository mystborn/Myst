using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Myst.Structures
{
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Private Fields

        private static readonly Vector2 zeroVector = new Vector2(0f, 0f);
        private static readonly Vector2 unitVector = new Vector2(1f, 1f);
        private static readonly Vector2 unitXVector = new Vector2(1f, 0f);
        private static readonly Vector2 unitYVector = new Vector2(0f, 1f);

        #endregion

        #region Fields

        private float _x;
        private float _y;

        #endregion

        #region Properties

        /// <summary>
        /// The x coordinate of this <see cref="Vector2"/>.
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// The y coordinate of this <see cref="Vector2"/>.
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 0, 0.
        /// </summary>
        public static Vector2 Zero
        {
            get { return zeroVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 1, 1.
        /// </summary>
        public static Vector2 One
        {
            get { return unitVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 1, 0.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return unitXVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 0, 1.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return unitYVector; }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    _x.ToString(), "  ",
                    _y.ToString()
                );
            }
        }

        public override string ToString()
        {
            return "{X:" + _x + " Y:" + _y + "}";
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Vector2(float value)
        {
            _x = value;
            _y = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Inverts values in the specified <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vector2 operator -(Vector2 value)
        {
            value._x = -value._x;
            value._y = -value._y;
            return value;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vector2 operator +(Vector2 value1, Vector2 value2)
        {
            value1._x += value2._x;
            value1._y += value2._y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Vector2"/> from a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vector2 operator -(Vector2 value1, Vector2 value2)
        {
            value1._x -= value2._x;
            value1._y -= value2._y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector2 operator *(Vector2 value1, Vector2 value2)
        {
            value1._x *= value2._x;
            value1._y *= value2._y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            value._x *= scaleFactor;
            value._y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            value._x *= scaleFactor;
            value._y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by the components of another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vector2"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector2 operator /(Vector2 value1, Vector2 value2)
        {
            value1._x /= value2._x;
            value1._y /= value2._y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector2 operator /(Vector2 value1, float divider)
        {
            float factor = 1 / divider;
            value1._x *= factor;
            value1._y *= factor;
            return value1;
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vector2 value1, Vector2 value2)
        {
            return value1._x == value2._x && value1._y == value2._y;
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Vector2"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=(Vector2 value1, Vector2 value2)
        {
            return value1._x != value2._x || value1._y != value2._y;
        }


        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return Equals((Vector2)obj);
            }

            return false;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vector2"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector2"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vector2 other)
        {
            return (_x == other._x) && (_y == other._y);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vector2"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vector2"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_x.GetHashCode() * 397) ^ _y.GetHashCode();
            }
        }

        #endregion

        #region Public Methods

        public Point ToPoint()
        {
            return new Point((int)_x, (int)_y);
        }

        #endregion
    }
}
