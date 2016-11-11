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
				RaisePropertyChanged(nameof(DataSource));
			}
		}



		public void UpdateDataSource(StepsData data, int boardNumber, int stepNumber, int width, int height)
		{
			Board board = new Board(width, height, new SortedList<BlockType, int>());
			data.SetStartingPoint(stepNumber, boardNumber);
			foreach (BlockInstance blockInstance in data)
			{
				
				board.AddBlock(blockInstance);
			}
			DataSource = new Array2D(board.Content);
		}


	}
}
