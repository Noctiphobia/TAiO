using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Converters;

namespace TAiO.Model
{   

    /// <summary>
    /// Klasa reprezentująca planszę, na której układane są klocki
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Szerokość planszy
        /// </summary>
	    public int Width
	    {
	        get { return Content.GetLength(0); }
	    }
        /// <summary>
        /// Wysokość planszy
        /// </summary>
		public int Height
        {
            get { return Content.GetLength(1); }
        }
        /// <summary>
        /// Zawartość planszy
        /// 0 oznacza brak klocka
        /// >0 to numer klocka w danym miejscu
        /// </summary>
        public int[,] Content { get; set; }
        /// <summary>
        /// Liczba klocków już położonych na planszy
        /// </summary>
        public int BlocksNumber { get; set; }
        /// <summary>
        /// Krok wysokości - o tyle zwiększamy wysokość,
        /// gdy następny położony klocek może wystawać poza planszę (tj. wystaje ze studni)
        /// </summary>
		private int StepHeight { get; set; }
        /// <summary>
        /// Flaga określająca, czy "śledzimy" klocki -
        /// domyślnie jest to true, jednak w przypadku plansz tymczasowych,
        /// na których często dokładamy i odkładamy te same klocki
        /// lub z różnych przyczyn jesteśmy pewni, że ilość klocków jest właściwa
        /// (vide ChooseBlocks() poniżej lub UpdateDataSource() w PreviewViewModel)
        /// nie ma potrzeby wykonywać czasochłonnych dodawań i odejmowań elemenów w słowniku
        /// </summary>
		private bool KeepTrackOfBlocks { get; set; }
        /// <summary>
        /// Słownik zawierający parę: klocek i liczba dostępnych klocków danego typu minus te obecne na planszy
        /// </summary>
	    private SortedList<BlockType, int> AvailableBlocks;
        
		public int this[int x, int y]
	    {
		    get { return Content[x, y]; }
		    set { Content[x, y] = value; }
	    }
        /// <summary>
        /// Konstruktor planszy
        /// </summary>
        /// <param name="w">szerokość planszy</param>
        /// <param name="h">wysokość planszy</param>
        /// <param name="availableBlocks">dostępne klocki (patrz opis do AvailableBlocks)</param>
        /// <param name="keepTrackOfBlocks">czy chcemy śledzić klocki (patrz opis do KeepTrackOfBlocks)</param>
        public Board(int w, int h, SortedList<BlockType, int> availableBlocks, bool keepTrackOfBlocks = true)
        {
			StepHeight = Math.Max(h / 2, 4);
			Content = new int[w, h];
            BlocksNumber = 0;
	        AvailableBlocks = availableBlocks;
	        KeepTrackOfBlocks = keepTrackOfBlocks;
        }

        /// <summary>
        /// Funkcja kopiująca planszę wraz z zawartością
        /// </summary>
        /// <param name="keepTrackOfBlocks">czy chcemy śledzić klocki (patrz opis do KeepTrackOfBlocks)</param>
        /// <returns></returns>
	    private Board Copy(bool keepTrackOfBlocks = true)
	    {
		    return new Board(Width, Height, keepTrackOfBlocks ? new SortedList<BlockType, int>(AvailableBlocks) : null, keepTrackOfBlocks)
		    { 
			    Content = (int[,]) this.Content.Clone(),
			    BlocksNumber = this.BlocksNumber,
			    StepHeight = this.StepHeight
		    };
	    }

        /// <summary>
        /// Funkcja tworząca planszę z danych zapisanych w StepsData
        /// </summary>
        /// <param name="data">Dane kroków algorytmu</param>
        /// <param name="stepNumber">numer kroku algorytmu</param>
        /// <param name="boardNumber">numer planszy</param>
        /// <param name="width">szerokość planszy</param>
        /// <param name="height">wysokość planszy</param>
        /// <param name="keepTrackOfBlocks">czy chcemy śledzić klocki (patrz opis do KeepTrackOfBlocks)</param>
        /// <param name="availableBlockSortedList">dostępne klocki (patrz opis do AvailableBlocks)</param>
        /// <returns></returns>
	    public static Board CreateFromStepsData(StepsData data, int stepNumber, int boardNumber, int width, int height, 
            bool keepTrackOfBlocks = false, SortedList<BlockType, int> availableBlockSortedList = null)
	    {
			Board board = new Board(width, height, availableBlockSortedList, keepTrackOfBlocks);
			data.SetStartingPoint(stepNumber, boardNumber);
			List<BlockInstance> bis = new List<BlockInstance>(data);
			bis.Reverse();
		    foreach (var blockInstance in bis)
		    {
                board.AddBlock(blockInstance);
		    }
		    return board;
	    }


        /// <summary>
        /// Funkcja zwiększająca wysokość planszy
        /// </summary>
		private void Resize()
        {
			int[,] tmp = new int[Width, Height + StepHeight];
			for (int i = 0; i < Height; i++)
			    for (int j = 0; j < Width; j++)
			        tmp[j, i] = Content[j, i];
		    Content = tmp;
        }

