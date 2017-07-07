using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    internal class FibonacciNode<T>
    {
        private T _value;
        private FibonacciNode<T> _left;
        private FibonacciNode<T> _right;
        private FibonacciNode<T> _parent = null;
        private FibonacciNode<T> _child = null;
        private int _degree = 0;

        public T Value { get => _value; set => _value = value; }
        public int Degree { get => _degree; set => _degree = value; }
        internal FibonacciNode<T> Left { get => _left; set => _left = value; }
        internal FibonacciNode<T> Right { get => _right; set => _right = value; }
        internal FibonacciNode<T> Parent { get => _parent; set => _parent = value; }
        internal FibonacciNode<T> Child { get => _child; set => _child = value; }

        public FibonacciNode(T value)
        {
            _value = value;
            _left = this;
            _right = this;
        }
    }

    internal class FibonacciNode<TValue, TKey>
    {
        private int _degree = 0;
        private TValue _value;
        private TKey _key;
        private FibonacciNode<TValue, TKey> _left;
        private FibonacciNode<TValue, TKey> _right;
        private FibonacciNode<TValue, TKey> _parent = null;
        private FibonacciNode<TValue, TKey> _child = null;

        public int Degree { get => _degree; set => _degree = value; }
        public TValue Value { get => _value; set => _value = value; }
        public TKey Key { get => _key; set => _key = value; }
        internal FibonacciNode<TValue, TKey> Left { get => _left; set => _left = value; }
        internal FibonacciNode<TValue, TKey> Right { get => _right; set => _right = value; }
        internal FibonacciNode<TValue, TKey> Parent { get => _parent; set => _parent = value; }
        internal FibonacciNode<TValue, TKey> Child { get => _child; set => _child = value; }

        public FibonacciNode(TValue value, TKey key)
        {
            _value = value;
            _key = key;
            _left = this;
            _right = this;
        }
    }
}
