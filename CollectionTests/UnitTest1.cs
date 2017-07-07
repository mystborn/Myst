using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myst.Collections;

namespace CollectionTests
{
    [TestClass]
    public class Grid_Tests
    {
        [TestMethod]
        public void Grid_Contains_ShouldBeTrue()
        {
            Grid<int> testGrid = new Grid<int>(5, 5, -1);
            testGrid[3, 4] = 5;

            Assert.IsTrue(testGrid.Contains(5));
        }

        [TestMethod]
        public void Grid_Contains_ShouldBeFalse()
        {
            Grid<int> testGrid = new Grid<int>(5, 5, -1);
            testGrid[3, 4] = 3;

            Assert.IsFalse(testGrid.Contains(5));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Grid_WhenIndexIsLessThan0()
        {
            Grid<int> testGrid = new Grid<int>(5, 5);
            testGrid[-1, 3] = 4;
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Grid_WhenIndexIsGreaterSize()
        {
            Grid<int> testGrid = new Grid<int>(5, 5);
            testGrid[5, 3] = 4;
        }

        [TestMethod]
        public void Grid_Enumerate()
        {
            Grid<int> testGrid = new Grid<int>(5, 5);
            
            for(var h = 0; h < testGrid.Height; h++)
            {
                for(var w = 0; w < testGrid.Width; w++)
                {
                    testGrid[w, h] = w + (h * testGrid.Width);
                }
            }

            var i = 0;
            foreach(var item in testGrid)
            {
                Assert.AreEqual(i++, item);
            }
        }

        [TestMethod]
        public void Grid_CopyTo_WithoutIndex()
        {
            Grid<int> source = new Grid<int>(5, 5, 2);
            Grid<int> dest = new Grid<int>(6, 6, 1);

            source.CopyTo(dest);

            Assert.IsTrue(dest[5, 0] == 1);
            Assert.IsTrue(dest[0, 1] == 2);
        }

        [TestMethod]
        public void Grid_CopyTo_WithIndex()
        {
            Grid<int> source = new Grid<int>(5, 5, 2);
            Grid<int> dest = new Grid<int>(6, 6, 1);

            source.CopyTo(dest, 1, 1);

            Assert.IsTrue(dest[5, 5] == 2);
            Assert.IsTrue(dest[0, 0] == 1);
            Assert.IsTrue(dest[0, 4] == 1);
            Assert.IsTrue(dest[1, 4] == 2);
        }
    }
}
