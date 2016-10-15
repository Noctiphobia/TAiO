using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;

namespace TAiO.Model
{
    /// <summary>
    /// 
    /// </summary>
	public class BlockType
	{
		public int Height { get; set; }
		public int Width { get; set; }
        public int[,] Shape { get; set; }
		public int BlockNumber { get; set; }

	    public BlockType(int h, int w, int[,] s)
	    {
	        Height = h;
	        Width = w;
	        Shape = s;
	        BlockNumber = 1;
	    }

	}
}
