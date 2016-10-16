namespace TAiO.Model
{
	public class Array2D
	{
		public Array2D(int[,] array)
		{
			Array = array;
		}

		public int[,] Array { get; set; }
		public int Width => Array.GetLength(0);
		public int Height => Array.GetLength(1);
	}
}