using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAiO.Model;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący przeglądarki klocków.
	/// </summary>
	public class BrowserViewModel : BaseViewModel
	{
		public ObservableCollection<BlockTypeViewModel> BlockTypeViewModels { get; set; }

		public BrowserViewModel()
		{
			RefreshBlockTypeViewModelsList(new List<BlockType>());
		}

		public BrowserViewModel(List<BlockType> blockTypes)
		{
			RefreshBlockTypeViewModelsList(blockTypes);
		}

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
