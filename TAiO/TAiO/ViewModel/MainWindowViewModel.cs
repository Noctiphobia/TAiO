using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MicroMvvm;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący okna sterującego całym programem.
	/// </summary>
	public class MainWindowViewModel : BaseViewModel
	{
		private ICommand _showBrowserCommand;
		private Browser _browser;

		public ICommand ShowBrowserCommand
		{
			get
			{
				return _showBrowserCommand;
			}
			set { _showBrowserCommand = value; }
		}



		public MainWindowViewModel()
		{
			_showBrowserCommand = new RelayCommand(ShowBrowser);
		}

		public void ShowBrowser()
		{
			MessageBox.Show("halooo");
			if(_browser == null)
				_browser = new Browser();
			_browser.Show();
		}

	}
}
