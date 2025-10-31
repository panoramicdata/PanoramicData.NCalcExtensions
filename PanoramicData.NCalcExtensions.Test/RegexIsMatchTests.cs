namespace PanoramicData.NCalcExtensions.Test;

public class RegexIsMatchTests : NCalcTest
{
	[Fact]
	public void RegexIsMatch_True_Succeeds()
	{
		var result = Test("regexIsMatch('abc:def:2019-01-01', '^.+?:.+?:(.+)$')");
		((bool?)result).Should().BeTrue();
	}

	[Fact]
	public void RegexIsMatch_False_Succeeds()
	{
		var result = Test("regexIsMatch('YYYYYYYYYYY', '^XXXXXXXX$')");
		((bool?)result).Should().BeFalse();
	}

	[Fact]
	public void RegexIsMatch_JObject_Succeeds()
	{
		var result = Test("regexIsMatch(jPath(jObject('name', 'UK123'), 'name'), '^UK')");
		((bool?)result).Should().BeTrue();
	}
}
