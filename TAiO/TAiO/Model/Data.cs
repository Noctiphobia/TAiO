using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// Klasa zawierająca dane wejściowe problemu i parametry algorytmu oraz metody pozwalające na uruchomienie algorytmu. 
	/// </summary>
	public class Data
	{
		private static Data _instance;




		/// <summary>
		/// Jedyna instancja danych w całym programie.
		/// </summary>
		public static Data Instance => _instance ?? new Data();
		

		public int Test { get; set; }

		private Data()
		{
			_instance = this;
		}
	}
}
