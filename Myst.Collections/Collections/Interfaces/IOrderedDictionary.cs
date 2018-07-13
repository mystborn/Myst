using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Collections
{
    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        TValue this[int index] { get; set; }
        void Insert(int index, TKey key, TValue value);
        void RemoveAt(int index);
        bool TryGetIndex(int index, out TValue value);
    }
}
