using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
using TAiO.Tools;

namespace TAiO.Tests
{
	[TestClass]
	public class AlgorithmTests
	{
		/// <summary>
		/// Domyślna nazwa pliku z klockami
		/// </summary>
		const string filename = "60.blocks";
		/// <summary>
		/// Domyślna nazwa funkcji kosztu
		/// </summary>
		const string functionName = "Najmniej dziur";
		/// <summary>
		/// Definicja funkcji wybierającej klocki do algorytmu
		/// </summary>
		/// <param name="blocks"></param>
		delegate void blockChooser(List<BlockType> blocks);
		
		/// <summary>
		/// Sprawdzenie gęstości ułożenia klocków
		/// Argumenty: dwie gałęzie, 20 klocków, funkcja "Najmniej dziur", klocki z pliku "60.blocks"
		/// </summary>
		[TestMethod]
		public void TwoBranches()
		{
			double density = RunAlgorithm((blocks) => KFirstChooser(blocks, 20, 1), functionName, filename, 2);
		}
		/// <summary>
		/// Sprawdzenie gęstości ułożenia klocków
		/// Argumenty: trzy gałęzie, 20 klocków, funkcja "Najmniej dziur", klocki z pliku "60.blocks"
		/// </summary>
		[TestMethod]
		public void ThreeBranches()
		{
			double density = RunAlgorithm((blocks) => KFirstChooser(blocks, 20, 1), functionName, filename, 3);
		}
		/// <summary>
		/// Sprawdzenie gęstości ułożenia klocków
		/// Argumenty: trzy gałęzie, 5*(liczba typów) klocków, funkcja "Najmniej dziur", klocki z pliku "60.blocks"
		/// </summary>
		[TestMethod]
		public void ThreeBranchesWholeSet()
		{
			double density = RunAlgorithm((blocks) => WholeSetChooser(blocks, 5), functionName, filename, 3);
		}

		/// <summary>
		/// Sprawdzenie gęstości ułożenia klocków
		/// Argumenty: 20 gałęzi, 5*(liczba typów) klocków, funkcja "Najmniej dziur", klocki z pliku "60.blocks"
		/// </summary>
		[TestMethod]
		public void TwentyBranchesWholeSet()
		{
			double density = RunAlgorithm((blocks) => WholeSetChooser(blocks, 5), functionName, filename, 20);
		}
		/// <summary>
		/// Funkcja pomocnicza - ustawia wybór k klocków kazdgo typu
		/// </summary>
		/// <param name="blocks">lista klocków</param>
		/// <param name="k">liczba klocków do wybrania</param>
		private void WholeSetChooser(List<BlockType> blocks, uint k)
		{
			int blocksNumber = 10;
			if (blocks.Count == 0)
				Assert.Fail("0 blocks");
			blocks[0].BlockNumber = (uint)blocksNumber;

			for (int i = 0; i < blocks.Count; i++)
			{
				blocks[i].BlockNumber = k;
			}
		}

		/// <summary>
		/// Funkcja pomocnicza ustawiająca wybór n klocków z k pierwszych typów (razem k*n klocków)
		/// </summary>
		/// <param name="blocks">lista klocków</param>
		/// <param name="k">liczba typów klocków</param>
		/// <param name="n">liczba klocków każdego typu</param>
		private void KFirstChooser(List<BlockType> blocks, int k, uint n)
		{
			int blocksNumber = k;
			if (blocks.Count == 0)
				Assert.Fail("0 blocks");
			blocks[0].BlockNumber = (uint)blocksNumber;

			int chosen = Math.Min(blocks.Count, k);

			for (int i = 1; i < chosen; i++)
			{
				blocks[i].BlockNumber = n;
			}

			for (int i = chosen + 1; i < blocks.Count; i++)
			{
				blocks[i].BlockNumber = 0;
			}


		}

		/// <summary>
		/// Funkcja pomocnicza uruchamiająca algorytm
		/// </summary>
		/// <param name="chooser">funkcja wybierająca klocki do algorytmu</param>
		/// <param name="function">nazwa funkcji kosztu</param>
		/// <param name="localFilename">nazwa pliku z klockami</param>
		/// <param name="branches">liczba rozgałęzień algorytmu</param>
		/// <returns></returns>
		private double RunAlgorithm(blockChooser chooser, string function, string localFilename, int branches)
		{
			if (!File.Exists(localFilename))
				Assert.Inconclusive("File {0} doesn't exist.", localFilename);
			Data Data = Data.Instance;
			List<BlockType> blocks;
			int width;
			Parser.ParseFile(localFilename, out blocks, out width);

			if (blocks.Count == 0)
				Assert.Fail("0 blocks");

			chooser(blocks);

			Data.Blocks = blocks;
			Data.BoardWidth = width;
			Data.Branches = branches;

			Algorithm a = new Algorithm(Data, CostFunctionFactory.AvailableFunctions.
				Find(b => b.Name == function).Function, PlacementFunctionFactory.AvailableFunctions[0].Function);

			a.RunAlgorithm();

			double Density;
			// first is the best
			Board board = a.CurrentbestBoard;
			Density = CalculateDensity(new Array2D(board.Content));

			return Density;

		}

		/// <summary>
		/// Funkcja wyliczająca minimalną wysokość ułożonych klocków
		/// </summary>
		/// <param name="DataSource">plansza</param>
		/// <returns></returns>
		private int CalculateHeight(Array2D DataSource)
		{
			int Height;
			if (DataSource == null)
			{
				return 0;
			}
			int height = 0;
			for (int i = 0; i < DataSource.Width; ++i)
			{
				for (int j = DataSource.Height - 1; j >= 0; --j)
					if (DataSource[i, j] != 0)
					{
						if (j + 1 > height)
							height = j + 1;
						break;
					}
			}

			Height = height;
			return Height;
		}
		/// <summary>
		/// Funkcja wyliczająca gęstość ułożonych klocków
		/// </summary>
		/// <param name="DataSource">plansza</param>
		/// <returns></returns>
		private double CalculateDensity(Array2D DataSource)
		{
			if (DataSource == null)
				return 0;
			int Height = CalculateHeight(DataSource);

			int count = 0;
			for (int i = 0; i < DataSource.Width; ++i)
				for (int j = 0; j < Height; ++j)
					if (DataSource[i, j] > 0)
						++count;
			return (int)Math.Round((double)(count) / (DataSource.Width * Height) * 100);

		}
	}
}
