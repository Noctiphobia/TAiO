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
	public class BlockTypeViewModel:ObservableObject
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
		public uint BlockNumber
		{
			get { return Block.BlockNumber; }
			set
			{
				Block.BlockNumber = value;
				RaisePropertyChanged("BlockNumber");
			}
		}

		/// <summary>
		/// Lista wariantów danego klocka (rotacji)
		/// </summary>
		public List<Array2D> Shape
        {
            get
	        {
	            var res = new List<Array2D>();
	            foreach (int[,] s in Block.Shape)
	            {
	                res.Add(new Array2D(s));
	            }
	            return res;
	        }
        }
	}
}
