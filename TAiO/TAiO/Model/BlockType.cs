using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MicroMvvm;

namespace TAiO.Model
{
	public class BlockType:ObservableObject
	{
		private int _height;
		private int _width;
		private int _blockNumber;

		public int Height
		{
			get { return _height; }
			set
			{
				_height = value;
				RaisePropertyChanged("Height");
			}
		}

		public int Width
		{
			get { return _width; }
			set
			{
				_width = value;
				//OnPropertyChanged("Width");
			}
		}

		public int BlockNumber
		{
			get { return _blockNumber; }
			set
			{
				_blockNumber = value;
				RaisePropertyChanged("BlockNumber");
				if(_blockNumber == 90)
					MessageBox.Show("Block Number changed!!!");
			}
		}
	}
}
