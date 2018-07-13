using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myst.Extensions;

namespace Myst.Collections
{
    internal class TrieNode<TKey, TValue>
    {
        public bool IsTerminal { get; set; } = false;
        public TValue Value { get; set; } = default(TValue);
        public Dictionary<TKey, TrieNode<TKey, TValue>> Edges { get; } = new Dictionary<TKey, TrieNode<TKey, TValue>>();

        public IEnumerable<TValue> GetValues()
        {
            if (IsTerminal)
                yield return Value;
            foreach(var node in Edges.Values)
            {
                foreach(var value in node.GetValues())
                {
                    yield return value;
                }
            }
        }

        public IEnumerable<IEnumerable<TKey>> GetKeys()
        {
            var list = new List<TKey>();
            return GetKeys(list);
        }

        private IEnumerable<IEnumerable<TKey>> GetKeys(List<TKey> keys)
        {
            if (IsTerminal)
                yield return keys;
            foreach(var kvp in Edges)
            {
                keys.Add(kvp.Key);
                foreach (var list in kvp.Value.GetKeys(keys))
                    yield return list;
                keys.Pop();
            }
        }

        public IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> Enumerate()
        {
            var list = new List<TKey>();
            return Enumerate(list);
        }

        private IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> Enumerate(List<TKey> keys)
        {
            if (IsTerminal)
                yield return new KeyValuePair<IEnumerable<TKey>, TValue>(keys, Value);
            foreach(var kvp in Edges)
            {
                keys.Add(kvp.Key);
                foreach (var list in kvp.Value.Enumerate(keys))
                    yield return list;
                keys.Pop();
            }
        }
    }

    internal class TrieNode
    {
        public bool IsTerminal => Value != null;
        public string Value { get; set; } = null;
        public Dictionary<char, TrieNode> Edges { get; } = new Dictionary<char, TrieNode>();

        public IEnumerable<string> GetValues()
        {
            return InternalGetValues(1, 1);
        }

        public IEnumerable<string> GetValues(int minLetters)
        {
            return InternalGetValues(1, minLetters);
        }

        private IEnumerable<string> InternalGetValues(int deep, int minLetters)
        {
            if (deep >= minLetters)
                if (IsTerminal)
                    yield return Value;

            foreach (var node in Edges.Values)
                foreach (var value in node.GetValues(deep + 1, minLetters))
                    yield return value;
        }

        public IEnumerable<string> GetValues(int minLetters, int maxLetters)
        {
            return InternalGetValues(1, minLetters, maxLetters);
        }

        private IEnumerable<string> InternalGetValues(int deep, int minLetters, int maxLetters)
        {
            if (deep >= minLetters)
                if (IsTerminal)
                    yield return Value;

            if (deep <= maxLetters)
                foreach (var node in Edges.Values)
                    foreach (var value in node.InternalGetValues(deep + 1, minLetters, maxLetters))
                        yield return value;
        }
    }
}
