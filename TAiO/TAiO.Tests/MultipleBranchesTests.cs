using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
using TAiO.Tools;

namespace TAiO.Tests
{
	[TestClass]
	public class MultipleBranchesTests
	{
		[TestMethod]
		public void TwoBranches()
		{
			Data Data = Data.Instance;
			List<BlockType> blocks;
			int width;
			Parser.ParseFile("60.blocks", out blocks, out width);

			int blocksNumber = 5;
			if(blocks.Count == 0)
				Assert.Fail("0 blocks");
			blocks[0].BlockNumber =(uint)blocksNumber;

			for (int i = 1; i < blocks.Count; i++)
			{
				blocks[i].BlockNumber = 0;
			}
			Data.Blocks = blocks;
			Data.BoardWidth = 10;
			Data.Branches = 2;

			Algorithm a = new Algorithm(Data, CostFunctionFactory.AvailableFunctions.
				Find(b => b.Name == "Najmniej dziur").Function);
			
			a.RunAlgorithm();
		}

		[TestMethod]
		public void ThreeBranches()
		{
			Data Data = Data.Instance;
			List<BlockType> blocks;
			int width;
			Parser.ParseFile("60.blocks", out blocks, out width);

			int blocksNumber = 5;
			if (blocks.Count == 0)
				Assert.Fail("0 blocks");
			blocks[0].BlockNumber = (uint)blocksNumber;

			for (int i = 1; i < blocks.Count; i++)
			{
				blocks[i].BlockNumber = 0;
			}
			Data.Blocks = blocks;
			Data.BoardWidth = 10;
			Data.Branches = 3;

			Algorithm a = new Algorithm(Data, CostFunctionFactory.AvailableFunctions.
				Find(b => b.Name == "Najmniej dziur").Function);

			a.RunAlgorithm();
		}


		[TestMethod]
		public void ThreeBranchesWholeSet()
		{
			Data Data = Data.Instance;
			List<BlockType> blocks;
			int width;
			Parser.ParseFile("60.blocks", out blocks, out width);
			
			if (blocks.Count == 0)
				Assert.Fail("0 blocks");

			for (int i = 0; i < blocks.Count; i++)
			{
				blocks[i].BlockNumber = 0;
			}

			Data.Blocks = blocks;
			Data.BoardWidth = width;
			Data.Branches = 3;

			Algorithm a = new Algorithm(Data, CostFunctionFactory.AvailableFunctions.
				Find(b => b.Name == "Najmniej dziur").Function);

			a.RunAlgorithm();
		}
	}
}
