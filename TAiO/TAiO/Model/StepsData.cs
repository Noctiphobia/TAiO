using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
    /// Klasa zawierająca zminimalizowane dane algorytmu w każdym kroku programu
    /// Informacje z niej wystarczą do narysowania każdego kroku na każdej planszy
	/// </summary>
	public class StepsData:IEnumerable<BlockInstance>
	{
		/// <summary>
		/// Array of BlockInstances
		/// first coordinate is a step number
		/// second coordinate is a board number
		/// in BlockInstance there's an info about on which board there was a previous block located,
		/// so we can draw all blocks 
        /// Tablica obiektów BlockInstance
        /// pierwsza współrzędna - numer kroku
        /// druga współrzędna - numer planszy
        /// w obiekcie BlockInstance zapisana jest informacja,
        /// na której planszy polożony był poprzedni klocek w tej gałęzi algorytmu.
        /// Powstaje zatem struktura drzewiasta zapisana w tablicy
		/// </summary>
		public BlockInstance[,] BlockInstances { get; private set; }

        /// <summary>
        /// Pomocnicze zmienne, opisujące, która współrzędna czego dotyczy
        /// </summary>
		protected const int StepNumberCoord = 0;
		protected const int BoardNumberCoord = 1;

        /// <summary>
        /// Zmienne określające punkt startowy dla enumeracji (foreach)
        /// </summary>
		protected int startingI;
		protected int startingJ;
        

        public int LastStepFinished { get; set; }
        /// <summary>
        /// Konstruktor obiektu klasy StepsData
        /// </summary>
        /// <param name="branches">rozgałęzienie algorytmu (liczba plansz)</param>
        /// <param name="blocksNumber">ilość bloków (= liczba krokrów)</param>
        public StepsData(int branches, int blocksNumber)
		{
			BlockInstances = new BlockInstance[blocksNumber, branches];
            LastStepFinished = -1;
		}

        /// <summary>
        /// Funkcja ustawiająca współrzędne węzła początkowego do foreacha
        /// </summary>
        /// <param name="stepNumber">pierwsza współrzędna (numer kroku)</param>
        /// <param name="boardNumber">druga współrzędna (numer planszy)</param>
		public void SetStartingPoint(int stepNumber, int boardNumber)
		{
			int last = LastStepFinished;
			startingI = stepNumber;
			startingJ = boardNumber;

			if (startingI > last)
				throw new ArgumentException("You're starting from the wrong step number! - StepsData.SetStartingPoint()");
		}

        /// <summary>
        /// Funkcja umożliwiająca wykonanie foreach po drzewku od liścia w górę
        /// </summary>
        /// <returns></returns>
		public IEnumerator<BlockInstance> GetEnumerator()
		{
			int j = startingJ;
			int prevBoard;
			for (int i = startingI; i >= 0; i--)
			{
				prevBoard = BlockInstances[i, j].PreviousBlockBoardNumber;
				yield return BlockInstances[i, j];
				j = prevBoard;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
