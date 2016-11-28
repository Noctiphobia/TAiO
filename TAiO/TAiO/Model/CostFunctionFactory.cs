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
	public static class CostFunctionFactory
	{
		
		/// <summary>
		/// Wszystkie dostępne funkcje.
		/// </summary>
		public static List<NamedCostFunction> AvailableFunctions { get; } = new List<NamedCostFunction>
		{
			new NamedCostFunction((b) =>
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
						new NamedCostFunction((b) =>
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
				int avg = (int) Math.Round(maxHeights.Average());
				for(int i=0; i<b.Width; ++i)
					if (maxHeights[i] < avg)
						maxHeights[i] = avg;
				for (int x = 0; x < b.Width; ++x)
				{
					for (int y = 0; y < maxHeights[x]; ++y)
					{
						if (b[x, y] == 0) cost++;
					}
				}
				return cost;
			}, "Najmniej dziur ze średnią"), //dziura = wszystko, co jest puste i ma nad sobą klocka
			new NamedCostFunction((b) =>
			{
				int cost = 0;
				for (int x = 0; x < b.Width; ++x)
					for (int y = b.Height - 1; y >= cost; --y)
						if (b[x, y] > 0)
							cost = y + 1;

				return cost;
			}, "Najmniejsza wysokość"),
			new NamedCostFunction((b) =>
			{
				int cost = 0;
				for (int x = 0; x < b.Width; ++x)
					for (int y = 0; y < b.Height; ++y)
					{
						if (b[x,y] > 0){
							cost -= new bool[]
							{
								x == 0,			//po lewej ściana
								x == b.Width-1,	//po prawej ściana
								y == 0,			//na dole ściana
								//x > 0 && b[x-1,y] != 0 && b[x-1, y] != b[x,y],				//po lewej klocek
								x < b.Width - 1 && b[x+1, y] != 0 && b[x+1, y] != b[x,y], 	//po prawej klocek
								//y > 0 && b[x, y-1] != 0 && b[x, y-1] != b[x,y], 			//na dole klocek
								y < b.Height - 1 && b[x, y+1] != 0 && b[x, y+1] != b[x,y] 	//na górze klocek
							}.Count(t=>t);
						}			
					}
				return cost;
			}, "Największa przyległość"),
			new NamedCostFunction((b) =>
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
			}, "Najładniejsze puste pola"), //koszt = liczba_dziur/zapełnione_pola - bezużyteczna, ale tworzy fajny ażurowy wzorek :)
		};
	}
}
