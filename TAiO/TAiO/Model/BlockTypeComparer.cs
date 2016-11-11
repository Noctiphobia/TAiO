using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	class BlockTypeComparer:IComparer<BlockType>
	{
		public int Compare(BlockType x, BlockType y)
		{
			if (x.Height.CompareTo(y.Height) != 0)
				return x.Height.CompareTo(y.Height);
			return 0;
		}
	}
}
