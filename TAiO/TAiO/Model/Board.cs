﻿using System;
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
	    public int Width
	    {
	        get { return Content.GetLength(0); }
	    }
		public int Height
        {
            get { return Content.GetLength(1); }
        }
        public int[,] Content { get; set; }
        public int BlocksNumber { get; set; }
		private int StepHeight { get; set; }
		private bool KeepTrackOfBlocks { get; set; }
	    //private List<KeyValuePair<BlockType, int>> AvailableBlocks2;
		//private Dictionary<> AvailableBlocks;
	    private SortedList<BlockType, int> AvailableBlocks;


		public int this[int x, int y]
	    {
		    get { return Content[x, y]; }
		    set { Content[x, y] = value; }
	    }

        public Board(int w, int h, SortedList<BlockType, int> availableBlocks, bool keepTrackOfBlocks = true)
        {
			StepHeight = h / 2;
			Content = new int[w, h];
            BlocksNumber = 0;
	        AvailableBlocks = availableBlocks;
	        KeepTrackOfBlocks = keepTrackOfBlocks;
        }

	    public Board Copy(bool keepTrackOfBlocks = true)
	    {
		    return new Board(Width, Height, keepTrackOfBlocks ? new SortedList<BlockType, int>(AvailableBlocks) : null, keepTrackOfBlocks)
		    { 
			    Content = (int[,]) this.Content.Clone(),
			    BlocksNumber = this.BlocksNumber,
			    StepHeight = this.StepHeight
		    };
	    }



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
					//if ((Content[i + block.X, j + block.Y] & block.Block.Shape[block.BlockVersion][i, j]) > 0)
					//    return false;

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

	    public bool DeleteBlock(BlockInstance block)
	    {
			int h = (block.BlockVersion % 2 == 0 ? block.Block.Height : block.Block.Width),
				w = (block.BlockVersion % 2 == 0 ? block.Block.Width : block.Block.Height);
			BlocksNumber--;
			for (int i = 0; i < w; i++)
				for (int j = 0; j < h; j++)
				{
					//if ((Content[i + block.X, j + block.Y] & block.Block.Shape[block.BlockVersion][i, j]) > 0)
					//	return false;
					//Content[i + block.X, j + block.Y] = block.Block.Shape[block.BlockVersion][i, j] * BlocksNumber;

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
        /// Wybiera pierwsze miejsce, w którym można położyć klocek
        /// </summary>
        /// <param name="block">Klocek do położenia</param>
        /// <param name="rotation">Wersja klocka</param>
        /// <returns>Obiekt klasy, zawierający informację o umieszczeniu klocka</returns>
        private BlockInstance FindPlaceForBlock(BlockType block, int rotation)
        {
            BlockInstance resultBlock = new BlockInstance {Block = block, BlockVersion = rotation};
            int[,] blockTab = block.Shape[rotation];
            int blockWidth = blockTab.GetLength(0), blockHeight = blockTab.GetLength(1);
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width - blockWidth; j++)
                {
                    int maxHeight = Math.Min(blockHeight, Height - i);
                    bool isGood = true;
                    for (int k = 0; k < maxHeight && isGood; k++)
                        for (int m = 0; m < blockWidth && isGood; m++)
                            if (blockTab[m, k] > 0 && Content[j + m, i + k] > 0)
                                isGood = false;
                    if (isGood)
                    {
                        resultBlock.X = j;
                        resultBlock.Y = i;
                        return resultBlock;
                    }
                }
            resultBlock.X = 0;
            resultBlock.Y = Height;
            return resultBlock;
        }

        /// <summary>
        /// Wybiera wskazaną liczbę ułożeń z najmniejszymi wartościami funkcji kosztu
        /// </summary>
        /// <param name="blocks">Lista klocków</param>
        /// <param name="resultsCount">Ile zwrócić wyników (= liczba rozgałęzień algorytmu)</param>
        /// <returns>Listę posortowanych rosnąco po funkcji kosztu rozwiązań</returns>
        public List<PartialSolution> ChooseBlocks(int resultsCount, CostFunction costFunction)
        {
            // TODO:
            // 1. Funkcja wskazująca miejsce do położenia klocka o danym obrocie
            // UPDATE: Funkcja zwraca BlockInstance, ale NIE ZAPISUJE W TAM INFORMACJI O POPRZEDNIM KLOCKU. Ustawiać tu lub zmienić funkcję.
            // 2. Zaimplementowanie funkcji kosztu i policzenie jej dla każdego obrotu każdego klocka
            // 3. Wybranie i zwrócenie resultsCount ułożeń z najniższą funkcją kosztu
            // ??? Czy ta funkcja powinna zwracać też zaktualizowaną listę klocków?

	        var solutions = new List<PartialSolution>();
	        int k = 0;
	        int min = Int32.MaxValue;

	        Board board = this.Copy(false);

	        foreach (var blockType in this.AvailableBlocks)
	        {
		        if (blockType.Value < 1)
			        continue;
		        for (int i = 0; i < blockType.Key.Shape.Count; i++)
		        {
			        BlockInstance bi = FindPlaceForBlock(blockType.Key, i);
			        board.AddBlock(bi);
			        int cost = costFunction(board);

					board.DeleteBlock(bi);
					

					if (k < resultsCount)
					{
						solutions.Add(new PartialSolution() { Cost = cost, Move = bi });
						k++;
					}
					else if (cost < min)
					{
						bool done = false;
					    for (int j = 0; j < solutions.Count; j++) // podmiana
					    {
						    if (solutions[j].Cost > cost && !done)
						    {
							    done = true;
							    solutions[j].Cost = cost;
							    solutions[j].Move = bi;
						    }
						    if (solutions[j].Cost < min)
						    {
							    min = solutions[j].Cost;
						    }
					    }
				        
			        }
		        }
	        }
			solutions.Sort(new PartialSolutionsComparer());
			return solutions;
        }
    }
}
