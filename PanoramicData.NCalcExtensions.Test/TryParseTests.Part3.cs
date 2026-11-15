namespace PanoramicData.NCalcExtensions.Test;

public partial class TryParseTests : NCalcTest
{
	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Decimal_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('decimal', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_SByte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('sbyte', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void TryParse_Byte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('byte', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Short_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('short', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void TryParse_Ushort_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('ushort', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	public void TryParse_JObject_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('jObject', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[\"a\",1]")]
	public void TryParse_JArray_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('jArray', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	public void TryParse_CapitalJ_JObject_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('JObject', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[\"a\",1]")]
	public void TryParse_CapitalJ_JArray_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('JArray', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}
}
