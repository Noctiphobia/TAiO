using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;

namespace TAiO.Tests
{
	[TestClass]
	public class BoardTests
	{
		private BlockType FShape;


		public BoardTests()
		{
			FShape = new BlockType(3, 5, new int[,]
			{
				{ 1, 1, 1, 1, 1},
				{ 1, 0, 1, 0, 0},
				{ 1, 0, 1, 0, 0},
			});
		}



		[TestMethod]
		public void AddOneBlockTest()
		{
			SortedList<BlockType, int> list = new SortedList<BlockType, int>(1);
			list.Add(FShape, 5);
			Board board = new Board(20, 20, list);
			board.AddBlock(new BlockInstance() {Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 1, Y = 1});

			for (int i = 0; i < FShape.Height; i++)
			{
				for (int j = 0; j < FShape.Width; j++)
				{
					if (board[j+1, i+1] != FShape.Shape[0][j, i])
						Assert.Fail("AddBlock added block in different place");

				}
			}
		}

		[TestMethod]
		public void AddBlocksInTheSamePlaceTest()
		{
			SortedList<BlockType, int> list = new SortedList<BlockType, int>(1);
			list.Add(FShape, 5);
			Board board = new Board(20, 20, list);
			BlockInstance bi = new BlockInstance() { Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 1, Y = 1 };
			if(!board.AddBlock(bi))
				Assert.Fail("Cannot add block");
			if(board.AddBlock(bi))
				Assert.Fail("Can add block on another block, which should be impossible");
		}

		[TestMethod]
		public void AddAndDeleteBlockTest()
		{
			SortedList<BlockType, int> list = new SortedList<BlockType, int>(1);
			list.Add(FShape, 5);
			Board board = new Board(20, 20, list);
			BlockInstance bi = new BlockInstance() { Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 1, Y = 1 };
			board.AddBlock(bi);
			board.DeleteBlock(bi);
			for (int i = 0; i < board.Height; i++)
			{
				for (int j = 0; j < board.Width; j++)
				{
					if (board[j, i] != 0)
						Assert.Fail("DeleteBlock deleted block from different place");

				}
			}
		}


		[TestMethod]
		public void CreateBoardFromStepsDataTest()
		{
			SortedList<BlockType, int> list = new SortedList<BlockType, int>(1);
			list.Add(FShape, 5);
			Board board = new Board(20, 20, list);
			BlockInstance bi = new BlockInstance() { Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 1, Y = 1 };
			board.AddBlock(bi);
			//
			StepsData data = new StepsData(2, 5);
			data.BlockInstances[0, 0] = bi;
			data.BlockInstances[0, 1] = new BlockInstance() { Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 4, Y = 4 };
			data.LastStepFinished = 1;
			Board board2 = Board.CreateFromStepsData(data, 0, 0, 20, 20, true, new SortedList<BlockType, int>(list));
			for (int i = 0; i < board.Height; i++)
			{
				for (int j = 0; j < board.Width; j++)
				{
					if (board[i, j] != board2[i, j])
					{
						Assert.Fail("Boards are different, creating from StepsData failed.");
					}
				}
			}

		}

		[TestMethod]
		public void ChooseBlocksTest()
		{
			SortedList<BlockType, int> list = new SortedList<BlockType, int>(1);
			list.Add(FShape, 5);
			Board board = new Board(20, 20, list);
			BlockInstance bi = new BlockInstance() { Block = FShape, BlockVersion = 0, PreviousBlockBoardNumber = 0, X = 1, Y = 1 };

			List<PartialSolution> solutions = board.ChooseBlocks(1, CostFunctionFactory.AvailableFunctions.Find(s => s.Name == "Najmniej dziur").Function,
				PlacementFunctionFactory.AvailableFunctions[0].Function);
			if(solutions.Count != 1)
				Assert.Fail("Got wrong number of solutions");
			if(solutions[0].Move.Block != bi.Block)
				Assert.Fail("Wrong solution");
			if (solutions[0].Move.BlockVersion != 0)
				Assert.Fail("Wrong block version");
			if (solutions[0].Move.X != 0)
				Assert.Fail("Wrong place");
			if (solutions[0].Move.Y != 0)
				Assert.Fail("Wrong place");
		}


	}
}
