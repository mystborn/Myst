using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    public struct TableItem<TRow, TCol, TValue> : IEquatable<TableItem<TRow, TCol, TValue>>
    {
        public TRow Row { get; }
        public TCol Col { get; }
        public TValue Value { get; }

        public TableItem(TRow row, TCol col, TValue value)
        {
            Row = row;
            Col = col;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is TableItem<TRow, TCol, TValue> item)
                return Equals(item);

            return false;
        }

        public bool Equals(TableItem<TRow, TCol, TValue> other)
        {
            return Row.Equals(other.Row) && 
                Col.Equals(other.Col) && 
                Value.Equals(other.Value);
        }

        public static bool operator==(TableItem<TRow, TCol, TValue> left, TableItem<TRow, TCol, TValue> right)
        {
            //Don't check for nullity, this is a struct.
            return left.Equals(right);
        }

        public static bool operator!=(TableItem<TRow, TCol, TValue> left, TableItem<TRow, TCol, TValue> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Row.GetHashCode();
                hash = hash * 23 + Col.GetHashCode();
                hash = hash * 23 + Value.GetHashCode();
                return hash;
            }
        }
    }
}
