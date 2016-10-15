using System;
using System.Collections.Generic;
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
		public BrowserViewModel()
		{
			Random rand = new Random();
			for (int i = 0; i < 50; i++)
			{
				Data.Instance.OriginalBlockTypes.Add(new BlockType() { BlockNumber = rand.Next(10, 30) });
			}
		}
	}
}
