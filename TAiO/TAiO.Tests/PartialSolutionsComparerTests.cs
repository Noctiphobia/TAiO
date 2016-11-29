using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TAiO.Model;
namespace TAiO.Tests
{
	[TestClass]
	public class PartialSolutionsComparerTests
	{
		[TestMethod]
		public void GreaterTest()
		{
			PartialSolution s1 = new PartialSolution {Cost = 10};
			PartialSolution s2 = new PartialSolution {Cost = 9};
			Assert.AreEqual(1, new PartialSolutionsComparer().Compare(s1, s2));
		}
		[TestMethod]
		public void SmallerTest()
		{
			PartialSolution s1 = new PartialSolution { Cost = 9 };
			PartialSolution s2 = new PartialSolution { Cost = 10 };
			Assert.AreEqual(-1, new PartialSolutionsComparer().Compare(s1, s2));
		}
		[TestMethod]
		public void EqualTest()
		{
			PartialSolution s1 = new PartialSolution { Cost = 10 };
			PartialSolution s2 = new PartialSolution { Cost = 10 };
			Assert.AreEqual(1, new PartialSolutionsComparer().Compare(s1, s2));
		}
	}
}
