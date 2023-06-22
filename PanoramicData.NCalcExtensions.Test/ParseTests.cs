using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class ParseTests
{
	[Theory]
	[InlineData("")]
	[InlineData("'int', 1")]
	[InlineData("'xxx', '1'")]
	[InlineData("'Guid', '1'")]
	[InlineData("1")]
	public void Parse_IncorrectParameterCountOrType_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse({parameters})");
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
	public void Parse_Unparsable_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse('{parameters}', 'x')");
		Assert.Throws<FormatException>(expression.Evaluate);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("false")]
	[InlineData("False")]
	public void Parse_Bool_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('bool', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(bool.Parse(text));
	}

	[Theory]
	[InlineData("ttrue")]
	[InlineData("Truex")]
	[InlineData("x")]
	[InlineData("")]
	public void Parse_Bool_InvalidInput_Succeeds(string? text)
	{
		var expression = new ExtendedExpression($"parse('bool', a, null)");
		expression.Parameters["a"] = text;
		var result = expression.Evaluate();
		result.Should().BeNull();
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Int_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('int', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(int.Parse(text));
	}

	[Theory]
	[InlineData("BC2BF06D-1B6A-4D2F-A7D2-9D160EB2AE12")] // Upper case
	[InlineData("{D72C2BAF-0F89-44E0-8083-4A8102425429}")] // Upper case with curlies
	[InlineData("fa2e60e8-dd1e-4cbb-b53c-e69c63f14866")] // Lower case
	[InlineData("{fa2e60e8-dd1e-4cbb-b53c-e69c63f14866}")] // Lower case with curlies
	[InlineData("fa2e60e8-dd1e-AAAA-b53c-e69c63f14866")] // Mixed case
	[InlineData("{fa2e60e8-dd1e-AAAA-b53c-e69c63f14866}")] // Mixed case with curlies
	public void Parse_Guid_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('Guid', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(Guid.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Long_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('long', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(long.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_ULong_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('ulong', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(ulong.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_UInt_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('uint', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(uint.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Double_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('double', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(double.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Float_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('float', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(float.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Decimal_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('decimal', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(decimal.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_SByte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('sbyte', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(sbyte.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_Byte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('byte', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(byte.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Short_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('short', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(short.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_Ushort_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('ushort', '{text}')");
		var result = expression.Evaluate();
		result.Should().Be(ushort.Parse(text));
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	public void Parse_JObject_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('jObject', '{text}')");
		var result = expression.Evaluate();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[\"a\",1]")]
	public void Parse_JArray_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('jArray', '{text}')");
		var result = expression.Evaluate();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	public void Parse_CapitalJ_JObject_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('JObject', '{text}')");
		var result = expression.Evaluate();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[\"a\",1]")]
	public void Parse_CapitalJ_JArray_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('JArray', '{text}')");
		var result = expression.Evaluate();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}
}
