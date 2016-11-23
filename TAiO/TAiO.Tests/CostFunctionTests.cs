using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
using System.Collections.Generic;
namespace TAiO.Tests
{
    [TestClass]
    public class CostFunctionTests
    {

        private Dictionary<string, CostFunction> costFunctions;
        private Board testBoard;
        public CostFunctionTests()
        {
            costFunctions = new Dictionary<string, CostFunction>();
            foreach (var nf in CostFunctionFactory.AvailableFunctions)
                costFunctions.Add(nf.Name, nf.Function);
            testBoard = new Board(6, 10, new SortedList<BlockType, int>(), false);
            testBoard.Content = new int[6, 10]{
                {1, 1, 1, 0, 0, 4, 4, 0, 0, 0 },
                {1, 0, 2, 2, 0, 4, 0, 0, 0, 0 },
                {1, 2, 2, 3, 3, 4, 4, 0, 0, 0 },
                {2, 2, 3, 3, 3, 4, 5, 5, 0, 0 },
                {2, 6, 6, 6, 3, 4, 0, 5, 0, 0 },
                {6, 6, 6, 6, 4, 4, 5, 5, 0, 0 }
            };
        }

        [TestMethod]
        public void LeastHolesTest()
        {
            CostFunction cf;
            if (costFunctions.TryGetValue("Najmniej dziur", out cf) == false)
                Assert.Fail("Brak funkcji 'Najmniej dziur'");
            Assert.AreEqual(5, cf(testBoard));
        }

        [TestMethod]
        public void LowestHeightTest()
        {
            CostFunction cf;
            if (costFunctions.TryGetValue("Najmniejsza wysokość", out cf) == false)
                Assert.Fail("Brak funkcji 'Najmniejsza wysokość'");
            Assert.AreEqual(8, cf(testBoard));
        }

        [TestMethod]
        public void HighestAdjacencyTest()
        {
            CostFunction cf;
            if (costFunctions.TryGetValue("Największa przyległość", out cf) == false)
                Assert.Fail("Brak funkcji 'Największa przyległość'");
            Assert.AreEqual(-40, cf(testBoard));
        }

        [TestMethod]
        public void PrettiestHolesTest()
        {
            CostFunction cf;
            if (costFunctions.TryGetValue("Najładniejsze puste pola", out cf) == false)
                Assert.Fail("Brak funkcji 'Najładniejsze puste pola'");
            Assert.AreEqual(0, cf(testBoard));
        }
    }
}
