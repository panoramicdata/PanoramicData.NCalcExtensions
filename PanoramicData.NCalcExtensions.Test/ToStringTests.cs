namespace PanoramicData.NCalcExtensions.Test;

public class ToStringTests
{
	[Fact]
	public void ToString_IsNull_ReturnsNull()
	{
		var expression = new ExtendedExpression("toString(null)");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void ToString_Int_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1)");
		expression.Evaluate().Should().Be("1");
	}

	[Fact]
	public void ToString_Int_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1000, 'N2')");
		expression.Evaluate().Should().Be("1,000.00");
	}

	[Fact]
	public void ToString_DateTime_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 1, 1));
		expression.Evaluate().Should().Be("2020-01-01");
	}

	[Fact]
	public void ToString_DateTimeOffset_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTimeOffset, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTimeOffset", new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
		expression.Evaluate().Should().Be("2020-01-01");
	}
}
