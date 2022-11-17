namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertyTests
{
	[Fact]
	public void GetProperty()
	{
		var year = 2019;
		var expression = new ExtendedExpression($"getProperty(toDateTime('{year}-01-01', 'yyyy-MM-dd'), 'Year')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(year);
	}
}
