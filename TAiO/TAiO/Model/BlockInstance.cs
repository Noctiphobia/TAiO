using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// BlockInstance is a helper class for showing every step of algorithm.
	/// 
	/// Since remembering steps in the way algorithm needs to see them (in arrays of ints)
	/// is too memory greedy and gives us no other advantages,
	/// we chose to remember previous steps in array of blocks added in a specific step.
	/// 
	/// BlockInstance correspond with one block added in one specific step on one specific board.
	/// </summary>
	public struct BlockInstance
	{
		/// <summary>
		/// Type of block
		/// </summary>
		public BlockType Block { get; set; }
		/// <summary>
		/// version of block (rotation)
		/// not called "rotation", because there can be four or two or only one not duplicating rotation,
		/// so for example "2" can easily meaning not the same rotation in two other situation -
		/// 90 or 180 degrees
		/// </summary>
		public int BlockVersion { get; set; }
		/// <summary>
		/// X coordinate of position of left upper corner of that block
		/// </summary>
		public int X { get; set; }
		/// <summary>
		/// Y coordinate of position of left upper corner of that block
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Id number of board of previous block in that path in algorithm
		/// </summary>
		public int PreviousBlockBoardNumber { get; set; }


		public override string ToString()
		{
			return Block.ToString() + ", vers: " + BlockVersion + ", prev = " + PreviousBlockBoardNumber + ", ("+ X + ", " + Y+")";
		}
	}
}
