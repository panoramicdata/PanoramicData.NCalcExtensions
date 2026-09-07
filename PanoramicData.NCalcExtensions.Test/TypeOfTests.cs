namespace PanoramicData.NCalcExtensions.Test;

public class TypeOfTests : NCalcTest
{
	[Theory]
	[InlineData("String", "'text'")]
	[InlineData("Int32", "1")]
	[InlineData("Double", "1.1")]
	[InlineData(null, "null")]
	public void TypeOf_ReturnsExpected(string? expected, string input)
		=> Test($"typeOf({input})").Should().Be(expected);
}
