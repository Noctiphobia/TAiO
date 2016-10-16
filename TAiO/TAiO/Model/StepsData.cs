using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// StepsData contains all data needed to draw all steps of algorithm on all boards
	/// </summary>
	class StepsData:IEnumerable<BlockInstance>
	{
		/// <summary>
		/// Array of BlockInstances
		/// first coordinate is a step number
		/// second coordinate is a board number
		/// in BlockInstance there's an info about on which board there was a previous block located,
		/// so we can draw all blocks 
		/// </summary>
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
				j = BlockInstances[i, j].PreviousBlockBoardNumber;
				yield return BlockInstances[i, j];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
