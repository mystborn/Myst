using System.Collections.Generic;

namespace Myst.Collections
{
    /// <summary>
    /// Binds objects to an index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBinder<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>Item</returns>
        T this[int index] { get; }

        /// <summary>
        /// Gets the number of bound items.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an item to the binder, returning it's index.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item index.</returns>
        int Add(T item);

        /// <summary>
        /// Determines if the Binder contains an item with the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        bool Contains(int index);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        bool Remove(int index);

        /// <summary>
        /// Attempts to get the item at the specified index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="value">The item, if found.</param>
        /// <returns>True if the item was found; otherwise false.</returns>
        bool TryGetValue(int index, out T value);
    }
}
