using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa porównujaca dwa obiekty klasy PartialSolution
    /// </summary>
	class PartialSolutionsComparer:IComparer<PartialSolution>
	{
		public int Compare(PartialSolution x, PartialSolution y)
		{
			if (x.Cost < y.Cost)
				return -1;
			else return 1;
		}
	}
}
