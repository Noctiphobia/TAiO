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
		private Board[] CurrentStepBoards;
		private List<List<BlockType>> CurrentStepBoardsBlocks;
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
                MergeSolutions(new List<List<PartialSolution>> {solutions});
				//DivideSolutionsBetweenBoards(solutions);
			}
			else
			{
				var correctIndices = new List<int>();

				for (int i = 0; i < Data.Branches; ++i)												//odrzucanie zduplikowanych bloków
				{
					bool take = true;
					for (int j = 0; j < i; ++j)
					{
						if (StepsData.BlockInstances[StepsData.LastStepFinished, j].Equals(			//wstępne odsiewanie przez ostatni ruch
							StepsData.BlockInstances[StepsData.LastStepFinished, i]))
						{
							if (CurrentStepBoards[j].Equals(CurrentStepBoards[i]))					//dokładne porównanie tablic
							{
								take = false;
								break;
							}
						}
					}
					if (take) 
						correctIndices.Add(i);
				}

				var tasks = new Task[correctIndices.Count];
				var partialSolutions = new List<PartialSolution>[tasks.Length];

				for (int i = 0; i < correctIndices.Count; i++)
				{
					int j = correctIndices[i];
					(tasks[i] =
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
						throw new ArgumentException("Cannot add block! (UpdateStepBoardsNow())");
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
							throw new ArgumentException("Cannot add block! (UpdateStepBoardsNow())");
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
			if (CurrentStep < 0)
				return;
			for (int i = 0; i < Data.Branches; i++)
			{
				if (!CurrentStepBoards[i].AddBlock(StepsData.BlockInstances[CurrentStep, i]))
				{
					throw new ArgumentException("Cannot add block! (UpdateStepBoards())");
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
					CurrentStepBoards[i] = new Board(width, height, new SortedList<BlockType, int>(AvailableBlocksSorted));
				}
			}
			else if (currentStep > 0)
			{
				for (int i = 0; i < Data.Branches; i++)
				{
					if (StepsData.BlockInstances[currentStep, i].PreviousBlockBoardNumber != i)
					{
						CurrentStepBoards[i] = Board.CreateFromStepsData(StepsData, currentStep, i, 
							CurrentStepBoards[i].Width, CurrentStepBoards[i].Height, true, new SortedList<BlockType, int>(AvailableBlocksSorted));
					}
				}
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
            for (int i = 0; i < Data.Branches; i++)
            {
                int min = Int32.MaxValue, minind = -1;
                for (int j = 0; j < solutions.Count; j++)
                {
                    if (ind[j]<solutions[j].Count && solutions[j][ind[j]].Cost < min)
                    {
                        min = solutions[j][ind[j]].Cost;
                        minind = j;
                    }
                }
	            if (minind == -1)	//jeśli zabrakło nam danych
	            {
		            for (; i < Data.Branches; ++i)	//dopełniamy pozostałe najlepszym rozwiązaniem i kończymy
		            {
			            StepsData.BlockInstances[StepsData.LastStepFinished + 1, i] =
				            StepsData.BlockInstances[StepsData.LastStepFinished + 1, 0];
		            }
		            break;
	            }
	            StepsData.BlockInstances[StepsData.LastStepFinished + 1, i] = solutions[minind][ind[minind]].Move;
                ind[minind]++;
            }
	    }

	}
}
