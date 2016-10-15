using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;
using TAiO.Model;

namespace TAiO.ViewModel
{
	class BlockTypeViewModel:ObservableObject
	{
		public BlockType Block { get; set; }

		public BlockTypeViewModel(BlockType block)
		{
			Block = block;
		}
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
