using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TAiO.Model;

namespace TAiO.ViewModel
{
	/// <summary>
	/// ViewModel dotyczący podglądu przebiegu algorytmu.
	/// </summary>
	public class PreviewViewModel : BaseViewModel
	{
		private int _stepsPerChange;
		private int _currentStep;
		private Array2D _dataSource;
		private int _height;
        private int _density;

		/// <summary>
		/// Kroki na jedną zmianę.
		/// </summary>
		public int StepsPerChange
		{
			get
			{
				return _stepsPerChange;
			}
			set
			{
				_stepsPerChange = value; 
				RaisePropertyChanged(nameof(StepsPerChange));
			}
		}
		
		/// <summary>
		/// Obecny krok. 
		/// </summary>D
		public int CurrentStep
		{
			get
			{
				return _currentStep;
			}
			set
			{
				_currentStep = value;
				CalculateHeightAndDensity();
				RaisePropertyChanged(nameof(CurrentStep));
			}
		}

		/// <summary>
		/// Źródło danych.
		/// </summary>
		public Array2D DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				_dataSource = value;
				CalculateHeightAndDensity();
				RaisePropertyChanged(nameof(DataSource));
			}
		}

		/// <summary>
		/// Obecna wysokość planszy.
		/// </summary>
		public int Height
		{
			get { return _height; }
			set
			{
				_height = value;
				RaisePropertyChanged(nameof(Height));
				RaisePropertyChanged(nameof(HeightString));
			}
		}

		/// <summary>
		/// Napis o wysokości.
		/// </summary>
		public string HeightString => $"Wysokość: {Height}";


        /// <summary>
        /// Gęstość.
        /// </summary>
        public int Density
        {
            get { return _density; }
            set
            {
                _density = value;
                RaisePropertyChanged(nameof(Density));
                RaisePropertyChanged(nameof(DensityString));
            }
        }

        /// <summary>
        /// Napis o gęstości.
        /// </summary>
        public string DensityString => $"Gęstość: {Density}%";

        /// <summary>
        /// Odświeżenie widoku na podstawie danych ze struktury.
        /// </summary>
        /// <param name="data">Struktura z danymi kroku.</param>
        /// <param name="boardNumber">Numer tablicy.</param>
        /// <param name="viewstepNumber">Numer kroku.</param>
        /// <param name="width">Szerokość planszy.</param>
        /// <param name="height">Wysokość planszy.</param>
		public void UpdateDataSource(StepsData data, int boardNumber, int viewstepNumber, int width, int height)
		{
			Board board = Board.CreateFromStepsData(data, viewstepNumber - 1, boardNumber, width, height);
		    board.AddBlock(data.BlockInstances[Math.Min(viewstepNumber, data.BlockInstances.GetLength(0) - 1), boardNumber]);
			DataSource = new Array2D(board.Content);
		}

        /// <summary>
        /// Przestawienie obecnego kroku.
        /// </summary>
        /// <param name="data">Struktura z danymi kroku.</param>
        /// <param name="boardNumber">Numer tablicy.</param>
        /// <param name="viewstepNumber">Numer kroku.</param>
        /// <param name="width">Szerokość planszy.</param>
        /// <param name="height">Wysokość planszy.</param>
		public void SetCurrentStep(StepsData data, int boardNumber, int viewstepNumber, int width, int height)
		{
			CurrentStep = viewstepNumber;
			if(data != null && viewstepNumber <= data.LastStepFinished + 1)
				UpdateDataSource(data, boardNumber, viewstepNumber, width, height);
		}
		/// <summary>
		/// Oblicz gęstość oraz wysokość
		/// </summary>
		private void CalculateHeightAndDensity()
		{
            CalculateHeight();
            CalculateDensity();
		}
		/// <summary>
		/// Oblicz wysokość
		/// </summary>
		private void CalculateHeight()
        {
            if (DataSource == null)
            {
                Height = 0;
                return;
            }
            int height = 0;
            for (int i = 0; i < DataSource.Width; ++i)
            {
                for (int j = DataSource.Height - 1; j >= 0; --j)
                    if (DataSource[i, j] != 0 && DataSource[i, j] <= CurrentStep)
                    {
                        if (j + 1 > height)
                            height = j + 1;
                        break;
                    }
            }

            Height = height;
        }
		/// <summary>
		/// Oblicz gęstość
		/// </summary>
        private void CalculateDensity()
        {
            if (DataSource == null || Height == 0)
            {
                Density = 100;
                return;
            }
            int count = 0;
            for (int i = 0; i < DataSource.Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (DataSource[i, j] > 0)
                        ++count;
            Density = (int) Math.Round((double)(count) / (DataSource.Width * Height) * 100);
        }
	}
}
