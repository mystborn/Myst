﻿using System;
using System.Collections.Generic;

namespace Myst.Collections
{
    /// <summary>
    /// Binds heap allocated objects to an index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassBinder<T> : IBinder<T> where T : class
    {
        private T[] _binder = new T[10];
        private int _count = 0;
        private readonly Stack<int> _openSlots = new Stack<int>();

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        public T this[int index]
        {
            get => _binder[index];
        }

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Adds an item to the binder, returning its index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index</returns>
        public int Add(T item)
        {
            int index;
            if (_openSlots.Count == 0)
            {
                if (_count == _binder.Length)
                {
                    var temp = new T[_count * 2];
                    Array.Copy(_binder, temp, _count);
                    _binder = temp;
                }
                index = _count;
            }
            else
                index = _openSlots.Pop();

            ++_count;
            _binder[index] = item;
            return index;
        }

        /// <summary>
        /// Determines if the Binder contains an item with the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool Contains(int index)
        {
            return index >= 0 && index < _count && _binder[index] != null;
        }

        /// <summary>
        /// Returns an enumerator used to enumerate through all of the bound items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                if (_binder[i] != null)
                    yield return _binder[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public bool Remove(int index)
        {
            if(_binder[index] != null)
            {
                _binder[index] = null;
                --_count;
                _openSlots.Push(index);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the item at the specified index without any equality checks.
        /// </summary>
        /// <param name="index"></param>
        public void FastRemove(int index)
        {
            _binder[index] = null;
            --_count;
            _openSlots.Push(index);
        }

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool TryGetValue(int index, out T value)
        {
            value = null;
            if (index < 0 || index >= _count)
                return false;
            value = _binder[index];
            if (value == null)
                return false;
            return true;
        }
    }

    /// <summary>
    /// Binds stack allocated objects to an index. Requires a default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StructBinder<T> : IBinder<T> where T : struct
    {
        private T[] _binder = new T[10];
        private readonly Stack<int> _openSlots = new Stack<int>();
        private int _count = 0;

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        public T this[int index] => _binder[index];

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets the default value of this binder.
        /// </summary>
        public T Default { get; }

        /// <summary>
        /// Creates a new StructBinder with default(T) as the default value.
        /// </summary>
        public StructBinder()
        {
            Default = default(T);
        }

        /// <summary>
        /// Creates a new StructBinder with the specified default value.
        /// </summary>
        /// <param name="defaultValue"></param>
        public StructBinder(T defaultValue)
        {
            Default = defaultValue;
        }

        /// <summary>
        /// Adds an item to the binder, returning it's index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index.</returns>
        public int Add(T item)
        {
            int index;
            if (_openSlots.Count == 0)
            {
                if (_count == _binder.Length)
                {
                    var temp = new T[_count * 2];
                    Array.Copy(_binder, temp, _count);
                    _binder = temp;
                }
                index = _count;
            }
            else
                index = _openSlots.Pop();

            _binder[index] = item;
            ++_count;
            return index;
        }

        /// <summary>
        /// Determines if the Binder contains an item with the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool Contains(int index)
        {
            return index >= 0 && index < _count && !_binder[index].Equals(Default);
        }

        /// <summary>
        /// Returns an enumerator used to enumerate through all of the bound items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                if (!_binder[i].Equals(Default))
                    yield return _binder[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        public bool Remove(int index)
        {
            if(!_binder[index].Equals(Default))
            {
                _binder[index] = Default;

                --_count;
                _openSlots.Push(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the item at the specified index without any equality checks.
        /// </summary>
        /// <param name="index"></param>
        public void FastRemove(int index)
        {
            _binder[index] = Default;

            --_count;
            _openSlots.Push(index);
        }

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        public bool TryGetValue(int index, out T value)
        {
            value = Default;
            if (index < 0 || index >= _count)
                return false;
            value = _binder[index];
            if (value.Equals(Default))
                return false;
            return true;
        }
    }
}
