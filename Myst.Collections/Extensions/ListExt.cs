using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Extensions
{
    public static class ListExt
    {
        public static T Pop<T>(this List<T> list)
        {
            var index = list.Count - 1;
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}
