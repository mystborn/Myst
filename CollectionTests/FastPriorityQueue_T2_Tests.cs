using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myst.Collections;

namespace CollectionTests
{
    [TestClass]
    public class FastPriorityQueue_T2_Tests
    {
        [TestMethod]
        public void Queue_T2_Does_Insert()
        {
            var queue = new FastPriorityQueue<int, int>();
            Assert.AreEqual(0, queue.Count);

            queue.Enqueue(0, 5);
            queue.Enqueue(1, 4);

            Assert.AreEqual(2, queue.Count);
        }

        [TestMethod]
        public void Queue_T2_Gets_Min_Correctly()
        {
            var queue = new FastPriorityQueue<int, int>();
            queue.Enqueue(5, 0);
            queue.Enqueue(0, 5);
            queue.Enqueue(1, 4);
            queue.Enqueue(3, 2);
            queue.Enqueue(2, 3);
            queue.Enqueue(4, 1);

            Assert.AreEqual(5, queue.Peek());

            var test = new int[] { 5, 4, 3, 2, 1, 0 };
            var sorted = new int[6];

            int index = 0;
            while(!queue.IsEmpty)
            {
                sorted[index++] = queue.Dequeue();
            }

            CollectionAssert.AreEqual(test, sorted);
        }

        [TestMethod]
        public void Queue_T2_Gets_Contain_Correctly()
        {
            var queue = new FastPriorityQueue<int, int>();
            queue.Enqueue(5, 0);
            queue.Enqueue(0, 5);
            queue.Enqueue(1, 4);
            queue.Enqueue(3, 2);
            queue.Enqueue(2, 3);
            queue.Enqueue(4, 1);

            Assert.IsTrue(queue.Contains(0));
            Assert.IsFalse(queue.Contains(6));
            Assert.IsTrue(queue.Contains(5, 0));
            Assert.IsFalse(queue.Contains(2, 0));
            Assert.IsTrue(queue.Contains((1, 4)));
            Assert.IsFalse(queue.Contains((3, 3)));
        }
    }
}
