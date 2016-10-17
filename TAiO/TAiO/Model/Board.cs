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
            int h = (block.RotationNum%2 == 0 ? block.Block.Height : block.Block.Width),
                w = (block.RotationNum%2 == 0 ? block.Block.Width : block.Block.Height);
            if (block.LeftUpperCornerY + h > Height)
                Resize(2 * Height);
            BlocksNumber++;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    if ((Content[i + block.LeftUpperCornerX, j + block.LeftUpperCornerY] & block.Block.Shape[block.RotationNum][i, j]) > 0)
                        return false;
                    Content[i + block.LeftUpperCornerX, j + block.LeftUpperCornerY] = block.Block.Shape[block.RotationNum][i, j] * BlocksNumber;
                }
            return true;
        }
    }
}
