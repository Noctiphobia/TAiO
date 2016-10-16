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
	/// ViewModel dla klasy BlockType, dzięki któremu możemy obserwować zmiany w BlockNumber
	/// </summary>
	class BlockTypeViewModel:ObservableObject
	{
		/// <summary>
		/// Obiekt typu BlockType, w którym zmieniamy BlockNumber (liczbę klocków danego typu rozważanych w algorytmie)
		/// </summary>
		public BlockType Block { get; set; }

		/// <summary>
		/// Konstruktor ViewModelu, przyjmujący obiekt typu BlockType
		/// </summary>
		/// <param name="block">obiekt, w którym zmieniamy BlockNumber (liczbę klocków danego typu)</param>
		public BlockTypeViewModel(BlockType block)
		{
			Block = block;
		}
		/// <summary>
		/// Liczba klocków danego typu - interfejs dla widoku
		/// "set" wywołuje event PropertyChanged
		/// </summary>
		public int BlockNumber
		{
			get { return Block.BlockNumber; }
			set
			{
				Block.BlockNumber = value; 
				RaisePropertyChanged("BlockNumber");
			}
		}
	}
}
