using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Converters;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa reprezentująca studnię, w której układane są klocki
    /// </summary>
    class Board
    {
	    public int Width
	    {
	        get { return Content.GetLength(0); }
	    }
		public int Height
        {
            get { return Content.GetLength(1); }
        }
        public int[,] Content { get; set; }
        public int BlocksNumber { get; set; }
		private int StepHeight { get; set; } 

        public Board(int w, int h)
        {
			StepHeight = h / 2;
			Content = new int[w, h];
            BlocksNumber = 0;
        }

		private void Resize()
        {
			int[,] tmp = new int[Width, Height + StepHeight];
			for (int i = 0; i < Height; i++)
			    for (int j = 0; j < Width; j++)
			        tmp[i, j] = Content[i, j];
		    Content = tmp;
        }

        public bool AddBlock(BlockInstance block)
        {
			int h = (block.BlockVersion%2 == 0 ? block.Block.Height : block.Block.Width),
				w = (block.BlockVersion%2 == 0 ? block.Block.Width : block.Block.Height);
			while (block.Y + h > Height)
				Resize();
            BlocksNumber++;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
					if ((Content[i + block.X, j + block.Y] & block.Block.Shape[block.BlockVersion][i, j]) > 0)
                        return false;
					Content[i + block.X, j + block.Y] = block.Block.Shape[block.BlockVersion][i, j] * BlocksNumber;
                }
            return true;
        }
    }
}
