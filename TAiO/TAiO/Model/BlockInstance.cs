using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	struct BlockInstance
	{

		public int BlockId; // or BlockType
		public int BlockVersion;

		public int LeftUpperCornerX;
		public int LeftUpperCornerY;


		public int PreviousBlockCoord;

	}
}
