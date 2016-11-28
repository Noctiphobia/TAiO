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
        /// <summary>
        /// Jedyna instancja klasy Data
        /// </summary>
		private static Data _instance;
        
        /// <summary>
        /// Zestaw wczytanych klocków wraz z oryginalnymi (ustawionymi w Browserze)
        /// wartościami BlocksNumber (liczba dostęponych klocków)
        /// </summary>
		public List<BlockType> Blocks { get; set; }

        /// <summary>
        /// Szerokość planszy (wczytywana z pliku lub ustawiana przez użytkownika)
        /// </summary>
		public int BoardWidth { get; set; }
        /// <summary>
        /// Liczba rozgałęzień algorytmu
        /// </summary>
		public int Branches { get; set; }

        /// <summary>
        /// Liczba klocków wybieranych losowo z wczytanego zestawu
        /// </summary>
		public uint RandomBlocksNumber { get; set; } = 20;
        /// <summary>
        /// Liczba różnych klocków wybieranych jako [DifferentBlocksNumber] pierwszych z zestawu
        /// </summary>
		public uint DifferentBlocksNumber { get; set; } = 20;
		/// <summary>
		/// Liczba klocków każdego typu (bierzemy po [AllBlocksNumber] klocków każdego typu)
		/// </summary>
		public uint EachBlockTypeNumber { get; set; } = 1;


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
