using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    public class Trie<TKey, TValue> : IDictionary<IEnumerable<TKey>, TValue>
    {
        private TrieNode<TKey, TValue> _root = new TrieNode<TKey, TValue>();
        private int _count = 0;

        public int Count => _count;

        public bool IsReadOnly => false;

        public TValue this[IEnumerable<TKey> keys]
        {
            get
            {
                if (TryGetValue(keys, out var value))
                {
                    return value;
                }
                else
                    throw new ArgumentException("There is no value under the given keys.");
            }
            set => Add(keys, value);
        }

        public ICollection<IEnumerable<TKey>> Keys => new List<IEnumerable<TKey>>(_root.GetKeys());

        public ICollection<TValue> Values => new List<TValue>(_root.GetValues());

        public void Add(IEnumerable<TKey> keys, TValue value)
        {
            var node = AddNode(keys);

            if (node.IsTerminal)
                throw new ArgumentException("keys", "There is already a value for keys.");

            node.Value = value;
            node.IsTerminal = true;
            ++_count;
        }

        void ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>.Add(KeyValuePair<IEnumerable<TKey>, TValue> pair)
        {
            Add(pair.Key, pair.Value);
            ++_count;
        }

        public void Clear()
        {
            _root.Edges.Clear();
            _count = 0;
        }

        bool ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>.Contains(KeyValuePair<IEnumerable<TKey>, TValue> item)
        {
            if(TryGetValue(item.Key, out var value))
            {
                if (value.Equals(item.Value))
                    return true;
            }
            return false;
        }

        public bool ContainsKey(IEnumerable<TKey> keys)
        {
            if(TryGetNode(keys, out var node))
            {
                return node.IsTerminal;
            }
            return false;
        }

        void ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>.CopyTo(KeyValuePair<IEnumerable<TKey>, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IEnumerable<TKey> keys)
        {
            if(TryGetNode(keys, out var node))
            {
                if(node.IsTerminal)
                {
                    //Consider cutting trie
                    node.Value = default(TValue);
                    node.IsTerminal = false;
                    --_count;
                    return true;
                }
            }

            return false;
        }
        
        public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
        {
            foreach (var kvp in _root.Enumerate())
                yield return kvp;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<KeyValuePair<IEnumerable<TKey>, TValue>>.Remove(KeyValuePair<IEnumerable<TKey>, TValue> item)
        {
            if(TryGetNode(item.Key, out var node))
            {
                if(node.IsTerminal && node.Value.Equals(item.Value))
                {
                    node.Value = default(TValue);
                    node.IsTerminal = false;
                    --_count;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(IEnumerable<TKey> keys, out TValue value)
        {
            value = default(TValue);
            if(TryGetNode(keys, out var node))
            {
                if(node.IsTerminal)
                {
                    value = node.Value;
                    return true;
                }
            }

            return false;
        }

        private TrieNode<TKey, TValue> AddNode(IEnumerable<TKey> keys)
        {
            var node = _root;
            foreach (var key in keys)
            {
                if (!node.Edges.TryGetValue(key, out var temp))
                {
                    temp = new TrieNode<TKey, TValue>();
                    node.Edges.Add(key, temp);
                }
                node = temp;
            }

            return node;
        }

        private bool TryGetNode(IEnumerable<TKey> keys, out TrieNode<TKey, TValue> node)
        {
            node = _root;
            foreach (var key in keys)
            {
                if (!node.Edges.TryGetValue(key, out node))
                    return false;
            }

            return true;
        }
    }
}
