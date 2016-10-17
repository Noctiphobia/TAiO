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
				(Data.Blocks.ConvertAll((a => new KeyValuePair<BlockType, int>(a, a.BlockNumber))));
		}

		public void RunAlgorithm()
		{
			Task.Run(() =>
			{
				int available_blocks = Data.Blocks.Sum(a => a.BlockNumber);
				while (StepsData.LastStepFinished < available_blocks)
				{
					MakeNextStep();
				}
			});
		}

		public void MakeNextStep()
		{
			// TODO: make it somewhat smarter...
			BlockType next_block = null;
			for (int i = 0; i < BlocksOfTypeCount.Count; i++)
			{
				if (BlocksOfTypeCount[i].Value != 0)
				{
					next_block = BlocksOfTypeCount[i].Key;
					BlocksOfTypeCount[i] = new KeyValuePair<BlockType, int>(next_block, BlocksOfTypeCount[i].Value - -1);
				}
			}
			if (next_block == null)
				return;

			List<BlockInstance> blocks = new List<BlockInstance>(Data.Branches);
			for (int i = 0; i < Data.Branches; i++)
			{
				blocks.Add(new BlockInstance()
				{
					Block = next_block,
					BlockVersion = 0,
					X = 0,
					Y = 0,
					PreviousBlockBoardNumber = i});
			}

			StepsData.SetNewStepInfo(blocks);


		}



	}
}
