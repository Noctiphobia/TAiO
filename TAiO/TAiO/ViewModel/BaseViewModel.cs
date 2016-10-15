using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;
using TAiO.Model;

namespace TAiO.ViewModel
{
	/// <summary>
	/// Klasa bazowa dla ViewModeli reprezentujących cały widok. Pozwala na dostęp do danych algorytmu.
	/// </summary>
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Dane wejściowe i konfiguracja dla dostępu przez XAML.
		/// </summary>
		public Data Data => Data.Instance;

		// TODO: Delete this thrash (orphan BlockType)
		public BlockType BlockType { get; set; }

		public BaseViewModel()
		{
			BlockType = new BlockType() {BlockNumber = 100};
		}
	}
}
