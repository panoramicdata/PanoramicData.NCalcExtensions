namespace PanoramicData.NCalcExtensions.Test;

public class StructuredAnswerDefinitionTests
{
	public static TheoryData<string> StructuredTypeNames => new()
	{
		"List<string?>",
		"List<int?>",
		"List<object>",
		"List<JObject>",
		"Dictionary<string, object>",
		"Dictionary<string, object?>",
		"JObject",
		"JArray",
		"JsonDocument",
		"DateTimeOffset",
		"Type",
		"Exception",
		"object?",
	};

	[Theory]
	[MemberData(nameof(StructuredTypeNames))]
	public void Parse_TypeOnlyAnswer_IsRecognised(string typeName)
	{
		var document = ExtendedExpressionDocumentParser.Parse($"// answer:{typeName}\r\nnull");

		document.Answer.Should().Be(TypedDefinition.FromTypeOnly(typeName));
		document.Comments.Should().BeEmpty();
	}

	[Theory]
	[InlineData("List<string?>", "[\"a\",null,\"b\"]")]
	[InlineData("List<int?>", "[1,null,3]")]
	[InlineData("Dictionary<string, object?>", "{\"KEY1\":\"Hello\",\"KEY2\":false}")]
	[InlineData("JObject", "{\"a\":1,\"b\":null}")]
	[InlineData("JArray", "[1,null,\"woo\"]")]
	[InlineData("JsonDocument", "[{\"a\":1}]")]
	[InlineData("DateTimeOffset", "9999-12-31T23:59:59.9999999+00:00")]
	[InlineData("Type", "PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException")]
	public void Parse_AnswerWithValue_KeepsTheValueText(string typeName, string value)
	{
		var document = ExtendedExpressionDocumentParser.Parse($"// answer:{typeName}:{value}\r\nnull");

		document.Answer.Should().Be(TypedDefinition.FromLiteral(typeName, value));
	}

	[Fact]
	public void Parse_ObjectNullAnswer_IsAnExplicitNull()
	{
		var document = ExtendedExpressionDocumentParser.Parse("// answer:object?:null\r\nnull");

		document.Answer.Should().Be(TypedDefinition.FromNull("object?"));
	}

	[Theory]
	[InlineData("JObject", "{not json")]
	[InlineData("List<int?>", "1,2")]
	[InlineData("DateTimeOffset", "tomorrow")]
	public void Parse_AnswerWithInvalidValue_IsOnlyAComment(string typeName, string value)
	{
		var document = ExtendedExpressionDocumentParser.Parse($"// answer:{typeName}:{value}\r\nnull");

		document.Answer.Should().BeNull();
	}

	[Theory]
	[InlineData("Dictionary<int, object>")]
	[InlineData("List<NotAType>")]
	[InlineData("List<string, string>")]
	public void Parse_UnsupportedGenericAnswer_IsOnlyAComment(string typeName)
	{
		var document = ExtendedExpressionDocumentParser.Parse($"// answer:{typeName}\r\nnull");

		document.Answer.Should().BeNull();
	}

	[Fact]
	public void ConvertDefinitionValue_JsonParameterWithValue_ReportsUnsupported()
	{
		Action act = () => new ExtendedExpression("// x:JObject:{\"a\":1}\r\nx").Evaluate();

		act.Should().Throw<FormatException>().WithMessage("*not supported*");
	}

	[Fact]
	public void ConvertDefinitionValue_DateTimeOffsetParameter_ParsesTheValue()
	{
		var expression = new ExtendedExpression("// when:DateTimeOffset:2026-01-02T03:04:05+01:00\r\nwhen");

		expression.Evaluate().Should().Be(new DateTimeOffset(2026, 1, 2, 3, 4, 5, TimeSpan.FromHours(1)));
	}
}
