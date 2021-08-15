using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication4;

namespace TestProject2
{

		[TestClass]
		public class UnitTest1
		{
			Class _calc;

			public UnitTest1()
			{
				_calc = new Class();
			}
			[TestMethod]
			public void shouldaddtwonumbers()
			{
				int res = _calc.Add(5, 4);
				Assert.AreEqual(res, 9);
			}
			[TestMethod]
			public void shouldsubtwonumbers()
			{
				int res = _calc.Sub(5, 3);
				Assert.AreEqual(res, 2);
			}
		}
	}

