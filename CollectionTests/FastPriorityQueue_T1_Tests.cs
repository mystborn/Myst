using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myst.Collections;

namespace CollectionTests
{
    [TestClass]
    public class FastPriorityQueue_T1_Tests
    {
        [TestMethod]
        public void Queue_T1_InsertWorks()
        {
            var queue = new FastPriorityQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(2);
            queue.Enqueue(4);
            queue.Enqueue(1);

            Assert.IsTrue(queue.Count == 5);
        }

        [TestMethod]
        public void Queue_T1_InsertWorksCorrectly()
        {
            var queue = new FastPriorityQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(2);
            queue.Enqueue(4);
            queue.Enqueue(1);
            while(!queue.IsEmpty)
            {
                queue.Dequeue();
            }
            Assert.IsTrue(queue.IsEmpty);
        }
    }
}
