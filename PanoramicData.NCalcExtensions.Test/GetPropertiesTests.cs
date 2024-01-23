using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertiesTests
{
	[Fact]
	public void GetProperties()
	{
		var year = 2019;
		var expression = new ExtendedExpression($"getProperties(toDateTime('{year}-01-01', 'yyyy-MM-dd'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<string>>();
		result.Should().BeEquivalentTo(new[] { "Date", "Day", "DayOfWeek", "DayOfYear", "Hour", "Kind", "Millisecond", "Microsecond", "Nanosecond", "Minute", "Month", "Now", "Second", "Ticks", "TimeOfDay", "Today", "Year", "UtcNow" });
	}

	[Fact]
	public void GetProperties_FromJObject()
	{
		var expression = new ExtendedExpression("getProperties(parse('jObject', '{ \"A\": 1, \"B\": 2 }'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<string>>();
		result.Should().BeEquivalentTo(new[] { "A", "B" });
	}
}
