using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący wyboru liczby rozgałęzień.
	/// </summary>
	public class BranchesDialogViewModel : BaseViewModel
	{
		public int Branches
		{
			get { return Data.Branches; }
			set
			{
				Data.Branches = value;
				RaisePropertyChanged(nameof(Branches));
			}
		}
	}
}
