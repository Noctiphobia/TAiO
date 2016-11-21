using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
	/// <summary>
	/// Klasa zawierająca wszystkie funkcje umieszczenia klocka wraz z łatwym dostępem do ich listy.
	/// </summary>
	public static class PlacementFunctionFactory
	{
		public static List<NamedPlacementFunction> AvailableFunctions { get; } = new List<NamedPlacementFunction>
		{
			new NamedPlacementFunction((board, block, rotation) =>
			{
			    List<BlockInstance> result = new List<BlockInstance>();
				BlockInstance resultBlock = new BlockInstance {Block = block, BlockVersion = rotation};
				int[,] blockTab = block.Shape[rotation];
				int blockWidth = blockTab.GetLength(0), blockHeight = blockTab.GetLength(1);
			    bool isNotEmptyLine = true;
			    for (int i = 0; i < board.Height && isNotEmptyLine; i++)
			    {
			        isNotEmptyLine = false;
			        for (int j = 0; j < board.Width - blockWidth + 1; j++)
			        {
			            if (board[j, i] > 0)
			                isNotEmptyLine = true;
			            int maxHeight = Math.Min(blockHeight, board.Height - i);
			            bool isGood = true;
			            for (int k = 0; k < maxHeight && isGood; k++)
			                for (int m = 0; m < blockWidth && isGood; m++)
			                    if (blockTab[m, k] > 0 && board.Content[j + m, i + k] > 0)
			                        isGood = false;
			            if (isGood)
			            {
			                resultBlock.X = j;
			                resultBlock.Y = i;
			                result.Add(resultBlock);
			            }
			        }
			    }
			    if (block.Width <= board.Width)
			    {
			        resultBlock.X = 0;
			        resultBlock.Y = board.Height;
			        result.Add(resultBlock);
			    }
			    return result;

			}, ""),
		};
	}
}
