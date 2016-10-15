using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	class StepsData:IEnumerable<BlockInstance>
	{

		public BlockInstance[,] BlockInstances { get; set; }
		private int startingI;
		private int startingJ;
		public int LastStepFinished { get; private set; }

		public StepsData(int branches, int blocksNumber)
		{
			BlockInstances = new BlockInstance[branches, blocksNumber];
		}


		public void SetStartingPoint(int stepNumber, int boardNumber)
		{
			startingI = stepNumber;
			startingJ = boardNumber;
		}
		public IEnumerator<BlockInstance> GetEnumerator()
		{
			int j = startingJ;
			for (int i = startingI; i >= 0; i--)
			{
				j = BlockInstances[i, j].PreviousBlockCoord;
				yield return BlockInstances[i, j];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
