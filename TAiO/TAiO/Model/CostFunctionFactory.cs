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
			
		};



		private CostFunctionFactory() { }
	}
}
