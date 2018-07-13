using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    /// <summary>
    /// Represents a generic collection of objects mapped to a table index.
    /// </summary>
    public interface ITable<TRow, TCol, TValue>
    {
        /// <summary>
        /// The number of values in this <see cref="ITable{TRow, TCol, TValue}"/>
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or sets the value at the specified table index.
        /// </summary>
        /// <param name="row">The row of the value.</param>
        /// <param name="col">The column of the value.</param>
        /// <returns>The value at the specified index.</returns>
        TValue this[TRow row, TCol col] { get; set; }

        /// <summary>
        /// Adds a value to the <see cref="ITable{TRow, TCol, TValue}"/> at the specified index.
        /// </summary>
        /// <param name="row">The row of the index.</param>
        /// <param name="col">The column of the index.</param>
        /// <param name="value">The value to add.</param>
        void Add(TRow row, TCol col, TValue value);

        /// <summary>
        /// Adds an item into the <see cref="ITable{TRow, TCol, TValue}"/>.
        /// </summary>
        /// <param name="item"></param>
        void Add(TableItem<TRow, TCol, TValue> item);

        /// <summary>
        /// Clears all values from the <see cref="ITable{TRow, TCol, TValue}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines if a value exists at the specified table index.
        /// </summary>
        /// <param name="row">The row of the index.</param>
        /// <param name="col">The column of the index.</param>
        /// <returns><see cref="true"/> if there is a value at the index, <see cref="false"/> otherwise.</returns>
        bool ContainsIndex(TRow row, TCol col);

        /// <summary>
        /// Determines if a value exists within the <see cref="ITable{TRow, TCol, TValue}"/>, using the default equality comparer.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <returns><see cref="true"/> if the value exists, <see cref="false"/> otherwise.</returns>
        bool ContainsValue(TValue value);
        /// <summary>
        /// Determines if a value exists within the <see cref="ITable{TRow, TCol, TValue}"/>, using the specified equality comparer.
        /// </summary>
        /// <param name="value">The value to look for.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used to determine if the specified value equals a value within the <see cref="ITable{TRow, TCol, TValue}"/>.</param>
        /// <returns></returns>
        bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer);

        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> that iterates through the <see cref="ITable{TRow, TCol, TValue}"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerator<TableItem<TRow, TCol, TValue>> GetEnumerator();

        /// <summary>
        /// Removes a value at the specified table index.
        /// </summary>
        /// <param name="row">The row of the index.</param>
        /// <param name="col">The column of the index.</param>
        /// <returns><see cref="true"/> if a value was removed, <see cref="false"/> otherwise.</returns>
        bool Remove(TRow row, TCol col);

        /// <summary>
        /// Attempts to retrieve a value from the <see cref="ITable{TRow, TCol, TValue}"/> at the specified index.
        /// </summary>
        /// <param name="row">The row of the index.</param>
        /// <param name="col">The column of the index.</param>
        /// <param name="value">The value taht was retrieved if successful, otherwise the default of <see cref="TValue"/>.</param>
        /// <returns></returns>
        bool TryGetValue(TRow row, TCol col, out TValue value);
    }
}