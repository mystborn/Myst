using System;
using System.Collections.Generic;
using System.Linq;

namespace Myst.Collections
{
    /// <summary>
    /// Collection used to bind instances of <see cref="T"/> to an index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Binder<T>
    {
        #region Fields

        private readonly List<T> _binder;
        private readonly HashSet<int> _openSlots;
        private readonly Dictionary<T, int> _map;

        #endregion

        #region Properties

        public int Count => _binder.Count - _openSlots.Count;

        public IEnumerable<int> Indices => _map.Values;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _binder.Count || _openSlots.Contains(index))
                    throw new ArgumentOutOfRangeException("index", "Tried to get an item at an invalid index.");

                return _binder[index];
            }
            set => ValidateAndAdd(value, index);
        }

        #endregion

        #region CTor

        public Binder()
        {
            _binder = new List<T>();
            _openSlots = new HashSet<int>();
            _map = new Dictionary<T, int>();
        }

        #endregion

        #region Public Api

        public int Add(T item)
        {
            int index;
            if (_openSlots.Count == 0)
            {
                index = _binder.Count;
                _binder.Add(item);
            }
            else
            {
                index = _openSlots.First();
                _openSlots.Remove(index);
                _binder[index] = item;
            }

            _map.Add(item, index);

            return index;
        }

        public void ValidateAndAdd(T item, int index)
        {

            if (index == _binder.Count)
            {
                _binder.Add(item);
            }
            else if (_openSlots.Contains(index))
            {
                _openSlots.Remove(index);
                _binder[index] = item;
            }
            else
                throw new ArgumentException($"Tried to add an item at a filled index.", "index");

            _map.Add(item, index);
        }

        public int GetNextIndex()
        {
            if (_openSlots.Count == 0)
                return _binder.Count;
            else
                return _openSlots.First();
        }

        public bool Contains(T item)
        {
            return _map.ContainsKey(item);
        }

        public bool Contains(int index)
        {
            return index >= 0 && index < _binder.Count && !_openSlots.Contains(index);
        }

        public bool Remove(T item)
        {
            if (_map.TryGetValue(item, out var index))
            {
                _map.Remove(item);
                _openSlots.Add(index);
                _binder[index] = default(T);
                return true;
            }
            return false;
        }

        public bool Remove(int index)
        {
            if (index < 0 || index >= _binder.Count || _openSlots.Contains(index))
                return false;

            var item = _binder[index];
            _map.Remove(item);
            _openSlots.Add(index);
            _binder[index] = default(T);

            return true;
        }

        public bool TryGetValue(int index, out T item)
        {
            item = default(T);
            if (index < 0 || index >= _binder.Count || _openSlots.Contains(index))
                return false;

            item = _binder[index];
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var index in _map.Values)
                yield return _binder[index];
        }

        #endregion
    }
}
