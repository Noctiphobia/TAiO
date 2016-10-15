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
			get { return _running;}
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
		});
	}

    public class Parser
    {
        public static bool ParseFile(string filename, List<BlockType> blocks, int width)
        {
            blocks = new List<BlockType>();
            string[] lines = System.IO.File.ReadAllLines(@filename);
            string[] words = lines[0].Split(' ');
            if (!Int32.TryParse(words[0], out width))
                return false;
            for (int i = 1; i < lines.Length; i++)
            {
                words = lines[i].Split(' ');
                if (words.Length == 0)
                    continue;
                if(words.Length != 2)
                    return false;
                int w, h;
                if (!Int32.TryParse(words[0], out w))
                    return false;
                if (!Int32.TryParse(words[1], out h))
                    return false;
                int end = i + h;
                int[,] s = new int[w, h];
                for (int j = 0; j < h; j++, i++)
                {
                    words = lines[i].Split(' ');
                    for (int k = 0; k < w; k++)
                    {
                        int b;
                        if (!Int32.TryParse(words[k], out b))
                            return false;
                        s[k, j] = b;
                    } 
                }
                BlockType block = new BlockType(w, h, s);
                blocks.Add(block);
            }
            return true;
        }
    }
}
