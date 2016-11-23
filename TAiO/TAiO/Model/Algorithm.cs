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
        /// <summary>
        /// Zmienna zawierająca zminimalizowane dane kroków algorytmu
        /// </summary>
		public StepsData StepsData { get; private set; }
        /// <summary>
        /// Ostatni ukończony krok
        /// </summary>
		public int CurrentStep => StepsData.LastStepFinished;
        /// <summary>
        /// Dane algorytmu
        /// </summary>
        private Data Data;
        /// <summary>
        /// Funkcja kosztu
        /// </summary>
		private CostFunction CostFunction;
        /// <summary>
        /// Funkcja położenia
        /// </summary>
        private PlacementFunction PlacementFunction;
        /// <summary>
        /// Aktualne plansze, na których działa algorytm
        /// </summary>
        private Board[] CurrentStepBoards;
        /// <summary>
        /// Słownik par klocek i liczba dostępnych klocków tego typu
        /// (kopiowanie ze słownika jest prostsze i szybsze niż kopiowanie z listy)
        /// </summary>
		private SortedList<BlockType, int> AvailableBlocksSorted;
		

        /// <summary>
        /// Konstruktor klasy Algorithm
        /// </summary>
        /// <param name="data">dane algorytmu</param>
        /// <param name="costFunction">funkcja kosztu</param>
        /// <param name="placementFunction">funkcja położenia</param>
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

        /// <summary>
        /// Funkcja wykonująca wszystkie kroki algorytmu
        /// </summary>
		public void RunAlgorithm()
		{
				int availableBlocks = Data.Blocks.Sum(a => (int)a.BlockNumber);
				while (StepsData.LastStepFinished < availableBlocks - 1)
				{
					MakeNextStep();
				}
		}
        /// <summary>
        /// Funkcja wykonująca kolejny krok algorytmu
        /// </summary>
		public void MakeNextStep()
		{

			UpdateStepBoards(Data.BoardWidth, Data.BoardWidth);

			if (CurrentStep < 0 && CurrentStepBoards.Length > 0)
			{
				List<PartialSolution> solutions = (CurrentStepBoards[0].ChooseBlocks(Data.Branches, CostFunction, PlacementFunction));
                MergeSolutions(new List<List<PartialSolution>> {solutions});
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
				MergeSolutions(partialSolutions.ToList());
			}

            int currentStep = StepsData.LastStepFinished;
			StepsData.LastStepFinished = currentStep + 1;

		}

        /// <summary>
        /// Funkcja aktualizująca zawartość plansz na podstawie danych ze StepsData
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
		public void UpdateStepBoards(int width, int height)
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
						throw new ArgumentException("Cannot add block (UpdateStepBoardsNow())");
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
						}
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
