using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class AverageTests
{
	[Fact]
	public void Average_OfIntegers_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression("average(x)");
		expression.Parameters.Add("x", new List<int> { 1, 2, 3, 4, 5 });
		expression.Evaluate().Should().Be(3.0);
	}

	[Fact]
	public void Average_OfDoubles_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression("average(x)");
		expression.Parameters.Add("x", new List<double> { 1.0, 2.0, 3.0 });
		expression.Evaluate().Should().Be(2.0);
	}

	[Fact]
	public void Average_OfObjects_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression("average(x)");
		expression.Parameters.Add("x", new List<object?> { 10, 20, 30 });
		expression.Evaluate().Should().Be(20.0);
	}

	[Fact]
	public void Average_EmptyList_ReturnsNaN()
	{
		var expression = new ExtendedExpression("average(x)");
		expression.Parameters.Add("x", new List<int>());
		((double)expression.Evaluate()!).Should().Be(double.NaN);
	}

	[Fact]
	public void Average_SingleElement_ReturnsThatElement()
	{
		var expression = new ExtendedExpression("average(x)");
		expression.Parameters.Add("x", new List<int> { 42 });
		expression.Evaluate().Should().Be(42.0);
	}

	[Fact]
	public void Average_UsingListOf_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression("average(listOf('int', 2, 4, 6))");
		expression.Evaluate().Should().Be(4.0);
	}

	[Fact]
	public void Average_NullParameter_ThrowsException()
		=> new ExtendedExpression("average(null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*average*cannot be null*");
}
