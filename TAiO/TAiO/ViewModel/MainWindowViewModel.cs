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
		private string _status;
		private int _step = 1;
		private int _delay = 3;
		private uint _boardWidth = 0;
		private bool _running = false;
		private bool _stopped = true;
		private string _playPause = "▶";
		private Browser _browser;
		private List<Preview> _previews = new List<Preview>();
		private DispatcherTimer _timer = new DispatcherTimer();
		private Algorithm LastAlgorithm = null;
		


		public string Status
		{
			get { return _status; }
			set
			{
				_status = value;
				RaisePropertyChanged(nameof(Status));
			}
		}

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

		public uint BoardWidth
		{
			get { return _boardWidth; }
			set
			{
				_boardWidth = value;
				Data.Instance.BoardWidth = (int)value;
				RaisePropertyChanged(nameof(BoardWidth));
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
			int currentStep = 0;
			for (int i = 0; i < _previews.Count; i++)
			{
				var preview = _previews[i];
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				if (vm.CurrentStep > 0)
				{
					if (vm.CurrentStep - vm.StepsPerChange >= 0)
						currentStep = vm.CurrentStep - vm.StepsPerChange;
					//vm.CurrentStep = vm.CurrentStep - vm.StepsPerChange;
					else
					{
						currentStep = 0;
						//vm.CurrentStep = 0;
					}
					//currentStep = vm.CurrentStep;
					vm.SetCurrentStep(LastAlgorithm?.StepsData, i, currentStep, Data.BoardWidth, Data.BoardWidth);
				}
			}
			Status = StatusFactory.PausedAlgorithm(currentStep);
		}, () =>
		{
			if (_previews.Count == 0)
				return false;
			PreviewViewModel vm = _previews[0].DataContext as PreviewViewModel;
			return vm?.CurrentStep > 0;
		});

		public ICommand NextStep => new RelayCommand(() =>
		{
			int currentStep = 0;
			for (int i = 0; i < _previews.Count; i++)
			{
				var preview = _previews[i];
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				currentStep = vm.CurrentStep + vm.StepsPerChange;
				//currentStep = vm.CurrentStep;
				vm.SetCurrentStep(LastAlgorithm?.StepsData, i, currentStep, Data.BoardWidth, Data.BoardWidth);
			}
			Status = StatusFactory.PausedAlgorithm(currentStep);
		}, () =>
		{
			if (_previews.Count == 0)
				return false;
			PreviewViewModel vm = _previews[0].DataContext as PreviewViewModel;
			if (vm == null)
				return false;
			return vm.CurrentStep < (LastAlgorithm?.CurrentStep ?? -1) + 1;	
		});

		public ICommand FirstStep => new RelayCommand(() =>
		{
			for (int i = 0; i < _previews.Count; i++)
			{
				var preview = _previews[i];
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				//vm.CurrentStep = 0;
				vm.SetCurrentStep(LastAlgorithm?.StepsData, i, 0, Data.BoardWidth, Data.BoardWidth);
			}
			Status = StatusFactory.PausedAlgorithm(0);
		}, () => _previews.Count > 0);

		public ICommand LastStep => new RelayCommand(() =>
		{
			int currentStep = LastAlgorithm?.CurrentStep + 1 ?? 0;
			for (int i = 0; i < _previews.Count; i++)
			{
				var preview = _previews[i];
				PreviewViewModel vm = preview.DataContext as PreviewViewModel;
				if (vm == null)
					continue;
				//vm.CurrentStep = currentStep;
				vm.SetCurrentStep(LastAlgorithm?.StepsData, i, currentStep, Data.BoardWidth, Data.BoardWidth);
			}
			Status = StatusFactory.PausedAlgorithm(currentStep);
		}, () => _previews.Count > 0);

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

				Algorithm a = new Algorithm(Data, (dialog.DataContext as BranchesDialogViewModel)?.CostFunction, PlacementFunctionFactory.AvailableFunctions[0].Function);

				bool success = true;
				try
				{
					a.RunAlgorithm();
				}
				catch (Exception e)
				{
                    MessageBox.Show(e.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    success = false;
				}
				if (!success)
					return;

				LastAlgorithm = a;
				Rect area = SystemParameters.WorkArea;
				double width = area.Width/Data.Branches;
				double height = area.Height - 195;
				for (int i = 0; i < Data.Branches; ++i)
				{
					Preview preview = new Preview
					{
						Width = width,
						Height = height,
						Left = area.Left + i*width,
						Top = area.Top
					};
					preview.Closed += (o, e) =>
					{
						if (_previews.Count>0)
							_previews.Remove(preview);
						if (_previews.Count == 0)
							Stop.Execute(this);
					};
					var vm = preview.DataContext as PreviewViewModel;
					_previews.Add(preview);
					preview.Show();
					if (vm == null) continue;

					vm.StepsPerChange = Step;

					vm.SetCurrentStep(LastAlgorithm?.StepsData, i, 0, Data.BoardWidth, Data.BoardWidth);
					//vm.CurrentStep = 0;
					//vm.UpdateDataSource(a.StepsData, i, a.CurrentStep, Data.BoardWidth, Data.BoardWidth);
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
				Status = StatusFactory.RunningAlgorithm((int)Data.Blocks.Sum(b => b.BlockNumber), Data.Branches);
			}
			Running = !Running;
		});

		/// <summary>
		/// Zatrzymaj wszystko.
		/// </summary>
		public ICommand Stop => new RelayCommand(() =>
		{
			Stopped = true; 
			while (_previews.Count>0)
				_previews[0].Close();
			Status = StatusFactory.StoppedAlgorithm();
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
		});

		/// <summary>
		/// Wczytaj nowy zestaw klocków.
		/// </summary>
		public ICommand Load => new RelayCommand(() =>
		{
			OpenFileDialog dialog = new OpenFileDialog {Multiselect = false};
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
					BoardWidth = (uint)width;
					Status = StatusFactory.LoadedBlocks(blocks.Count, dialog.FileName);
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
			Status = StatusFactory.BeforeLoad();
		}
	}
}
