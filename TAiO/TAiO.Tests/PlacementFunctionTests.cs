using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
using System.Collections.Generic;
namespace TAiO.Tests
{
    [TestClass]
    public class PlacementFunctionTests
    {
        PlacementFunction placement;
        /// <summary>
        /// Standardowa plansza.
        /// </summary>
        private Board testBoard;

        /// <summary>
        /// Pełna plansza, którą trzeba powiększyć.
        /// </summary>
        private Board fullBoard;

        /// <summary>
        /// Zbyt mała plansza na jakikolwiek klocek o szerokości większej niż 1.
        /// </summary>
        private Board tinyBoard;

        /// <summary>
        /// Podłużny klocek.
        /// </summary>
        private BlockType longBlock;

        /// <summary>
        /// Mały klocek.
        /// </summary>
        private BlockType smallBlock;
        public PlacementFunctionTests()
        {
            placement = PlacementFunctionFactory.AvailableFunctions[0].Function;
            testBoard = new Board(6, 10, new SortedList<BlockType, int>(), false);
            testBoard.Content = new int[6, 10]{
                {1, 1, 1, 0, 0, 4, 4, 0, 0, 0 },
                {1, 0, 2, 2, 0, 4, 0, 0, 0, 0 },
                {1, 2, 2, 3, 3, 4, 4, 0, 0, 0 },
                {2, 2, 3, 3, 3, 4, 5, 5, 0, 0 },
                {2, 6, 6, 6, 3, 4, 0, 5, 0, 0 },
                {6, 6, 6, 6, 4, 4, 5, 5, 0, 0 }
            };
            fullBoard = new Board(6, 10, new SortedList<BlockType, int>(), false);
            fullBoard.Content = new int[6, 10]{
                {1, 1, 1, 0, 0, 4, 4, 0, 7, 8 },
                {1, 0, 2, 2, 0, 4, 0, 0, 7, 8 },
                {1, 2, 2, 3, 3, 4, 4, 0, 7, 8 },
                {2, 2, 3, 3, 3, 4, 5, 5, 7, 8 },
                {2, 6, 6, 6, 3, 4, 0, 5, 7, 8 },
                {6, 6, 6, 6, 4, 4, 5, 5, 8, 8 }
            };
            tinyBoard = new Board(1, 10, new SortedList<BlockType, int>(), false);
            tinyBoard.Content = new int[1, 10]{
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            longBlock = new BlockType(4, 1, new int[4, 1]
            {
                {1},
                {1},
                {1},
                {1}
            });
            smallBlock = new BlockType(2, 2, new int[2, 2]
            {
                { 1, 1},
                { 0, 1}
            });
        }

        /// <summary>
        /// Położenie na wierzchu dużego klocka.
        /// </summary>
        [TestMethod]
        public void StandardPlacementTest()
        {
            BlockInstance returned = placement(testBoard, longBlock, 0);
            BlockInstance reference = new BlockInstance
            {
                Block = longBlock,
                BlockVersion = 0,
                X = 0,
                Y = 8
            };
            Assert.AreEqual(reference, returned);
        }

        /// <summary>
        /// Zmieszczenie małego klocka w środku.
        /// </summary>
        [TestMethod]
        public void InsidePlacementTest()
        {
            BlockInstance returned = placement(testBoard, smallBlock, 0);
            BlockInstance reference = new BlockInstance
            {
                Block = smallBlock,
                BlockVersion = 0,
                X = 0,
                Y = 3
            };
            Assert.AreEqual(reference, returned);
        }

        /// <summary>
        /// Położenie niemieszczącego się klocka.
        /// </summary>
        [TestMethod]
        public void ResizePlacementTest()
        {
            BlockInstance returned = placement(fullBoard, longBlock, 0);
            BlockInstance reference = new BlockInstance
            {
                Block = longBlock,
                BlockVersion = 0,
                X = 0,
                Y = 10
            };
            Assert.AreEqual(reference, returned);
        }

        /// <summary>
        /// Włożenie zbyt szerokiego klocka.
        /// </summary>
        [TestMethod]
        public void TooBigPlacementTest()
        {
            BlockInstance returned = placement(tinyBoard, smallBlock, 0);
            BlockInstance reference = new BlockInstance
            {
                Block = null
            };
            Assert.AreEqual(reference, returned);
        }

        /// <summary>
        /// Włożenie idealnie mieszczącego się do planszy klocka.
        /// </summary>
        [TestMethod]
        public void SmallEnoughPlacementTest()
        {
            BlockInstance returned = placement(tinyBoard, longBlock, 1);
            BlockInstance reference = new BlockInstance
            {
                Block = longBlock,
                BlockVersion = 1,
                X = 0,
                Y = 0
            };
            Assert.AreEqual(reference, returned);
        }
    }
}
