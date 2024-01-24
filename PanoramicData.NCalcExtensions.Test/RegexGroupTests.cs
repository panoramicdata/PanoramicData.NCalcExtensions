namespace PanoramicData.NCalcExtensions.Test;

public class RegexGroupTests : NCalcTest
{
	[Fact]
	public void RegexGroup_Simple_Succeeds()
	{
		var result = Test("regexGroup('abc:def:2019-01-01', '^.+?:.+?:(.+)$')");
		result.Should().Be("2019-01-01");
	}

	[Fact]
	public void RegexGroup_Succeeds()
	{
		var result = Test("regexGroup('abc:def:2019-01-01', '^.+?:.+?:(.+)$') < dateTime('UTC', 'yyyy-MM-dd', -30, 0, 0, 0)");
		(result as bool?).Should().NotBeNull();
		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void RegexGroupN_Succeeds()
	{
		var result = Test("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$')");
		result.Should().Be("X");
		result = Test("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 1)");
		result.Should().Be("Y");
		result = Test("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 2)");
		result.Should().Be("Z");
		result = Test("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 3)");
		result.Should().BeNull();
	}

	[Fact]
	public void RegexGroup_NotSoSimple_Succeeds()
	{
		var text = "AutoTask: xxx.xxx@xxx.com at 2022-05-10 08:26:57 UTC (37407238)\nTitle: Notification Description: xxx@xxx.xxx";
		var expression = new ExtendedExpression("regexGroup(text, 'AutoTask: .+ \\\\((\\\\d+)\\\\)\\\\n')");
		expression.Parameters["text"] = text;
		var result = expression.Evaluate();

		result.Should().Be("37407238");
	}

}
