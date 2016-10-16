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

        public bool AddBlock(BlockInstance block)
        {
            if (block.LeftUpperCornerY + (block.Rotation.Num % 2 == 0 ? block.Block.Height : block.Block.Width) > Height)
                Resize();
            BlocksNumber++;
            for (int i = 0; i < block.Block.Height; i++)
                for (int j = 0; j < block.Block.Width; j++)
                {
                    if ((Content[i + block.LeftUpperCornerX, j + block.LeftUpperCornerY] & block.Block.Shape[i, j]) > 0)
                        return false;
                    Content[i + block.LeftUpperCornerX, j + block.LeftUpperCornerY] = block.Block.Shape[i, j] * BlocksNumber;
                }
            return true;
        }
    }
}
