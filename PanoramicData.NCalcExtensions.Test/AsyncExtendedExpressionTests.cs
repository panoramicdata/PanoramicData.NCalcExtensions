using NCalc;
using NCalc.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanoramicData.NCalcExtensions.Test;

public class AsyncExtendedExpressionTests : NCalcTest
{
	[Fact]
	public void Adding_Reserverd_Keywords_Must_Fail()
	{
		Assert.Throws<InvalidOperationException>(() =>
		{
			_ = new AsyncExtendedExpression("expression", new AsyncExpressionContext
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
	public void Adding_Reserverd_Keywords_The_Same_Must_Not_Fail() => _ = new AsyncExtendedExpression("expression", new AsyncExpressionContext
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
	public async Task Custom_Async_Function_Must_Not_Fail()
	{
		async Task fnReturnNull(AsyncFunctionArgs arguments)
		{
			await Task.Delay(10).ConfigureAwait(false);
			arguments.Result = null;
		}

		async Task fnReturnValue(AsyncFunctionArgs arguments)
		{
			var val = await arguments.Parameters.First().EvaluateAsync().ConfigureAwait(false);
			await Task.Delay(10).ConfigureAwait(false);
			arguments.Result = val;
		}

		var expression = new AsyncExtendedExpression("nullCoalesce(fnReturnNull(), fnReturnValue(something))", new AsyncExpressionContext
		{
			StaticParameters = { 
				{ "something", "else" } 
			},
			AsyncEvaluateFunctionHandler = async (name, args) => {
				if (name == "fnReturnNull")
				{
					await fnReturnNull(args).ConfigureAwait(false);
				}

				if (name == "fnReturnValue")
				{
					await fnReturnValue(args).ConfigureAwait(false);
				}
			}
		});

		var value = await expression.EvaluateAsync();
		Assert.Equal("else", value);
	}
}