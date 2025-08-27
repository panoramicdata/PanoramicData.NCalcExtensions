using NCalc;
using NCalc.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PanoramicData.NCalcExtensions.Test;

public class ExtendedExpressionTests : NCalcTest
{
	[Fact]
	public void Adding_Reserverd_Keywords_Must_Fail()
	{
		Assert.Throws<InvalidOperationException>(() =>
		{
			_ = new ExtendedExpression("expression", new ExpressionContext
			{
				StaticParameters = new Dictionary<string, object?>
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
	public void Adding_Reserverd_Keywords_The_Same_Must_Not_Fail() => _ = new ExtendedExpression("expression", new ExpressionContext
	{
		StaticParameters = new Dictionary<string, object?>
			{
				{ "null", null },
				{ "True", true },
				{ "False", false },
				{ "EMPTYQUOTES", string.Empty }
			}
	});

	[Fact]
	public void Custom_Async_Function_Must_Not_Fail()
	{
		void fnReturnNull(FunctionArgs arguments)
		{
			Thread.Sleep(10);
			arguments.Result = null;
		}

		void fnReturnValue(FunctionArgs arguments)
		{
			var val = arguments.Parameters.First().Evaluate();
			Thread.Sleep(10);
			arguments.Result = val;
		}

		var expression = new ExtendedExpression("nullCoalesce(fnReturnNull(), fnReturnValue(something))", new ExpressionContext
		{
			StaticParameters = {
				{ "something", "else" }
			},
			EvaluateFunctionHandler = (name, args) => {
				if (name == "fnReturnNull")
				{
					fnReturnNull(args);
				}

				if (name == "fnReturnValue")
				{
					fnReturnValue(args);
				}
			}
		});

		var value = expression.Evaluate();
		Assert.Equal("else", value);
	}
}
