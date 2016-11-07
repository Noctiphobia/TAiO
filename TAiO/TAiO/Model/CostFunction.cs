using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// Funkcje liczące koszt ułożenia (im mniejszy, tym lepiej!) danego ułożenia klocków na planszy
	/// </summary>
	/// <param name="board">Plansza, zawierająca ułożenie</param>
	/// <returns>Koszt</returns>
	public delegate int CostFunction(Board board);

	/// <summary>
	/// Klasa zawierająca funkcję kosztu i jej nazwę.
	/// </summary>
	public class NamedFunction
	{
		public CostFunction Function { get; set; }
		public string Name { get; set; }

		public NamedFunction(CostFunction function, String name)
		{
			Function = function;
			Name = name;
		}
	}
}
