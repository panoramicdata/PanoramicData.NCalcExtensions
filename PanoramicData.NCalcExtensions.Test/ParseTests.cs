using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class ParseTests
{
	[Theory]
	[InlineData("")]
	[InlineData("'int', 1")]
	[InlineData("'xxx', '1'")]
	[InlineData("'int', '1', '1'")]
	[InlineData("1")]
	public void Parse_IncorrectParameterCountOrType_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse({parameters})");
		Assert.Throws<FormatException>(() => expression.Evaluate());
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
	public void Parse_Unparsable_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse('{parameters}', 'x')");
		Assert.Throws<FormatException>(() => expression.Evaluate());
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
}
