﻿using System;
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
			// TODO: Delete all random trash BlockTypes
			Random rand = new Random();
			for (int i = 0; i < 50; i++)
			{
				//Data.Instance.OriginalBlockTypes.Add(new BlockType(1, 1, new int[1,1]) { BlockNumber = rand.Next(10, 30) });
			}
		}
	}
}
