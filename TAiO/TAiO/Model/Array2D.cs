namespace TAiO.Model
{
	/// <summary>
	/// Wrapper na tablicę intów w celu wykorzystania jej jako DataSource
	/// </summary>
	public class Array2D
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="array">tablica danych</param>
		public Array2D(int[,] array)
		{
			Array = array;
		}
		/// <summary>
		/// Tablica danych
		/// </summary>
		public int[,] Array { get; set; }
		public int Width => Array.GetLength(0);
		public int Height => Array.GetLength(1);
		/// <summary>
		/// Indeksator
		/// </summary>
		/// <param name="i">pierwsza współrzędna</param>
		/// <param name="j">druga współrzędna</param>
		/// <returns></returns>
		public int this[int i, int j]
		{
			get { return Array[i, j]; }
			set { Array[i, j] = value; }
		}
	}
}