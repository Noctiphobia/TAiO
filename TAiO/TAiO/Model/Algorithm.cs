using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// Klasa zawierające wszystkie dane dotyczące przebiegu algorytmu
	/// i zawierająca metody pozwalające na wykonanie go
	/// </summary>
	class Algorithm
	{
		public StepsData StepsData { get; private set; }
		public int CurrentStep => StepsData.LastStepFinished;
		private Data Data;
		private List<Board> CurrentStepBoards;
		private List<KeyValuePair<BlockType, int>> BlocksOfTypeCount; 

		public Algorithm(Data data)
		{
			Data = data;
			BlocksOfTypeCount = new List<KeyValuePair<BlockType, int>>
				(Data.Blocks.ConvertAll((a => new KeyValuePair<BlockType, int>(a, (int)a.BlockNumber))));
			StepsData = new StepsData(Data.Branches, Data.Blocks.Sum(a => (int)a.BlockNumber));
		}

		public void RunAlgorithm()
		{
			//Task.Run(() =>
			//{
				int availableBlocks = Data.Blocks.Sum(a => (int)a.BlockNumber);
				while (StepsData.LastStepFinished < availableBlocks - 1)
				{
					MakeNextStep();
				}
			//});
		}

		public void MakeNextStep()
		{
			// TODO: make it somewhat smarter...
			BlockType nextBlock = null;
			for (int i = 0; i < BlocksOfTypeCount.Count; i++)
			{
				if (BlocksOfTypeCount[i].Value != 0)
				{
					nextBlock = BlocksOfTypeCount[i].Key;
					BlocksOfTypeCount[i] = new KeyValuePair<BlockType, int>(nextBlock, BlocksOfTypeCount[i].Value - 1);
					break;
				}
			}
			if (nextBlock == null)
				return;

			List<BlockInstance> blocks = new List<BlockInstance>(Data.Branches);

			if(CurrentStep > -1)
				StepsData.SetStartingPoint(CurrentStep, 0);
			int y = CurrentStep > -1 ? StepsData.Sum(blockInstance => blockInstance.Block.Height) : 0;

			for (int i = 0; i < Data.Branches; i++)
			{
				blocks.Add(new BlockInstance()
				{
					Block = nextBlock,
					BlockVersion = 0,
					X = 0,
					Y = y,
					PreviousBlockBoardNumber = i});
				//y += blocks[i].Block.Height;
			}

			StepsData.SetNewStepInfo(blocks);


		}



	}
}
