namespace PanoramicData.NCalcExtensions.Test;

public class RegexIsMatchTests : NCalcTest
{
	[Fact]
	public void RegexIsMatch_True_Succeeds()
	{
		var result = Test("regexIsMatch('abc:def:2019-01-01', '^.+?:.+?:(.+)$')");
		Assert.True((bool)result);
	}

	[Fact]
	public void RegexIsMatch_False_Succeeds()
	{
		var result = Test("regexIsMatch('YYYYYYYYYYY', '^XXXXXXXX$')");
		Assert.False((bool)result);
	}

	[Fact]
	public void RegexIsMatch_JObject_Succeeds()
	{
		var result = Test("regexIsMatch(jPath(jObject('name', 'UK123'), 'name'), '^UK')");
		Assert.True((bool)result);
	}
}
