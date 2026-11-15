namespace PanoramicData.NCalcExtensions.Test;

public class LambdaInheritanceTests : NCalcTest
{
	[Fact]
	public void Where_CustomFunctionInLambda_IsResolvedByTheCallersHandler()
	{
		var expression = new ExtendedExpression("count(where(list(1, 2, 3, 4), 'n', 'double(n) > 4'))");
		expression.EvaluateFunction += (name, args) =>
		{
			if (name == "double")
			{
				args.Result = Convert.ToInt32(args.Parameters.Evaluate(0), CultureInfo.InvariantCulture) * 2;
			}
		};

		expression.Evaluate().Should().Be(2);
	}

	[Fact]
	public void Any_CustomParameterInLambda_IsResolvedByTheCallersHandler()
	{
		var expression = new ExtendedExpression("any(list(1, 2, 3), 'n', 'n == limit')");
		expression.EvaluateParameter += (name, args) =>
		{
			if (name == "limit")
			{
				args.Result = 3;
			}
		};

		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_LambdaUsesTheExpressionOptionsOfTheCaller()
	{
		var expression = new ExtendedExpression(
			"""any(list(1), 'n', '("a" + n) == "a1"')""",
			ExpressionOptions.StringConcat,
			CultureInfo.InvariantCulture);

		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Select_LambdaUsesTheCultureOfTheCaller()
	{
		var expression = new ExtendedExpression(
			"first(select(list(1), 'n', 'n + 0.5'))",
			ExpressionOptions.None,
			CultureInfo.GetCultureInfo("de-DE"));

		expression.Evaluate().Should().Be(1.5);
	}

	[Fact]
	public void Where_BuiltInFunctionsAreNotEvaluatedTwice()
	{
		var calls = 0;
		var expression = new ExtendedExpression("""count(where(list(1, 2), 'n', 'n > 0 && !isNull(n)'))""");
		expression.EvaluateFunction += (name, _) => calls += name == "isNull" ? 1 : 0;

		expression.Evaluate().Should().Be(2);
		calls.Should().Be(0);
	}
}
