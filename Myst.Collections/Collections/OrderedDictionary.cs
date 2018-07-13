using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Myst.Collections
{
    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
    {
        #region Fields

        private Dictionary<TKey, TValue> _source;
        private List<TKey> _index;

        #endregion

        #region Properties

        public TValue this[TKey key]
        {
            get => _source[key];
            set => _source[key] = value;
        }

        public TValue this[int key]
        {
            get => _source[_index[key]];
            set => _source[_index[key]] = value;
        }

        public ICollection<TKey> Keys => _source.Keys;

        public ICollection<TValue> Values => _source.Values;

        public int Count => _source.Count;

        public bool IsReadOnly => false;

        #endregion

        #region Initialization

        public OrderedDictionary()
        {
            _source = new Dictionary<TKey, TValue>();
            _index = new List<TKey>();
        }

        public OrderedDictionary(int capacity)
        {
            _source = new Dictionary<TKey, TValue>(capacity);
            _index = new List<TKey>(capacity);
        }

        public OrderedDictionary(IEqualityComparer<TKey> comparer)
        {
            _source = new Dictionary<TKey, TValue>(comparer);
            _index = new List<TKey>();
        }

        public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _source = new Dictionary<TKey, TValue>(capacity, comparer);
            _index = new List<TKey>(capacity);
        }

        #endregion

        #region Public Api

        public void Add(TKey key, TValue value)
        {
            _source.Add(key, value);
            _index.Add(key);
        }

        public void Clear()
        {
            _source.Clear();
            _index.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _source.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(int index, TKey key, TValue value)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("index");
            else if (index == Count)
                Add(key, value);
            else
            {
                _index.Insert(index, key);
                _source.Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            if (_source.ContainsKey(key))
            {
                _source.Remove(key);
                _index.Remove(key);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("index");
            _source.Remove(_index[index]);
            _index.RemoveAt(index);
        }

        public bool TryGetIndex(int index, out TValue value)
        {
            value = default(TValue);
            if (index < 0 || index >= Count)
                return false;

            value = _source[_index[index]];
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _source.TryGetValue(key, out value);
        }

        #endregion

        #region Not Implemented

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
