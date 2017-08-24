using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    public class LookupTable<TRow, TCol, TValue>
    {
        private int _count = 0;
        private int _columns = 0;
        private Dictionary<TRow, Dictionary<TCol, TValue>> _source;
        private IEqualityComparer<TCol> _colComparer;

        public IEqualityComparer<TRow> RowComparer => _source.Comparer;
        public IEqualityComparer<TCol> ColumnComparer => _colComparer;

        public int Count => _count;

        public TValue this[TRow row, TCol col]
        {
            get
            {
                if(TryGetInternalSource(row, out var inner))
                {
                    if (inner.TryGetValue(col, out var value))
                        return value;
                    else
                        throw new ArgumentNullException("col");
                }
                else
                    throw new ArgumentNullException("row");
            }
            set
            {
                var inner = GetInternalSource(row);
                inner[col] = value;
            }
        }

        public LookupTable() : this(EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        public LookupTable(ICollection<(TRow row, TCol col, TValue value)> source) : this(source, EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        public LookupTable(ICollection<(TRow row, TCol col, TValue value)> source, IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer)
        {
            _source = new Dictionary<TRow, Dictionary<TCol, TValue>>(rowComparer);
            _colComparer = columnComparer;
            foreach(var item in source)
            {
                var inner = GetInternalSource(item.row);
                inner.Add(item.col, item.value);
            }
        }

        public LookupTable(IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer) : this(0, 0, rowComparer, columnComparer) { }

        public LookupTable(int defaultRows, int defaultColumns) : this(defaultRows, defaultColumns, EqualityComparer<TRow>.Default, EqualityComparer<TCol>.Default) { }

        public LookupTable(int defaultRows, int defaultColumns, IEqualityComparer<TRow> rowComparer, IEqualityComparer<TCol> columnComparer)
        {
            _colComparer = columnComparer;
            _columns = defaultColumns;
            _source = new Dictionary<TRow, Dictionary<TCol, TValue>>(defaultRows, rowComparer);
        }

        public void Add(TRow key1, TCol key2, TValue value)
        {
            var dict = GetInternalSource(key1);
            dict.Add(key2, value);
            _count++;
        }

        public void Clear()
        {
            _source.Clear();
            _count = 0;
        }

        public bool ContainsKeys(TRow row, TCol col)
        {
            if(TryGetInternalSource(row, out var inner))
                return inner.ContainsKey(col);

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            foreach(var inner in _source.Values)
            {
                foreach(var element in inner.Values)
                {
                    if (element.Equals(value))
                        return true;
                }
            }
            return false;
        }

        public bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer)
        {
            foreach (var inner in _source.Values)
            {
                foreach (var element in inner.Values)
                {
                    if (comparer.Equals(value, element))
                        return true;
                }
            }
            return false;
        }

        public bool Remove(TRow row, TCol col)
        {
            if (TryGetInternalSource(row, out var inner))
            {
                _count--;
                return inner.Remove(col);
            }
            return false;
        }

        public bool TryGetValue(TRow row, TCol col, out TValue value)
        {
            if (TryGetInternalSource(row, out var inner))
                return inner.TryGetValue(col, out value);

            value = default(TValue);
            return false;
        }

        private Dictionary<TCol, TValue> GetInternalSource(TRow key)
        {
            if(!_source.TryGetValue(key, out var inner))
            {
                inner = new Dictionary<TCol, TValue>(_columns, _colComparer);
                _source.Add(key, inner);
            }

            return inner;
        }

        private bool TryGetInternalSource(TRow key, out Dictionary<TCol, TValue> inner)
        {
            return _source.TryGetValue(key, out inner);
        }
    }
}
