using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Myst.Collections
{
    public class Table<TRow, TCol, TValue>
    {
        private Dictionary<long, TableItem<TRow, TCol, TValue>> _source;

        public int Count => _source.Count;

        public TValue this[TRow row, TCol col]
        {
            get
            {
                if (!TryGetValue(row, col, out var item))
                    throw new ArgumentOutOfRangeException($"No value in table at row {row} and column {col}");
                return item;
            }
            set => _source[GetKey(row, col)] = new TableItem<TRow, TCol, TValue>(row, col, value);
        }

        public void Add(TRow row, TCol col, TValue value)
        {
            _source.Add(GetKey(row, col), new TableItem<TRow, TCol, TValue>(row, col, value));
        }

        public void Clear()
        {
            _source.Clear();
        }

        public bool ContainsKeys(TRow row, TCol col)
        {
            if(_source.TryGetValue(GetKey(row, col), out var item))
                if (TableEquals(row, col, item))
                    return true;

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            foreach (var item in _source.Values.Select(ti => ti.Value))
                if (item.Equals(value))
                    return true;

            return false;
        }

        public IEnumerator<TableItem<TRow, TCol, TValue>> GetEnumerator()
        {
            return _source.Values.GetEnumerator();
        }

        public bool TryGetValue(TRow row, TCol col, out TValue value)
        {
            value = default(TValue);
            if(_source.TryGetValue(GetKey(row, col), out var item))
            {
                if (TableEquals(row, col, item))
                {
                    value = item.Value;
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TableEquals(TRow row, TCol col, TableItem<TRow, TCol, TValue> item)
        {
            return item.Row.Equals(row) && item.Col.Equals(col);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetKey(TRow row, TCol col)
        {
            return ((long)row.GetHashCode() << 32) | (uint)col.GetHashCode();
        }
    }
}
