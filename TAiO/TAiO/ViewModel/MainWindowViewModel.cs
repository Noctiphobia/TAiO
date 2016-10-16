using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MicroMvvm;
using TAiO.Model;
using Microsoft.Win32;
using TAiO.Tools;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący okna sterującego całym programem.
	/// </summary>
	public class MainWindowViewModel : BaseViewModel
	{
		private int _step = 1;
		private bool _running = false;
		private bool _stopped = true;
		private string _playPause = "▶";
		private Browser _browser;


		/// <summary>
		/// Krok symulacji.
		/// </summary>
		public int Step
		{
			get { return _step; }
			set
			{
				_step = value;
				RaisePropertyChanged(nameof(Step));
				RaisePropertyChanged(nameof(StepString));
			}
		}

		/// <summary>
		/// Krok symulacji - wersja wpisana przez użytkownika.
		/// </summary>
		public string StepString
		{
			get { return Step.ToString(); }
			set
			{
				int val;
				if (int.TryParse(value, out val))
				{
					Step = val;
				}
			}
		}

		/// <summary>
		/// Czy wizualizacja trwa?
		/// </summary>
		public bool Running
		{
			get { return _running; }
			set
			{
				if (_running != value)
				{
					_running = value;
					PlayPause = Running ? "⏸" : "▶";
					if (Running)
						Stopped = false;
					RaisePropertyChanged(nameof(Running));
				}
			}
		}

		/// <summary>
		/// Czy program jest zresetowany?
		/// </summary>
		public bool Stopped
		{
			get { return _stopped; }
			set
			{
				if (_stopped != value)
				{
					_stopped = value;
					Running = !Stopped;
					RaisePropertyChanged(nameof(Stopped));
				}
			}
		}

		/// <summary>
		/// ⏸ lub ▶
		/// </summary>
		public string PlayPause
		{
			get { return _playPause; }
			set
			{
				_playPause = value;
				RaisePropertyChanged(nameof(PlayPause));
			}
		}


		/// <summary>
		/// Włącz/wyłącz wizualizację.
		/// </summary>
		public ICommand ToggleRunning => new RelayCommand(() =>
		{
			if (Stopped)
			{
				BranchesDialog dialog = new BranchesDialog();
				if (!(dialog.ShowDialog() ?? false))
				{
					return;
				}
                //TODO: tymczasowe
                Preview preview = new Preview();
                preview.Show();
			}
			Running = !Running;
		});

		/// <summary>
		/// Zatrzymaj wszystko.
		/// </summary>
		public ICommand Stop => new RelayCommand(() => { Stopped = true; }, () => !Stopped);

		/// <summary>
		/// Pokaż przeglądarkę klocków.
		/// </summary>
		public ICommand ShowBrowser => new RelayCommand(() =>
		{
			if (_browser == null)
				_browser = new Browser();
			_browser.Show();
            //TODO: poprawić
		});


		public ICommand Load => new RelayCommand(() =>
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			if (dialog.ShowDialog() ?? false)
			{
				List<BlockType> blocks;
				int width;
				if (!Parser.ParseFile(dialog.FileName, out blocks, out width))
				{
					MessageBox.Show("Dane wejściowe są niepoprawne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
				    Data.Instance.Blocks = blocks;
				    Data.Instance.BoardWidth = width;
				}
			}
		}, () => Stopped);
	}
}
