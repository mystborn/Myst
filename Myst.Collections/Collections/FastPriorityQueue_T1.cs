using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Myst.Collections
{
    /// <summary>
    /// Minimalist Priority Queue implemented using a Fibonacci Heap
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastPriorityQueue<T> : ICollection<T>
    {
        private IComparer<T> _comparer;
        private FastFibonacciNode<T> _head = null;
        private int _count = 0;

        /// <summary>
        /// Get the current amount of items in the <see cref="FastPriorityQueue{T}"/>
        /// </summary>
        public int Count
        {
            get => _count;
        }

        /// <summary>
        /// Determine whether the <see cref="FastPriorityQueue{T}"/> is empty.
        /// </summary>
        public bool IsEmpty
        {
            get => _count == 0;
        }

        /// <summary>
        /// Determine whether the <see cref="FastPriorityQueue{T}"/> is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get => false;
        }

        /// <summary>
        /// Construct a <see cref="FastPriorityQueue{T}"/> using the default <see cref="Comparer"/>.
        /// </summary>
        public FastPriorityQueue()
        {
            if(!typeof(IComparable).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException($"{typeof(T)} must implement {typeof(IComparable<T>)} or provide a custom {typeof(IComparer<T>)}");

            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Construct a <see cref="FastPriorityQueue{T}"/> using a supplemented <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer"></param>
        public FastPriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
        }

        /// <summary>
        /// Add an item to the <see cref="FastPriorityQueue{T}"/>. 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            var node = new FastFibonacciNode<T>(item);

            if (_head == null)
                _head = node;
            else
            {
                node.Left = _head;
                node.Right = _head.Right;
                _head.Right = node;
                node.Right.Left = node;

                if (_comparer.Compare(item, _head.Value) < 0)
                    _head = node;
            }

            _count++;
        }

        /// <summary>
        /// Clear all items from the <see cref="FastPriorityQueue{T}"/>
        /// </summary>
        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        /// <summary>
        /// Determine if the <see cref="FastPriorityQueue{T}"/> contains the specified item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach(var value in this)
            {
                if (value.Equals(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Copies all values unordered from the <see cref="FastPriorityQueue{T}"/> to the given array starting at the given index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var value in this)
                array[arrayIndex++] = value;
        }

        /// <summary>
        /// Get and remove the current value with the minumum priority.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot remove value from an empty collection.");

            var result = _head.Value;
            int children = _head.Degree;
            var current = _head.Child;

            while(children > 0)
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
            {
                _head = null;
                Console.WriteLine("Head is null");
            }
            else
            {
                _head = _head.Right;
                Consolidate();
                Console.WriteLine("Head is not null");
            }

            --_count;

            return result;
        }

        /// <summary>
        /// Add an item to the <see cref="FastPriorityQueue{T}"/>
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            Add(item);
        }

        /// <summary>
        /// View the item with the smallest priority without removing.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return _head.Value;
        }

        /// <summary>
        /// Satisfy <see cref="ICollection{T}"/> implementation. Cannot use.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the enumerater used to enumerate unordered through all values in the <see cref="FastPriorityQueue{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in GetChildren(_head).Select(node => node.Value))
                yield return item;
        }

        /// <summary>
        /// Satisfy <see cref="ICollection{T}"/> implementation.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Consolidate()
        {
            var roots = new Dictionary<int, FastFibonacciNode<T>>((int)(_count * .5f));

            var rootCount = 0;
            var current = _head;
            var maxDegree = 0;

            if(current != null)
            {
                ++rootCount;
                current = current.Right;

                while(current != _head)
                {
                    ++rootCount;
                    current = current.Right;
                }
            }

            while(rootCount > 0)
            {
                var degree = current.Degree;
                var next = current.Right;

                while(true)
                {
                    if(!roots.TryGetValue(degree, out var sameDegree))
                    {
                        break;
                    }

                    if(_comparer.Compare(current.Value, sameDegree.Value) > 0)
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

            while(counter <= maxDegree)
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

                    if (_comparer.Compare(current.Value, _head.Value) < 0)
                        _head = current;
                }
                else
                    _head = current;
            }
        }

        private IEnumerable<FastFibonacciNode<T>> GetChildren(FastFibonacciNode<T> node)
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

        private void Link(FastFibonacciNode<T> child, FastFibonacciNode<T> parent)
        {
            child.Left.Right = child.Right;
            child.Right.Left = child.Left;

            child.Parent = parent;

            if(parent.Child == null)
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
