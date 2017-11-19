using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Extensions
{
    public static class ListExt
    {
        /// <summary>
        /// Removes the last element from a list and returns it.
        /// </summary>
        /// <param name="list">The <see cref="List{T}"/> to remove the element from.</param>
        /// <returns>The removed element.</returns>
        public static T Pop<T>(this List<T> list)
        {
            var index = list.Count - 1;
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Retrieves the last element of a list without removing it.
        /// </summary>
        /// <param name="list">The <see cref="List{T}"/> to retrieve the element from.</param>
        /// <returns>The last element of the list.</returns>
        public static T Peek<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
    }
}