        /// <summary>
        /// Funkcja dodająca klocek na planszę
        /// </summary>
        /// <param name="block">Informacje o klocku i gdzie ma zostać położony</param>
        /// <returns>True jeśli się udało, false wpp</returns>
        public bool AddBlock(BlockInstance block)
        {
			int h = (block.BlockVersion%2 == 0 ? block.Block.Height : block.Block.Width),
				w = (block.BlockVersion%2 == 0 ? block.Block.Width : block.Block.Height);
			
			while (block.Y + h >= Height)
				Resize();
            BlocksNumber++;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
					if (block.Block.Shape[block.BlockVersion][i, j] == 0)
						continue;
					if (Content[i + block.X, j + block.Y] > 0)
		                return false;
					Content[i + block.X, j + block.Y] = block.Block.Shape[block.BlockVersion][i, j] * BlocksNumber;
                }
	        if (!KeepTrackOfBlocks)
				return true;
	        if (AvailableBlocks.ContainsKey(block.Block) && AvailableBlocks[block.Block] > 0)
	        {
		        AvailableBlocks[block.Block] = AvailableBlocks[block.Block] - 1;
		        return true;
	        }
            return false;
        }

        /// <summary>
        /// Funkcja zdejmująca klocek z planszy
        /// </summary>
        /// <param name="block">Informacje o klocku i gdzie został położony</param>
        /// <returns>True jeśli się udało, false wpp</returns>
	    public bool DeleteBlock(BlockInstance block)
	    {
			int h = (block.BlockVersion % 2 == 0 ? block.Block.Height : block.Block.Width),
				w = (block.BlockVersion % 2 == 0 ? block.Block.Width : block.Block.Height);
			BlocksNumber--;
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					if(block.Block.Shape[block.BlockVersion][i, j] > 0)
						Content[i + block.X, j + block.Y] = 0;
				}
			if (!KeepTrackOfBlocks)
				return true;
			if (AvailableBlocks.ContainsKey(block.Block))
			{
				AvailableBlocks[block.Block]++;
				return true;
			}
			return true;
		}

        /// <summary>
        /// Wybiera wskazaną liczbę ułożeń z najmniejszymi wartościami funkcji kosztu
        /// </summary>
        /// <param name="resultsCount">Ile zwrócić wyników (= liczba rozgałęzień algorytmu)</param>
        /// <param name="costFunction">Funkcja kosztu</param>
        /// <param name="placementFunction">Funkcja położenia</param>
        /// <returns>Listę posortowanych rosnąco po funkcji kosztu rozwiązań</returns>
        public List<PartialSolution> ChooseBlocks(int resultsCount, CostFunction costFunction, PlacementFunction placementFunction)
        {
            var solutions = new List<PartialSolution>();
	        int k = 0;
	        int max = 0;

	        Board board = this.Copy(false);

	        foreach (var blockType in this.AvailableBlocks)
	        {
		        if (blockType.Value < 1)
			        continue;
	            bool blockPlaced = false;
		        for (int i = 0; i < blockType.Key.Shape.Count; i++)
		        {
			        BlockInstance bi = placementFunction(this, blockType.Key, i);
		            if (bi.Block == null)
		                continue;
		            blockPlaced = true;
			        board.AddBlock(bi);
			        int cost = costFunction(board);
					board.DeleteBlock(bi);
					
					if (k < resultsCount)
					{
						solutions.Add(new PartialSolution() { Cost = cost, Move = bi });
						k++;
						if (cost > max)
							max = cost;
					}
					else if (cost < max)
					{
						bool done = false;
					    for (int j = 0; j < solutions.Count; j++) // podmiana
					    {
						    if (solutions[j].Cost > max)
						    {
							    max = solutions[j].Cost;
						    }
						    if (solutions[j].Cost == max && !done)
						    {
							    done = true;
							    solutions[j].Cost = cost;
							    solutions[j].Move = bi;
						    }
					    }
				        
			        }
		        }
	            if (!blockPlaced)
	                throw new ArgumentException("Jeden z klocków jest szerszy niż plansza.");
	        }

			solutions.Sort(new PartialSolutionsComparer());
			return solutions;
        }

	    protected bool Equals(Board other)
	    {
		    if (BlocksNumber != other.BlocksNumber || Width != other.Width)
			    return false;
		    int h = Math.Min(Height, other.Height);
			for (int i=0; i<Width; ++i)
				for (int j=0; j<h; ++j)
					if (Content[i, j] != other.Content[i, j])
						return false;
		    return true;
	    }

	    public override int GetHashCode()
	    {
		    unchecked
		    {
			    var hashCode = Content?.GetHashCode() ?? 0;
			    hashCode = (hashCode*397) ^ BlocksNumber;
			    return hashCode;
		    }
	    }

	    public override string ToString()
	    {
		    StringBuilder sb = new StringBuilder();
		    for (int j = 0; j < Height; ++j)
		    {
			    for (int i = 0; i < Width; ++i)
			    {
				    sb.Append(Content[i, j]);
			    }
			    sb.Append('\n');
		    }
		    return sb.ToString();
	    }
    }
}
