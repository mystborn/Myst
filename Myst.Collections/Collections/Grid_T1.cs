using System;
using System.Collections;
using System.Collections.Generic;

namespace Myst.Collections
{
    public class Grid<T> : ICollection<T>, IEnumerable<T>,  IEnumerable
    {
        #region Fields

        private T[,] _source;
        private IEqualityComparer<T> _comparer;

        #endregion

        #region Properties

        public IEqualityComparer<T> Comparer
        {
            get => _comparer;
        }

        public int Count { get => (int)_source.LongLength; }

        public int Width { get => _source.GetLength(0); }

        public int Height { get => _source.GetLength(1); }

        public bool IsReadOnly { get => false; }

        public T this[int x, int y]
        {
            get { return _source[x, y]; }
            set { Set(x, y, value); }
        }

        #endregion

        #region Constructors

        public Grid(int width, int height) : this(width, height, EqualityComparer<T>.Default) { }

        public Grid(int width, int height, IEqualityComparer<T> comparer)
        {
            _source = new T[width, height];
            _comparer = comparer;
        }

        public Grid(int width, int height, T defaultValue) : this(width, height, defaultValue, EqualityComparer<T>.Default) { }

        public Grid(int width, int height, T defaultValue, IEqualityComparer<T> comparer)
        {
            _source = new T[width, height];
            _comparer = comparer;
            SetRegion(0, 0, width, height, defaultValue);
        }

        #endregion

        #region Grid Specific Methods

        public void Clear(T defaultValue)
        {
            Fill(defaultValue);
        }

        public void CopyTo(Grid<T> destination)
        {
            if (destination.Width < Width)
                throw new IndexOutOfRangeException("Destination grid must have a width >= source grid in order to copy");
            if (destination.Height < Height)
                throw new IndexOutOfRangeException("Destination grid must have a height >= source grid in order to copy");

            for(int h = 0; h < Height; h++)
            {
                Array.Copy(_source, h * Width, destination._source, h * destination.Width, Width);
            }
        }

        public void CopyTo(Grid<T> destination, int destinationX, int destinationY)
        {
            if (destination.Width + destinationX < Width)
                throw new IndexOutOfRangeException("Destination grid must have a width >= source grid in order to copy");
            if (destination.Height + destinationY < Height)
                throw new IndexOutOfRangeException("Destination grid must have a height >= source grid in order to copy");

            int destinationIndex = destinationY * destination.Width + destinationX;

            for(int h = 0; h < Height; h++)
            {
                Array.Copy(_source, h * Width, destination._source, destinationIndex, Width);
                destinationIndex += destination.Width;
            }
        }

        public void CopyTo(Grid<T> destination, int destinationX, int destinationY, int sourceX, int sourceY, int sourceWidth, int sourceHeight)
        {
            if (destination.Width + destinationX < sourceWidth)
                throw new IndexOutOfRangeException();
            if (destination.Height + destinationY < sourceHeight)
                throw new IndexOutOfRangeException();
            if (sourceX < 0 || sourceY < 0 || sourceX + sourceWidth >= Width || sourceY + sourceHeight >= Height)
                throw new ArgumentOutOfRangeException();

            var destinationIndex = destinationY * destination.Width + destinationX;
            var height = sourceY + sourceHeight;
            var sourceIndex = sourceY * Width + sourceX;

            for(int h = sourceY; h < height; h++)
            {
                Array.Copy(_source, sourceIndex, destination._source, destinationIndex, sourceWidth);
                destinationIndex += destination.Width;
                sourceIndex += Width;
            }
        }

        public void Fill(T value)
        {
            SetRegion(0, 0, Width, Height, value);
        }

        public (int x, int y) IndexOf(T item)
        {
            for(int h = 0; h < Height; h++)
            {
                for(int w = 0; w < Width; w++)
                {
                    if (_comparer.Equals(_source[w, h], item))
                        return (w, h);
                }
            }
            return (-1, -1);
        }

        public void Set(int x, int y, T value)
        {
            try
            {
                _source[x, y] = value;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException($"The position ({x}, {y}) is not in the grid.");
            }
        }

        public void SetRegion(int x, int y, int width, int height, T value)
        {
            for (int w = x; w < x + width; w++)
            {
                for (int h = y; h < y + height; h++)
                {
                    Set(w, h, value);
                }
            }
        }

        #endregion

        #region Collection Implementation

        public void Clear()
        {
            Array.Clear(_source, 0, (int)_source.LongLength);
        }

        public bool Contains(T item)
        {
            for(int h = 0; h < Height; h++)
            {
                for(int w = 0; w < Width; w++)
                {
                    if (_comparer.Equals(_source[w, h], item))
                        return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    array[arrayIndex++] = _source[w, h];
                }
            }
        }

        void ICollection<T>.Add(T value)
        {
            _source[0, 0] = value;
        }

        bool ICollection<T>.Remove(T item)
        {
            for (int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    if (_comparer.Equals(_source[w, h], item))
                    {
                        _source[w, h] = default(T);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region IEnumerable Implementation

        public IEnumerator<T> GetEnumerator()
        {
            for(int h = 0; h < Height; h++)
            {
                for(int w = 0; w < Width; w++)
                {
                    yield return _source[w, h];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        #endregion
    }
}
