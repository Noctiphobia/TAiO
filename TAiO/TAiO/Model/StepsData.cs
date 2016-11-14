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
	public class StepsData:IEnumerable<BlockInstance>
	{
		/// <summary>
		/// Array of BlockInstances
		/// first coordinate is a step number
		/// second coordinate is a board number
		/// in BlockInstance there's an info about on which board there was a previous block located,
		/// so we can draw all blocks 
		/// </summary>
		public BlockInstance[,] BlockInstances { get; private set; }

		protected const int StepNumberCoord = 0;
		protected const int BoardNumberCoord = 1;

		protected int startingI;
		protected int startingJ;
		protected int _lastStepFinished = -1;

		public int LastStepFinished
		{
			get
			{
				int last;
				lock (LockObject)
				{
					last = _lastStepFinished;
				}
				return last;
			}
		}

		public object LockObject { get; } = new object();

		public StepsData(int branches, int blocksNumber)
		{
			BlockInstances = new BlockInstance[blocksNumber, branches];
		}

		public void SetLastStepFinished(int step)
		{
			lock (LockObject)
			{
				_lastStepFinished = step;
			}

		}

		public void SetNewStepInfo(List<BlockInstance> stepBlockInstances)
		{
			if(stepBlockInstances.Count != BlockInstances.GetLength(BoardNumberCoord))
				throw new ArgumentException("sizes doesn't match");
			// TODO: create exceptions
			lock (LockObject)
			{
				_lastStepFinished++;
				for (int i = 0; i < BlockInstances.GetLength(BoardNumberCoord); i++)
				{
					BlockInstances[LastStepFinished, i] = stepBlockInstances[i];
				}
			}
		}

		public void SetStartingPoint(int stepNumber, int boardNumber)
		{
			int last;
			lock (LockObject)
			{
				last = _lastStepFinished;
			}
			startingI = stepNumber;
			startingJ = boardNumber;

			if (startingI > last)
				throw new ArgumentException("You're starting from wrong step number! Please check it before using...");
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
