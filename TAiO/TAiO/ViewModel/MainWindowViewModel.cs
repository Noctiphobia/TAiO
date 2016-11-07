using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
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
		private int _delay = 3;
		private bool _running = false;
		private bool _stopped = true;
		private string _playPause = "▶";
		private Browser _browser;
		private List<Preview> _previews = new List<Preview>();
		private DispatcherTimer _timer = new DispatcherTimer();

		/// <summary>
		/// Krok symulacji.
		/// </summary>
		public int Step
		{
			get { return _step; }
			set
			{
				if (value > 0)
				{
					_step = value;
					foreach (var p in _previews)
					{
						var vm = p.DataContext as PreviewViewModel;
						if (vm == null) continue;
						vm.StepsPerChange = _step;
					}
					RaisePropertyChanged(nameof(Step));
					RaisePropertyChanged(nameof(StepString));
				}
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
		/// Opóźnienie między krokami symulacji.
		/// </summary>
		public int Delay
		{
			get { return _delay; }
			set
			{
				if (value > 0)
				{
					_delay = value;
					bool started = _timer.IsEnabled;
					if (started)
						_timer.Stop();
					_timer.Interval = TimeSpan.FromSeconds(value);
					if (started)
						_timer.Start();
					RaisePropertyChanged(nameof(Delay));
					RaisePropertyChanged(nameof(DelayString));
				}
			}
		}

		/// <summary>
		/// Opóźnienie między krokami symulacji - wersja wpisana przez użytkownika.
		/// </summary>
		public string DelayString
		{
			get { return Delay.ToString(); }
			set
			{
				int val;
				if (int.TryParse(value, out val))
				{
					Delay = val;
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
						if (_running)
							_timer.Start();
						else
							_timer.Stop();
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


		public ICommand PreviousStep => new RelayCommand(() =>
		{
			foreach (var preview in _previews)
			{
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if(vm == null)
					continue;
				if (vm.CurrentStep > 0)
				{
					if (vm.CurrentStep - vm.StepsPerChange >= 0)
						vm.CurrentStep = vm.CurrentStep - vm.StepsPerChange;
					else
					{
						vm.CurrentStep = 0;
					}
				}
			}
		});

		public ICommand NextStep => new RelayCommand(() =>
		{
			foreach (var preview in _previews)
			{
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				vm.CurrentStep = vm.CurrentStep + vm.StepsPerChange;
			}
		});

		public ICommand FirstStep => new RelayCommand(() =>
		{
			foreach (var preview in _previews)
			{
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				vm.CurrentStep = 0;
			}
		});

		public ICommand LastStep => new RelayCommand(() =>
		{
			foreach (var preview in _previews)
			{
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				vm.CurrentStep = 1000;
			}
		});

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

				if (Data.Blocks.Count == 0)
					return;
				Algorithm a = new Algorithm(Data, null);
				a.RunAlgorithm();
				

				for (int i = 0; i < Data.Branches; ++i)
				{
					Preview preview = new Preview();
					var vm = preview.DataContext as PreviewViewModel;
					_previews.Add(preview);
					preview.Show();
					if (vm == null) continue;
					vm.StepsPerChange = Step;
					vm.CurrentStep = 3;
					vm.UpdateDataSource(a.StepsData, i, vm.CurrentStep, Data.BoardWidth, Data.BoardWidth);
					//vm.DataSource = new Array2D(
					//	new[,]
					//	{
					//			{ 0, 1, 1, 3, 2 },
					//			{ 1, 1, 3, 3, 2 },
					//			{ 1, 1, 3, 2, 2 },
					//			{ 0, 0, 0, 0, 2 },
					//			{ 0, 0, 0, 2, 2 },
					//			{ 0, 0, 0, 0, 0 }
					//	});
				}
			}
			Running = !Running;
		});

		/// <summary>
		/// Zatrzymaj wszystko.
		/// </summary>
		public ICommand Stop => new RelayCommand(() =>
		{
			Stopped = true; 
			foreach (var p in _previews)
				p.Close();
			_previews.Clear();
		}, () => !Stopped);

		/// <summary>
		/// Pokaż przeglądarkę klocków.
		/// </summary>
		public ICommand ShowBrowser => new RelayCommand(() =>
		{
			if (_browser == null || !_browser.IsLoaded)
			{
				_browser = new Browser();
				_browser.Show();
				RefreshBrowserBlocks();
			}
			//TODO: poprawić
		});

		/// <summary>
		/// Wczytaj nowy zestaw klocków.
		/// </summary>
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
					RefreshBrowserBlocks();
				}
			}
		}, () => Stopped);

		private void RefreshBrowserBlocks()
		{
			((BrowserViewModel) _browser?.DataContext)?.RefreshBlockTypeViewModelsList(Data.Instance.Blocks);
		}

		public MainWindowViewModel()
		{
			_timer.Tick += (o, e) => { if (NextStep.CanExecute(this)) NextStep.Execute(this); };
			_timer.Interval = TimeSpan.FromSeconds(3);
		}
	}
}
