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
	public class BlockType:IComparable
    {

	    private static int NextId = 0;

		public int Id { get; private set; }
	    public readonly int FullSquares;

		public int Height { get; set; }
		public int Width { get; set; }
        public List<int[,]> Shape { get; set; }
		public uint BlockNumber { get; set; }

	    public BlockType(int w, int h, int[,] s)
	    {
		    Id = NextId;
		    NextId++;
	        Height = h;
	        Width = w;
		    Shape = new List<int[,]> {s};
		    CreateRotations90();
	        BlockNumber = 1;
		    FullSquares = Shape.Select(a => a.Cast<int>().Sum()).Sum();
	    }

        private void CreateRotations90()
        {
            int[,] last = Shape[0];
            for (int i = 1; i < 4; i++)
            {
                last = Rotate90(last);
	            bool identical = false;
                for (int j = 0; j < i; j++)
	                if (CompareArrays(Shape[j], last)) // jeśli są takie same
	                {
		                identical = true;
		                break;
	                }
	            if(!identical)
					Shape.Add(last);
            }
	        if (Height > Width)		//zamieniamy, żeby zawsze był szerszy
	        {
		        int tmp = Height;
		        Height = Width;
		        Width = tmp;
				Shape.Add(Shape[0]);
				Shape.RemoveAt(0);
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

	    public override string ToString()
	    {
		    return "BT: Id = " + this.Id;
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



	    public int CompareTo(object obj)
	    {
			//return this.Id.CompareTo(obj as )
		    BlockType bt = obj as BlockType;
		    if (bt == null)
		    {
			    return 1;
		    }
		    return this.Id.CompareTo(bt.Id);


		    if (ReferenceEquals(this, obj))
		    {
			    return 0;
		    }
		    int a;
		    if ((a = this.GetHashCode().CompareTo(obj.GetHashCode())) != 0)
		    {
			    return a;
		    }
		    BlockType b = (BlockType) obj;
		    if ((a = this.Height.CompareTo(b.Height)) != 0)
		    {
			    return a;
		    }
			if ((a = this.Width.CompareTo(b.Width)) != 0)
			{
				return a;
			}
			//return this.Shape.
		    return 0;

	    }
	}
}
