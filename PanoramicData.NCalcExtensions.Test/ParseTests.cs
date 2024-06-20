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
		=> new ExtendedExpression($"parse({parameters})")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

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
	public void Parse_DoesNotParse_Throws(string parameters)
		=> new ExtendedExpression($"parse('{parameters}', 'x')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("false")]
	[InlineData("False")]
	public void Parse_Bool_Succeeds(string text)
	{
		new ExtendedExpression($"parse('bool', '{text}')")
				.Evaluate()
				.Should().Be(bool.Parse(text));

		new ExtendedExpression($"parse('System.Boolean', '{text}')")
			.Evaluate()
			.Should().Be(bool.Parse(text));
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
		var result = new ExtendedExpression($"parse('int', '{text}')").Evaluate();
		result.Should().Be(int.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Int32', '{text}')").Evaluate();
		result.Should().Be(int.Parse(text, CultureInfo.InvariantCulture));
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
		var result = new ExtendedExpression($"parse('Guid', '{text}')").Evaluate();
		result.Should().Be(Guid.Parse(text));
		result = new ExtendedExpression($"parse('System.Guid', '{text}')").Evaluate();
		result.Should().Be(Guid.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Long_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('long', '{text}')").Evaluate();
		result.Should().Be(long.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Int64', '{text}')").Evaluate();
		result.Should().Be(long.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_ULong_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('ulong', '{text}')").Evaluate();
		result.Should().Be(ulong.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.UInt64', '{text}')").Evaluate();
		result.Should().Be(ulong.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_UInt_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('uint', '{text}')").Evaluate();
		result.Should().Be(uint.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.UInt32', '{text}')").Evaluate();
		result.Should().Be(uint.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Double_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('double', '{text}')").Evaluate();
		result.Should().Be(double.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Double', '{text}')").Evaluate();
		result.Should().Be(double.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Float_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('float', '{text}')").Evaluate();
		result.Should().Be(float.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Single', '{text}')").Evaluate();
		result.Should().Be(float.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Decimal_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('decimal', '{text}')").Evaluate();
		result.Should().Be(decimal.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Decimal', '{text}')").Evaluate();
		result.Should().Be(decimal.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_SByte_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('sbyte', '{text}')").Evaluate();
		result.Should().Be(sbyte.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.SByte', '{text}')").Evaluate();
		result.Should().Be(sbyte.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_Byte_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('byte', '{text}')").Evaluate();
		result.Should().Be(byte.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Byte', '{text}')").Evaluate();
		result.Should().Be(byte.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public void Parse_Short_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('short', '{text}')").Evaluate();
		result.Should().Be(short.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.Int16', '{text}')").Evaluate();
		result.Should().Be(short.Parse(text, CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public void Parse_Ushort_Succeeds(string text)
	{
		var result = new ExtendedExpression($"parse('ushort', '{text}')").Evaluate();
		result.Should().Be(ushort.Parse(text, CultureInfo.InvariantCulture));

		result = new ExtendedExpression($"parse('System.UInt16', '{text}')").Evaluate();
		result.Should().Be(ushort.Parse(text, CultureInfo.InvariantCulture));
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
