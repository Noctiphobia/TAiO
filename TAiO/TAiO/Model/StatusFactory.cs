using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa produkująca statusy wyświetlane w głównym ekranie.
    /// </summary>
	public static class StatusFactory
	{
		public static string BeforeLoad() => "Wczytaj klocki przed uruchomieniem algorytmu.";

		public static string LoadedBlocks(int blocksNumber, string file) =>
			$"Wczytano {blocksNumber} klocków z pliku {file}.";

		public static string RunningAlgorithm(int blockNumber, int branches) =>
			$"Uruchomiono algorytm dla {branches} gałęzi i {blockNumber} klocków" + ".";

		public static string PausedAlgorithm(int step) =>
			$"Obecny krok: {step}.";

		public static string StoppedAlgorithm() =>
			"Algorytm został zatrzymany.";

	}
}
