using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TAiO.Model
{
	
	/// <summary>
	/// Klasa zawierająca wszystkie wykorzystane funkcje kosztu wraz z łatwym dostępem do ich listy.
	/// </summary>
	public class CostFunctionFactory
	{
		
		/// <summary>
		/// Wszystkie dostępne funkcje.
		/// </summary>
		public static List<NamedFunction> AvailableFunctions { get; } = new List<NamedFunction>
		{
			new NamedFunction((b) =>
			{
				int cost = 0;
				//znalezienie najwyższego punktu w każdej kolumnie
				int[] maxHeights = new int[b.Width];
				for (int x = 0; x < b.Width; ++x)
					for (int y = b.Height - 1; y > 0; --y)
						if (b[x, y] > 0)
						{
							maxHeights[x] = y;
							break;
						}
					

				for (int x = 0; x < b.Width; ++x)
				{
					for (int y = 0; y < maxHeights[x]; ++y)
					{
						if (b[x, y] == 0) cost++;
					}
				}
				return cost;
			}, "Najmniej dziur"), //dziura = wszystko, co jest puste i ma nad sobą klocka
			new NamedFunction((b) =>
			{
				int cost = 0;
				for (int x = 0; x < b.Width; ++x)
					for (int y = b.Height - 1; y > cost; --y)
						if (b[x, y] > 0)
							cost = y;

				return cost;
			}, "Najmniejsza wysokość"),
			new NamedFunction((b) =>
			{
				int cost = 0;
				//znalezienie najwyższego punktu w każdej kolumnie
				int[] maxHeights = new int[b.Width];
				for (int x = 0; x < b.Width; ++x)
					for (int y = b.Height - 1; y > 0; --y)
						if (b[x, y] > 0)
						{
							maxHeights[x] = y;
							break;
						}
				int fullsquares = 0;
				for (int x = 0; x < b.Width; x++)
				{
					for (int y = 0; y < b.Height; y++)
					{
						if (b[x, y] > 0)
							fullsquares++;
					}
				}

				for (int x = 0; x < b.Width; ++x)
				{
					for (int y = 0; y < maxHeights[x]; ++y)
					{
						if (b[x, y] == 0) cost++;
					}
				}
				//return cost - fullsquares;
				if (fullsquares == 0)
					return 0;
				return cost/fullsquares; // fullsquares > 0
			}, "Najmniej dziur II"), //koszt = liczba_dziur/zapełnione_pola - bezużyteczna, ale tworzy fajny ażurowy wzorek :)
		};



		private CostFunctionFactory() { }
	}
}
