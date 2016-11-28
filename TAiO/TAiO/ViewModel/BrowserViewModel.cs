using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MicroMvvm;
using TAiO.Model;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący przeglądarki klocków.
	/// </summary>
	public class BrowserViewModel : BaseViewModel
	{
		public ObservableCollection<BlockTypeViewModel> BlockTypeViewModels { get; set; }
		protected Random Random = new Random();

		/// <summary>
		/// Konstruktor
		/// </summary>
		public BrowserViewModel()
		{
			RefreshBlockTypeViewModelsList(new List<BlockType>());
		}


		/// <summary>
		/// Ustaw losowo liczby typów klocków tak, aby w sumie dawały [Data.RandomBlocksNumber]
		/// </summary>
		public ICommand GetRandomBlocks => new RelayCommand(() =>
		{
			int sum = (int)Data.RandomBlocksNumber;
			int maxBlockNumber = BlockTypeViewModels.Count;
			uint[] blocks = new uint[maxBlockNumber];

			for (int i = 0; i < sum; i++)
			{
				int next = Random.Next(maxBlockNumber);
				blocks[next]++;
			}
			for (int i = 0; i < BlockTypeViewModels.Count; i++)
			{
				BlockTypeViewModels[i].BlockNumber = blocks[i];
			}
		});

		/// <summary>
		/// Ustaw liczbę klocków każdego typu na 1
		/// </summary>
		public ICommand ResetBlocksNumbers => new RelayCommand(() =>
		{
			for (int i = 0; i < BlockTypeViewModels.Count; i++)
			{
				BlockTypeViewModels[i].BlockNumber = 1;
			}
		});
		/// <summary>
		/// Ustaw liczbę klocków każdego typu tak,
		/// aby do algorytmu wzięte było [Data.DifferentBlocksNumber] pierwszych klocków z zestawu
		/// </summary>
		public ICommand GetNDifferentBlocks => new RelayCommand(() =>
		{
			int maxBlockNumber = BlockTypeViewModels.Count;

			int gotBlocksMaxNumber = (int)Math.Min(Data.DifferentBlocksNumber, maxBlockNumber);

			for (int i = 0; i < gotBlocksMaxNumber; i++)
			{
				BlockTypeViewModels[i].BlockNumber = 1;
			}

			if (Data.DifferentBlocksNumber < maxBlockNumber)
			{
				for (int i = (int)Data.DifferentBlocksNumber; i < maxBlockNumber; i++)
				{
					BlockTypeViewModels[i].BlockNumber = 0;
				}
			}

		});

		/// <summary>
		/// Ustaw liczbę klocków każdego typu na [Data.EachBlockTypeNumber]
		/// </summary>
		public ICommand GetNEachBlock => new RelayCommand(() =>
		{
			for (int i = 0; i < BlockTypeViewModels.Count; i++)
			{
				BlockTypeViewModels[i].BlockNumber = Data.EachBlockTypeNumber;
			}
		});

		/// <summary>
		/// Odśwież widok listy dostępnych klocków
		/// </summary>
		/// <param name="blockTypes">typy klocków</param>
		public void RefreshBlockTypeViewModelsList(List<BlockType> blockTypes)
		{
			BlockTypeViewModels = new ObservableCollection<BlockTypeViewModel>();
			foreach (BlockType blockType in blockTypes)
			{
				BlockTypeViewModels.Add(new BlockTypeViewModel(blockType));
			}
			RaisePropertyChanged(nameof(BlockTypeViewModels));
		}

	}
}
