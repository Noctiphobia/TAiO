using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// Funkcje znajdujące położenie dla zadanego klocka o zadanym obrocie na danej planszy.
	/// </summary>
	/// <param name="board">Plansza.</param>
	/// <param name="block">Klocek.</param>
	/// <param name="rotation">Obrót.</param>
	/// <returns></returns>
	public delegate List<BlockInstance> PlacementFunction(Board board, BlockType block, int rotation);

	/// <summary>
	/// Klasa zawierająca funkcję położenia i jej nazwę.
	/// </summary>
	public class NamedPlacementFunction
	{
		public PlacementFunction Function { get; set; }
		public string Name { get; set; }

		public NamedPlacementFunction(PlacementFunction function, string name)
		{
			Function = function;
			Name = name;
		}
	}
}
