using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
namespace TAiO.Tests
{
    [TestClass]
    public class BlockTypeTests
    {
        /// <summary>
        /// Dokładne sprawdzenie standardowych obrotów.
        /// </summary>
        [TestMethod]
        public void FourRotationsTest()
        {
            BlockType block = new BlockType(2, 2, new int[2, 2]
            {
                { 1, 1},
                { 0, 1}
            });
            Assert.AreEqual(4, block.Shape.Count);
            bool[] rotationPassed = new bool[4];

            rotationPassed[0] =
                block.Shape[0][0, 0] == 1 && block.Shape[0][1, 0] == 0 &&
                block.Shape[0][0, 1] == 1 && block.Shape[0][1, 1] == 1;

            rotationPassed[1] =
               block.Shape[1][0, 0] == 1 && block.Shape[1][1, 0] == 1 &&
               block.Shape[1][0, 1] == 1 && block.Shape[1][1, 1] == 0;

            rotationPassed[2] =
               block.Shape[2][0, 0] == 1 && block.Shape[2][1, 0] == 1 &&
               block.Shape[2][0, 1] == 0 && block.Shape[2][1, 1] == 1;

            rotationPassed[3] =
                block.Shape[3][0, 0] == 0 && block.Shape[3][1, 0] == 1 &&
                block.Shape[3][0, 1] == 1 && block.Shape[3][1, 1] == 1;

            for (int i = 0; i < 4; ++i)
                Assert.IsTrue(rotationPassed[i]);
        }

        /// <summary>
        /// Sprawdzenie, czy program wykrył, że klocek jest pionowy i go odwrócił.
        /// </summary>
        [TestMethod]
        public void ChangedToHorizontalTest()
        {
            BlockType block = new BlockType(1, 4, new int[1, 4]
            {
                {1, 1, 1, 1 }
            });
            Assert.AreEqual(1, block.Height);
            Assert.AreEqual(4, block.Shape[0].Length);
        }

        /// <summary>
        /// Sprawdzenie, czy program wykrył powtarzające się obroty.
        /// </summary>
        [TestMethod]
        public void TrimmedRotationsTest()
        {
            BlockType block = new BlockType(1, 4, new int[1, 4]
            {
                {1, 1, 1, 1 }
            });
            Assert.AreEqual(2, block.Shape.Count);
        }
    }
}
