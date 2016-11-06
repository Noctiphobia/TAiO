using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
		private List<Board> CurrentStepBoards;
		private List<KeyValuePair<BlockType, int>> BlocksOfTypeCount; 

		public Algorithm(Data data, CostFunction costFunction)
		{
			Data = data;
			CostFunction = costFunction;
			CurrentStepBoards = new List<Board>();
			BlocksOfTypeCount = new List<KeyValuePair<BlockType, int>>
				(Data.Blocks.ConvertAll((a => new KeyValuePair<BlockType, int>(a, (int)a.BlockNumber))));
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
			// TODO: make it somewhat smarter...
			//BlockType nextBlock = null;
			//for (int i = 0; i < BlocksOfTypeCount.Count; i++)
			//{
			//	if (BlocksOfTypeCount[i].Value != 0)
			//	{
			//		nextBlock = BlocksOfTypeCount[i].Key;
			//		BlocksOfTypeCount[i] = new KeyValuePair<BlockType, int>(nextBlock, BlocksOfTypeCount[i].Value - 1);
			//		break;
			//	}
			//}
			//if (nextBlock == null)
			//	return;

			//List<BlockInstance> blocks = new List<BlockInstance>(Data.Branches);

			//if(CurrentStep > -1)
			//	StepsData.SetStartingPoint(CurrentStep, 0);
			//int y = CurrentStep > -1 ? StepsData.Sum(blockInstance => blockInstance.Block.Height) : 0;

			//for (int i = 0; i < Data.Branches; i++)
			//{
			//	blocks.Add(new BlockInstance()
			//	{
			//		Block = nextBlock,
			//		BlockVersion = 0,
			//		X = 0,
			//		Y = y,
			//		PreviousBlockBoardNumber = i});
			//	//y += blocks[i].Block.Height;
			//}

			//StepsData.SetNewStepInfo(blocks);
		    var tasks = new Task[CurrentStepBoards.Count];
			var partialSolutions = new List<List<PartialSolution>>(CurrentStepBoards.Count);

			for (int i = 0; i < CurrentStepBoards.Count; i++)
				(tasks[i] =
					new Task(() => { partialSolutions[i] = (CurrentStepBoards[i].ChooseBlocks(Data.Blocks, Data.Branches, CostFunction)); })).Start();
			
			Task.WaitAll(tasks);
            MergeSolutions(partialSolutions);
            //TODO: jakieś ruszenie tego kroku czy coś
        }

        /// <summary>
        /// Wybiera k z k^2 najlepszych rozwiązań
        /// </summary>
        /// <param name="solutions">Po k rozwiązań z k gałęzi</param>
        private void MergeSolutions(List<List<PartialSolution>> solutions)
	    {
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
