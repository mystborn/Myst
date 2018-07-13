using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    public class Trie
    {
        private TrieNode _root = new TrieNode();
        private int _count = 0;

        public void Add(string value)
        {
            var node = _root;
            foreach (var c in value)
            {
                if (!node.Edges.TryGetValue(c, out var temp))
                {
                    temp = new TrieNode();
                    node.Edges.Add(c, temp);
                }
                node = temp;
            }

            node.Value = value;
            _count++;
        }

        public bool Contains(string value)
        {
            var node = _root;
            foreach (var c in value)
                if (!node.Edges.TryGetValue(c, out node))
                    return false;

            return true;
        }

        public void Clear()
        {
            _count = 0;
            _root = new TrieNode();
        }

        public IEnumerable<string> GetValues()
        {
            return _root.GetValues();
        }

        public IEnumerable<string> GetValues(int minLetters)
        {
            return _root.GetValues(minLetters);
        }

        public IEnumerable<string> GetValues(int minLetters, int maxLetters)
        {
            return _root.GetValues(minLetters, maxLetters);
        }
    }
}
