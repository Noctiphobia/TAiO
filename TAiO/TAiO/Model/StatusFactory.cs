using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	public static class StatusFactory
	{
		public static string BeforeLoad() => "Please load blocks before starting any algorithm";

		public static string LoadedBlocks(int blocksNumber, string file) =>
			"Loaded " + blocksNumber + " blocks from file " + file;

		public static string RunningAlgorithm(int blockNumber, int branches) =>
			"Algorithm with " + branches + " branches and " + blockNumber + " blocks is now running";

		public static string PausedAlgorithm(int step) =>
			"Algorithm is on step: " + step;

		public static string StoppedAlgorithm() =>
			"Algorithm stopped. Adjust settings and start again";

	}
}
