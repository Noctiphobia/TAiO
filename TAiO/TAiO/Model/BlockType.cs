using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;

namespace TAiO.Model
{
    /// <summary>
    /// A class representing a type of block and how many blocks of this type do we have
    /// </summary>
	public class BlockType
	{
		public int Height { get; set; }
		public int Width { get; set; }
        public List<Array2D> Shape { get; set; }
		public int BlockNumber { get; set; }

	    public BlockType(int w, int h, int[,] s)
		{
	        Height = h;
	        Width = w;
            Shape = new List<Array2D>();
	        Shape.Add(new Array2D(s));
	        CreateRotations90();
	        BlockNumber = 1;
		}

        private void CreateRotations90()
        {
            int[,] last = Shape[0].Array;
            for (int i = 1; i < 4; i++)
            {
                last = Rotate90(last);
	            bool identical = false;
                for (int j = 0; j < i; j++)
	                if (CompareArrays(Shape[j].Array, last)) // jeśli są takie same
	                {
		                identical = true;
		                break;
	                }
	            if(!identical)
					Shape.Add(new Array2D(last));
            }
        }

        private int[,] Rotate90(int[,] t)
        {
            int[,] res = new int[t.GetLength(1), t.GetLength(0)];
            for (int i = 0; i < t.GetLength(0); i++)
                for (int j = 0; j < t.GetLength(1); j++)
                    res[t.GetLength(1) - 1 - j, i] = t[i, j];
            return res;
        }

        private bool CompareArrays(int[,] t1, int[,] t2)
        {
            if (t1.GetLength(0) != t2.GetLength(0) ||
                t1.GetLength(1) != t2.GetLength(1))
                return false;
            for (int i = 0; i < t1.GetLength(0); i++)
                for (int j = 0; j < t1.GetLength(1); j++)
                    if (t1[i, j] != t2[i, j])
                        return false;
            return true;
        }
    }
}
