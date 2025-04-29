using NCalc;
using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class AsyncExtendedExpressionTests : NCalcTest
{
	[Fact]
	public void Adding_Reserverd_Keywords_Must_Fail()
	{
		Assert.Throws<InvalidOperationException>(() =>
		{
			var expr = new AsyncExtendedExpression("expression", new AsyncExpressionContext
			{
				StaticParameters = new Dictionary<string, object?>()
				{
					{ "null", "notnull" },
					{ "True", false },
					{ "False", true },
					{ "EMPTYQUOTES", "something" }
				}
			});
		});
	}

	[Fact]
	public void Adding_Reserverd_Keywords_The_Same_Must_Not_Fail()
	{
		var expr = new AsyncExtendedExpression("expression", new AsyncExpressionContext
		{
			StaticParameters = new Dictionary<string, object?>()
			{
				{ "null", null },
				{ "True", true },
				{ "False", false },
				{ "EMPTYQUOTES", string.Empty }
			}
		});
	}
}