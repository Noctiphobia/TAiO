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
		
		// TODO: Delete this trash :P
		public static BlockType bt = new BlockType(1, 1, new[,] { { 1 }});
		public static BlockTypeViewModel btvm = new BlockTypeViewModel(bt);
		public List<BlockTypeViewModel> list { get; set; } = new List<BlockTypeViewModel>() {btvm};

		private Data()
		{
			_instance = this;
			Blocks = new List<BlockType>();
			Branches = 3;
		}
	}
}
