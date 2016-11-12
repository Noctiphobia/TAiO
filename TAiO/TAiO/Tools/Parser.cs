using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAiO.Model;

namespace TAiO.Tools
{
    /// <summary>
    /// Klasa służąca do tworzenia pobierania szerokości studni i tworzenia listy klocków na podstawie pliku
    /// </summary>
	public class Parser
	{
		public static bool ParseFile(string filename, out List<BlockType> blocks, out int width)
		{
			blocks = new List<BlockType>();
			string[] lines = System.IO.File.ReadAllLines(@filename);
			string[] words = lines[0].Split(' ');
			if (!int.TryParse(words[0], out width))
				return false;
			for (int i = 1; i < lines.Length; i++)
			{
				words = lines[i].Split(' ');
				if (words.Length == 0)
					continue;
				if (words.Length != 2)
					return false;
				int w, h;
				if (!int.TryParse(words[0], out w))
					return false;
				if (!int.TryParse(words[1], out h))
					return false;
				int end = i + h;
				int[,] s = new int[w, h];
                if (i + h > lines.Length)
                    return false;
                for (int j = 0; j < h; j++)
                {
                    i++;
					words = lines[i].Split(' ');
				    if (words.Length != w)
				        return false;
					for (int k = 0; k < w; k++)
					{
						int b;
						if (!int.TryParse(words[k], out b))
							return false;
						s[k, h - j - 1] = b;
					}
				}
				BlockType block = new BlockType(w, h, s);
				blocks.Add(block);
			}
			return true;
		}
	}
}
