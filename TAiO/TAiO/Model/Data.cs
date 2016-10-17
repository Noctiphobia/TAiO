using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAiO.ViewModel;

namespace TAiO.Model
{
	/// <summary>
	/// Klasa zawierająca dane wejściowe problemu i parametry algorytmu oraz metody pozwalające na uruchomienie algorytmu. 
	/// </summary>
	public class Data
	{
		private static Data _instance;

		public bool PreviewGoingOn { get; set; } = true;
		public List<BlockType> Blocks { get; set; }
        public int BoardWidth { get; set; }
		public int Branches { get; set; }


		/// <summary>
		/// Jedyna instancja danych w całym programie.
		/// </summary>
		public static Data Instance => _instance ?? new Data();
		

		private Data()
		{
			_instance = this;
			Blocks = new List<BlockType>();
			Branches = 3;
		}
	}
}
