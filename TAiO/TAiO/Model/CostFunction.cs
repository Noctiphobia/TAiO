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
	public class NamedCostFunction
	{
        /// <summary>
        /// Dana funkcja kosztu
        /// </summary>
		public CostFunction Function { get; set; }
        /// <summary>
        /// Nazwa funkcji
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="function">funkcja kosztu</param>
        /// <param name="name">nazwa funkcji kosztu</param>
		public NamedCostFunction(CostFunction function, string name)
		{
			Function = function;
			Name = name;
		}
	}
}
