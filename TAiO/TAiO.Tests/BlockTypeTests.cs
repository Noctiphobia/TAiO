using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
namespace TAiO.Tests
{
    [TestClass]
    public class BlockTypeTests
    {
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
                block.Shape[1][0, 0] == 0 && block.Shape[1][1, 0] == 1 &&
                block.Shape[1][0, 1] == 1 && block.Shape[1][1, 1] == 1;

            rotationPassed[2] =
                block.Shape[2][0, 0] == 1 && block.Shape[2][1, 0] == 1 &&
                block.Shape[2][0, 1] == 1 && block.Shape[2][1, 1] == 0;

            rotationPassed[3] =
                block.Shape[3][0, 0] == 1 && block.Shape[3][1, 0] == 1 &&
                block.Shape[3][0, 1] == 0 && block.Shape[3][1, 1] == 1;
        }
    }
}
