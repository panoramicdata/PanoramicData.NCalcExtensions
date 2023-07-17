namespace PanoramicData.NCalcExtensions.Test;

public class TryParseTests
{
	[Theory]
	[InlineData("")]
	[InlineData("'int', 1")]
	[InlineData("'xxx', '1'")]
	[InlineData("'Guid', '1'")]
	[InlineData("'Guid', '1', 'x', 'y'")]
	[InlineData("1")]
	public void TryParse_IncorrectParameterCountOrType_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"tryParse({parameters})");
		Assert.Throws<FormatException>(expression.Evaluate);
	}

	[Theory]
	[InlineData("short")]
	[InlineData("ushort")]
	[InlineData("long")]
	[InlineData("ulong")]
	[InlineData("int")]
	[InlineData("uint")]
	[InlineData("byte")]
	[InlineData("sbyte")]
	[InlineData("double")]
	[InlineData("float")]
	[InlineData("decimal")]
	[InlineData("jObject")]
	[InlineData("jArray")]
	[InlineData("Guid")]
	public void TryParse_DoesNotParse_ReturnsFalse(string parameters)
	{
		var expression = new ExtendedExpression($"tryParse('{parameters}', 'x', 'outputVariable')");
		expression.Evaluate().Should().Be(false);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("false")]
	[InlineData("False")]
	public void TryParse_Bool_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('bool', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Int_Succeeds(string text)
	{
		var expectedValue = int.TryParse(text, out var expected) ? expected : (int?)null;
		var expression = new ExtendedExpression($"if(tryParse('int', '{text}', 'outputVariable'), retrieve('outputVariable'), null)");
		var result = expression.Evaluate();
		result.Should().NotBe(null);
		result.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("BC2BF06D-1B6A-4D2F-A7D2-9D160EB2AE12")] // Upper case
	[InlineData("{D72C2BAF-0F89-44E0-8083-4A8102425429}")] // Upper case with curlies
	[InlineData("fa2e60e8-dd1e-4cbb-b53c-e69c63f14866")] // Lower case
	[InlineData("{fa2e60e8-dd1e-4cbb-b53c-e69c63f14866}")] // Lower case with curlies
	[InlineData("fa2e60e8-dd1e-AAAA-b53c-e69c63f14866")] // Mixed case
	[InlineData("{fa2e60e8-dd1e-AAAA-b53c-e69c63f14866}")] // Mixed case with curlies
	public void TryParse_Guid_Succeeds(string text)
	{
		var expectedValue = Guid.TryParse(text, out var expected) ? expected : (Guid?)null;
		var expression = new ExtendedExpression($"if(tryParse('Guid', '{text}', 'outputVariable'), retrieve('outputVariable'), null)");
		var result = expression.Evaluate();
		result.Should().NotBe(null);
		result.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Long_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('long', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void TryParse_ULong_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('ulong', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void TryParse_UInt_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('uint', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Double_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('double', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void TryParse_Float_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('float', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

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
