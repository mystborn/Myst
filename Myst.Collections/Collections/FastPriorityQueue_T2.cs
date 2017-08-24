using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Myst.Collections
{
    public class FastPriorityQueue<TValue, TPriority> : ICollection<(TValue item, TPriority priority)>
    {
        private IComparer<TPriority> _comparer;
        private FastFibonacciNode<TValue, TPriority> _head = null;
        private int _count = 0;

        public int Count
        {
            get => _count;
        }

        public bool IsEmpty
        {
            get => _count == 0;
        }

        public bool IsReadOnly
        {
            get => false;
        }

        public FastPriorityQueue()
        {
            if(!typeof(IComparable<TPriority>).IsAssignableFrom(typeof(TPriority)))
                throw new InvalidOperationException($"{typeof(TPriority)} must implement {typeof(IComparable<TPriority>)} or provide a custom {typeof(IComparer<TPriority>)}");

            _comparer = Comparer<TPriority>.Default;
        }

        public FastPriorityQueue(IComparer<TPriority> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
        }

        public void Add(TValue item, TPriority priority)
        {
            var node = new FastFibonacciNode<TValue, TPriority>(item, priority);

            if (_head == null)
                _head = node;
            else
            {
                node.Left = _head;
                node.Right = _head.Right;
                _head.Right = node;
                node.Right.Left = node;

                if (_comparer.Compare(priority, _head.Key) < 0)
                    _head = node;
            }

            _count++;
        }

        public void Add((TValue item, TPriority priority) pair)
        {
            var node = new FastFibonacciNode<TValue, TPriority>(pair.item, pair.priority);

            if (_head == null)
                _head = node;
            else
            {
                node.Left = _head;
                node.Right = _head.Right;
                _head.Right = node;
                node.Right.Left = node;

                if (_comparer.Compare(pair.priority, _head.Key) < 0)
                    _head = node;
            }

            _count++;
        }

        public bool Contains(TValue item)
        {
            foreach(var value in Values())
            {
                if (value.Equals(item))
                    return true;
            }

            return false;
        }

        public bool Contains(TValue item, TPriority priority)
        {
            foreach(var pair in this)
            {
                if (pair.item.Equals(item) && pair.priority.Equals(priority))
                    return true;
            }

            return false;
        }

        public bool Contains((TValue item, TPriority priority) item)
        {
            foreach (var pair in this)
            {
                if (pair.item.Equals(item.item) && pair.priority.Equals(item.priority))
                    return true;
            }

            return false;
        }

        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        public void CopyTo((TValue item, TPriority priority)[] array, int arrayIndex)
        {
            foreach (var value in this)
                array[arrayIndex++] = value;
        }

        public void CopyValues(TValue[] array, int arrayIndex)
        {
            foreach (var value in Values())
                array[arrayIndex++] = value;
        }

        public TValue Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot remove value from an empty collection.");

            var result = _head.Value;
            int children = _head.Degree;
            var current = _head.Child;

            while (children > 0)
            {
                var right = current.Right;

                current.Left.Right = current.Right;
                current.Right.Left = current.Left;

                current.Left = _head;
                current.Right = _head.Right;
                _head.Right = current;
                current.Right.Left = current;

                current.Parent = null;
                current = right;
                --children;
            }

            _head.Left.Right = _head.Right;
            _head.Right.Left = _head.Left;

            if (_head.Right == _head)
                _head = null;
            else
            {
                _head = _head.Right;
                Consolidate();
            }

            --_count;

            return result;
        }

        public (TValue item, TPriority priority) DequeuePair()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot remove value from an empty collection.");

            var result = (_head.Value, _head.Key);
            int children = _head.Degree;
            var current = _head.Child;

            while (children > 0)
            {
                var right = current.Right;

                current.Left.Right = current.Right;
                current.Right.Left = current.Left;

                current.Left = _head;
                current.Right = _head.Right;
                _head.Right = current;
                current.Right.Left = current;

                current.Parent = null;
                current = right;
                --children;
            }

            _head.Left.Right = _head.Right;
            _head.Right.Left = _head.Left;

            if (_head.Right == _head)
                _head = null;
            else
            {
                _head = _head.Right;
                Consolidate();
            }

            --_count;

            return result;
        }

        public void Enqueue(TValue item, TPriority priority)
        {
            Add(item, priority);
        }

        bool ICollection<(TValue item, TPriority priority)>.Remove((TValue item, TPriority priority) item)
        {
            throw new NotImplementedException();
        }

        public TValue Peek()
        {
            return _head.Value;
        }

        public IEnumerator<(TValue item, TPriority priority)> GetEnumerator()
        {
            foreach (var item in GetChildren(_head).Select(node => (node.Value, node.Key)))
                yield return item;
        }

        public IEnumerable<TValue> Values()
        {
            foreach (var item in GetChildren(_head).Select(node => node.Value))
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Consolidate()
        {
            var roots = new Dictionary<int, FastFibonacciNode<TValue, TPriority>>((int)(_count * .5f));

            var rootCount = 0;
            var current = _head;
            var maxDegree = 0;

            if (current != null)
            {
                ++rootCount;
                current = current.Right;

                while (current != _head)
                {
                    ++rootCount;
                    current = current.Right;
                }
            }

            while (rootCount > 0)
            {
                var degree = current.Degree;
                var next = current.Right;

                while (true)
                {
                    if (!roots.TryGetValue(degree, out var sameDegree))
                    {
                        break;
                    }

                    if (_comparer.Compare(current.Key, sameDegree.Key) > 0)
                    {
                        var temp = current;
                        current = sameDegree;
                        sameDegree = temp;
                    }

                    Link(sameDegree, current);

                    roots.Remove(degree);

                    degree++;
                }

                roots[degree] = current;

                if (degree > maxDegree)
                    maxDegree = degree;

                current = next;
                --rootCount;
            }

            _head = null;
            
            var counter = 0;

            while (counter <= maxDegree)
            {
                roots.TryGetValue(counter++, out current);
                if (current == null)
                    continue;

                if (_head != null)
                {
                    current.Left.Right = current.Right;
                    current.Right.Left = current.Left;

                    current.Left = _head;
                    current.Right = _head.Right;
                    _head.Right.Left = current;
                    _head.Right = current;

                    if (_comparer.Compare(current.Key, _head.Key) < 0)
                        _head = current;
                }
                else
                    _head = current;
            }
        }

        private IEnumerable<FastFibonacciNode<TValue, TPriority>> GetChildren(FastFibonacciNode<TValue, TPriority> node)
        {
            var first = node;
            var current = node;

            if (current.Child != null)
                foreach (var child in GetChildren(current.Child))
                    yield return child;

            yield return current;

            current = current.Right;

            while (current != first)
            {
                if (current.Child != null)
                    foreach (var child in GetChildren(current.Child))
                        yield return child;

                yield return current;

                current = current.Right;
            }
        }

        private void Link(FastFibonacciNode<TValue, TPriority> child, FastFibonacciNode<TValue, TPriority> parent)
        {
            child.Left.Right = child.Right;
            child.Right.Left = child.Left;

            child.Parent = parent;

            if (parent.Child == null)
            {
                parent.Child = child;
                child.Right = child;
                child.Left = child;
            }
            else
            {
                child.Left = parent.Child;
                child.Right = parent.Child.Right;
                parent.Child.Right = child;
                child.Right.Left = child;
            }
            ++parent.Degree;
        }
    }
}
