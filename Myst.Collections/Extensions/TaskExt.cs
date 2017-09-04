using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myst.Extensions
{
    public static class TaskExt
    {
        public static async Task<T> WhenAny<T>(this IEnumerable<Task<T>> @this, Predicate<T> match)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var taskList = new LinkedList<Task<T>>(@this);

            while (taskList.Any())
            {
                await Task.WhenAny(taskList);

                var taskNode = taskList.First;
                while (taskNode != null)
                {
                    var next = taskNode.Next;

                    if (taskNode.Value.IsCompleted)
                    {
                        var result = taskNode.Value.Result;

                        if (match.Invoke(result))
                            return result;

                        taskList.Remove(taskNode);
                    }

                    taskNode = next;
                }
            }

            throw new ArgumentException($"None of the given tasks returned a result matching {match}", nameof(@this));
        }

        public static async Task<bool> IfAny<T>(this IEnumerable<Task<T>> @this, Predicate<T> match)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var taskList = new LinkedList<Task<T>>(@this);

            while (taskList.Any())
            {
                await Task.WhenAny(taskList);

                var taskNode = taskList.First;
                while (taskNode != null)
                {
                    var next = taskNode.Next;

                    if (taskNode.Value.IsCompleted)
                    {
                        var result = taskNode.Value.Result;

                        if (match.Invoke(result))
                            return true;

                        taskList.Remove(taskNode);
                    }

                    taskNode = next;
                }
            }

            return false;
        }
    }
}
