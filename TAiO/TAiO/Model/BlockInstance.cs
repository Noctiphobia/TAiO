using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// BlockInstance to klasa pomocniczna do pokazywania kroków algorytmu.
	/// 
	/// Ponieważ zapamiętywanie kroków w taki sposób, w jaki algorytm potrzebuje je widzieć
	/// (tzn. w postaci tablic intów) jest zbyt pamięciożerne i nie daje nam żadnych dodatkowych korzyści,
	/// wybraliśmy zapamiętywanie poprzednich kroków w tablicy klocków dodanych w kolejnych krokach.
	/// 
	/// Obiekt BlockInstance reprezentuje jeden klocek dodany w jednym konkretnym kroku na jednej konkretnej tablicy.
	/// </summary>
	/// 
	public struct BlockInstance
	{
		/// <summary>
		/// Typ klocka
		/// </summary>
		public BlockType Block { get; set; }
		/// <summary>
		/// Wersja klocka (czyli obrót)
		/// </summary>
		public int BlockVersion { get; set; }
		/// <summary>
		/// Współrzędna X lewego górnego rogu klocka
		/// </summary>
		public int X { get; set; }
		/// <summary>
		/// Współrzędna Y prawego górnego rogu klocka
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Id planszy, na której został ułożony poprzedni klocek z tej ścieżki
		/// </summary>
		public int PreviousBlockBoardNumber { get; set; }


		public bool Equals(BlockInstance other)
		{
			return (Block.CompareTo(other.Block)==0) && BlockVersion == other.BlockVersion && X == other.X && Y == other.Y;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Block?.GetHashCode() ?? 0;
				hashCode = (hashCode*397) ^ BlockVersion;
				hashCode = (hashCode*397) ^ X;
				hashCode = (hashCode*397) ^ Y;
				return hashCode;
			}
		}

		public override string ToString()
		{
			return Block.ToString() + ", vers: " + BlockVersion + ", prev = " + PreviousBlockBoardNumber + ", ("+ X + ", " + Y+")";
		}
	}
}
