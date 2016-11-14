using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TAiO.Model
{
	/// <summary>
	/// Klasa zawierające wszystkie dane dotyczące przebiegu algorytmu
	/// i zawierająca metody pozwalające na wykonanie go
	/// </summary>
	public class Algorithm
	{
		public StepsData StepsData { get; private set; }
		public int CurrentStep => StepsData.LastStepFinished;
		private Data Data;
		private CostFunction CostFunction;
		private PlacementFunction PlacementFunction;
		//private List<Board> CurrentStepBoards;
		private Board[] CurrentStepBoards;
		private List<List<BlockType>> CurrentStepBoardsBlocks;
		//private List<KeyValuePair<BlockType, int>> BlocksOfTypeCount; 
		private SortedList<BlockType, int> AvailableBlocksSorted;
		


		public Algorithm(Data data, CostFunction costFunction, PlacementFunction placementFunction)
		{
			Data = data;
			CostFunction = costFunction;
			PlacementFunction = placementFunction;
			CurrentStepBoards = new Board[Data.Branches];
			if (Data.Blocks != null)
			{
				AvailableBlocksSorted = new SortedList<BlockType, int>(Data.Blocks.Count);
				foreach (BlockType block in Data.Blocks)
				{
					if(block.BlockNumber > 0)
						AvailableBlocksSorted.Add(block, (int)block.BlockNumber);
				}
			}
			StepsData = new StepsData(Data.Branches, Data.Blocks.Sum(a => (int)a.BlockNumber));
		}

		public void RunAlgorithm()
		{
				int availableBlocks = Data.Blocks.Sum(a => (int)a.BlockNumber);
				while (StepsData.LastStepFinished < availableBlocks - 1)
				{
					MakeNextStep();
				}
		}

		public void MakeNextStep()
		{

			UpdateStepBoardsNew(Data.BoardWidth, Data.BoardWidth);

			if (CurrentStep < 0 && CurrentStepBoards.Length > 0)
			{
				List<PartialSolution> solutions = (CurrentStepBoards[0].ChooseBlocks(Data.Branches, CostFunction, PlacementFunction));
				DivideSolutionsBetweenBoards(solutions);
			}
			else
			{

				var tasks = new Task[CurrentStepBoards.Length];
				var partialSolutions = new List<PartialSolution>[CurrentStepBoards.Length];


				for (int i = 0; i < CurrentStepBoards.Length; i++)
				{
					int j = i;
					(tasks[j] =
						new Task(() =>
						{
							partialSolutions[j] = (CurrentStepBoards[j].ChooseBlocks(Data.Branches, CostFunction, PlacementFunction));
							foreach (PartialSolution ps in partialSolutions[j])
							{
								BlockInstance bi = ps.Move;
								bi.PreviousBlockBoardNumber = j;
								ps.Move = bi;
							}

						})).Start();
				}

				Task.WaitAll(tasks);

				//TODO: change MergeSolutions

				MergeSolutions(partialSolutions.ToList());
			}

			StepsData.SetLastStepFinished(CurrentStep + 1);
		}


		public void UpdateStepBoardsNew(int width, int height)
		{
			if (CurrentStep < 0) // only creating new boards
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					CurrentStepBoards[i] = new Board(width, height, new SortedList<BlockType, int>(AvailableBlocksSorted));
				}
			}
			else if (CurrentStep == 0) // only adding new blocks
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					if (!CurrentStepBoards[i].AddBlock(StepsData.BlockInstances[CurrentStep, i]))
					{
						throw new ArgumentException("Cannot add block!");
						// TODO: delete or add exceptions
						// and try/catch...
					}
				}
			}
			else // creating new or only adding new blocks
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					if (StepsData.BlockInstances[CurrentStep, i].PreviousBlockBoardNumber != i) // if we have to copy the board
					{
						CurrentStepBoards[i] = Board.CreateFromStepsData(StepsData, CurrentStep, i,
							CurrentStepBoards[i].Width, CurrentStepBoards[i].Height, true,
							new SortedList<BlockType, int>(AvailableBlocksSorted));
					}
					else
					{
						if (!CurrentStepBoards[i].AddBlock(StepsData.BlockInstances[CurrentStep, i])) // if we can only add new block
						{
							throw new ArgumentException("Cannot add block!");
							// TODO: delete or add exceptions
							// and try/catch...
						}
					}
				}
			}
		}


		public void UpdateStepBoards(int width, int height)
		{
			CreateNewStepBoards(width, height);
			//foreach(BlockInstance bi in StepsData.BlockInstances)
			if (CurrentStep < 0)
				return;
			for (int i = 0; i < Data.Branches; i++)
			{
				if (!CurrentStepBoards[i].AddBlock(StepsData.BlockInstances[CurrentStep, i]))
				{
					throw new ArgumentException("Cannot add block!");
					// TODO: delete or add exceptions
					// and try/catch...
				}
			}
		}


		public void CreateNewStepBoards(int width, int height)
		{
			int currentStep = CurrentStep;
			if (currentStep < 0)
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					//CreateNewBoardFrom(currentStep, i, h);
					CurrentStepBoards[i] = new Board(width, height, new SortedList<BlockType, int>(AvailableBlocksSorted));
				}
			}
			else if (currentStep > 0)
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					if (StepsData.BlockInstances[currentStep, i].PreviousBlockBoardNumber != i)
					{
						//CreateNewBoardFrom(currentStep, i, h);
						//CurrentStepBoards[i] = CurrentStepBoards[StepsData.BlockInstances[currentStep, i].PreviousBlockBoardNumber]
						//	 .Copy();
						// we have to 
						CurrentStepBoards[i] = Board.CreateFromStepsData(StepsData, currentStep, i, 
							CurrentStepBoards[i].Width, CurrentStepBoards[i].Height, true, new SortedList<BlockType, int>(AvailableBlocksSorted));
					}
				}
			}
		}


		private void CreateNewBoardFrom(int step, int i, int h)
		{
			//Board board = new Board(Data.BoardWidth, h);
			//if (step >= 0)
			//{
			//	StepsData.SetStartingPoint(step, i);
			//	foreach (BlockInstance blockInstance in StepsData)
			//	{
			//		board.AddBlock(blockInstance);
			//	}
			//}
			CurrentStepBoards[i] = CurrentStepBoards[StepsData.BlockInstances[step, i]
				.PreviousBlockBoardNumber].Copy();
		}

		private void DivideSolutionsBetweenBoards(List<PartialSolution> solutions)
		{
			if (solutions.Count < 1)
				throw new ArgumentException("Too little solutions");
			int j = 0;
			for (int i = 0; i < CurrentStepBoards.Length; i++)
			{
				if (j == solutions.Count)
					j = 0;
				StepsData.BlockInstances[StepsData.LastStepFinished + 1, i] = solutions[j].Move;
				j++;
			}
		}


        /// <summary>
        /// Wybiera k z k^2 najlepszych rozwiązań
        /// </summary>
        /// <param name="solutions">Po k rozwiązań z k gałęzi</param>
        private void MergeSolutions(List<List<PartialSolution>> solutions)
	    {
			if(solutions.Count == 0)
				throw new ArgumentNullException("no solutions for us? :( ");
			int[] ind = new int[solutions.Count];
            for (int i = 0; i < solutions.Count; i++)
            {
                int min = Int32.MaxValue, minind = 0;
                for (int j = 0; j < solutions.Count; j++)
                {
                    if (solutions[j][ind[j]].Cost < min)
                    {
                        min = solutions[j][ind[j]].Cost;
                        minind = j;
                    }
                }
                StepsData.BlockInstances[StepsData.LastStepFinished + 1, i] = solutions[minind][ind[minind]].Move;
                ind[minind]++;
            }
	    }

	}
}
