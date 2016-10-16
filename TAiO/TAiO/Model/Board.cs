using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa reprezentująca studnię, w której układane są klocki
    /// </summary>
    class Board
    {
        public int Width { get; set; }
        public int[,] Content { get; set; }
        public int BlocksNumber { get; set; }
        public int Height { get; set; }

        public Board(int w, int h)
        {
            Width = w;
            BlocksNumber = 0;
        }

        private void Resize(int h)
        {

        }

        public void AddBlock(BlockInstance block)
        {
            /*if (block.LeftUpperCornerY + > Height)
            BlocksNumber++;
            for (int i = 0; i < block.Block.Height; i++)
                for (int j = 0; j < block.Block.Width; j++)*/

        }
    }
}
