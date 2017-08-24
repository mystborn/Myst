using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    internal class FastFibonacciNode<T>
    {
        private T _value;
        private FastFibonacciNode<T> _left;
        private FastFibonacciNode<T> _right;
        private FastFibonacciNode<T> _parent = null;
        private FastFibonacciNode<T> _child = null;
        private int _degree = 0;

        public T Value { get => _value; set => _value = value; }
        public int Degree { get => _degree; set => _degree = value; }
        internal FastFibonacciNode<T> Left { get => _left; set => _left = value; }
        internal FastFibonacciNode<T> Right { get => _right; set => _right = value; }
        internal FastFibonacciNode<T> Parent { get => _parent; set => _parent = value; }
        internal FastFibonacciNode<T> Child { get => _child; set => _child = value; }

        public FastFibonacciNode(T value)
        {
            _value = value;
            _left = this;
            _right = this;
        }
    }

    internal class FastFibonacciNode<TValue, TKey>
    {
        private int _degree = 0;
        private TValue _value;
        private TKey _key;
        private FastFibonacciNode<TValue, TKey> _left;
        private FastFibonacciNode<TValue, TKey> _right;
        private FastFibonacciNode<TValue, TKey> _parent = null;
        private FastFibonacciNode<TValue, TKey> _child = null;

        public int Degree { get => _degree; set => _degree = value; }
        public TValue Value { get => _value; set => _value = value; }
        public TKey Key { get => _key; set => _key = value; }
        internal FastFibonacciNode<TValue, TKey> Left { get => _left; set => _left = value; }
        internal FastFibonacciNode<TValue, TKey> Right { get => _right; set => _right = value; }
        internal FastFibonacciNode<TValue, TKey> Parent { get => _parent; set => _parent = value; }
        internal FastFibonacciNode<TValue, TKey> Child { get => _child; set => _child = value; }

        public FastFibonacciNode(TValue value, TKey key)
        {
            _value = value;
            _key = key;
            _left = this;
            _right = this;
        }
    }
}
