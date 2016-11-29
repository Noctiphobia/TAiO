using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Tools;
using TAiO.Model;
namespace TAiO.Tests
{
	[TestClass]
	public class ParserTests
	{
		public ParserTests()
		{
			if (File.Exists("ParserTest"))
				File.Delete("ParserTest");
			using (FileStream f = File.Create("ParserTest"))
			{
				using (TextWriter tw = new StreamWriter(f))
				{
					tw.WriteLine("20 1");
					tw.WriteLine("4 3");
					tw.WriteLine("1 1 1 1");
					tw.WriteLine("1 0 1 0");
					tw.WriteLine("1 0 1 0");
				}
			}
		}

		[TestMethod]
		public void ParserTest()
		{
			List<BlockType> list;
			int w;
			Parser.ParseFile("ParserTest", out list, out w);
			Assert.AreEqual(20, w);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(4, list[0].Width);
			Assert.AreEqual(3, list[0].Height);
			int[,] reference = new int[4, 3]
			{		
				{1, 1, 1},
				{0, 0, 1},
				{1, 1, 1},
				{0, 0, 1},
			};
			for (int i=0; i<4; ++i)
				for (int j=0; j<3; ++j)
					Assert.AreEqual(reference[i,j], list[0].Shape[0][i,j]);
		}

		[TestCleanup]
		public void CleanUp()
		{
			File.Delete("ParserTest");
		}
	}
}
