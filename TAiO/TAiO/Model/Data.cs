using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public bool PreviewGoingOn { get; set; } = true;
		public ObservableCollection<BlockType> OriginalBlockTypes { get; set; }

		/// <summary>
		/// Jedyna instancja danych w całym programie.
		/// </summary>
		public static Data Instance => _instance ?? new Data();
		


		private Data()
		{
			_instance = this;
			OriginalBlockTypes = new ObservableCollection<BlockType>();
		}
	}
}
