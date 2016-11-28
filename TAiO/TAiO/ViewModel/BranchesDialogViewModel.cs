using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAiO.Model;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący wyboru liczby rozgałęzień.
	/// </summary>
	public class BranchesDialogViewModel : BaseViewModel
	{
		private CostFunction _costFunction;
		/// <summary>
		/// Wybrana funkcja kosztu.
		/// </summary>
		public CostFunction CostFunction
		{
			get { return _costFunction; }
			set
			{
				_costFunction = value;
				RaisePropertyChanged(nameof(CostFunction));
				RaisePropertyChanged(nameof(IsCostFunctionSelected));
			}
		}
		/// <summary>
		/// Czy została wybrana funkcja kosztu?
		/// </summary>
		public bool IsCostFunctionSelected => CostFunction != null;

		/// <summary>
		/// Liczba rozgałęzień w algorytmie
		/// </summary>
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
