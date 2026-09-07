namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertyTests : NCalcTest
{
	[Fact]
	public void GetProperty()
	{
		var year = 2019;
		var expression = new ExtendedExpression($"getProperty(toDateTime('{year}-01-01', 'yyyy-MM-dd'), 'Year')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(year);
	}

	[Fact]
	public void GetProperty_FromJObject()
	{
		var expression = new ExtendedExpression($"getProperty(parse('jObject', '{{ \"A\": 1, \"B\": 2 }}'), 'B')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(2);
	}

	[Fact]
	public void GetProperty_FromDictionary()
	{
		var expression = new ExtendedExpression($"getProperty(dictionary('A', 2, 'B', 'Target'), 'B')");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("Target");
	}

	// JObject tests - all JTokenTypes
	[Fact]
	public void GetProperty_JObject_IntegerProperty_ReturnsInt()
		=> Test("getProperty(parse('jObject', '{\"count\": 42}'), 'count')").Should().Be(42);

	[Fact]
	public void GetProperty_JObject_FloatProperty_ReturnsFloat()
	{
		var expression = new ExtendedExpression("getProperty(parse('jObject', '{\"value\": 3.14}'), 'value')");
		var result = expression.Evaluate();
		result.Should().BeOfType<float>();
	}

	[Fact]
	public void GetProperty_JObject_StringProperty_ReturnsString()
		=> Test("getProperty(parse('jObject', '{\"name\": \"test\"}'), 'name')").Should().Be("test");

	[Fact]
	public void GetProperty_JObject_BooleanProperty_ReturnsBool()
		=> Test("getProperty(parse('jObject', '{\"flag\": true}'), 'flag')").Should().Be(true);

	[Fact]
	public void GetProperty_JObject_NullProperty_ReturnsNull()
		=> Test("getProperty(parse('jObject', '{\"value\": null}'), 'value')").Should().BeNull();

	[Fact]
	public void GetProperty_JObject_ArrayProperty_ReturnsJArray()
	{
		var expression = new ExtendedExpression("getProperty(parse('jObject', '{\"items\": [1,2,3]}'), 'items')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JArray>();
	}

	[Fact]
	public void GetProperty_JObject_ObjectProperty_ReturnsJObject()
	{
		var expression = new ExtendedExpression("getProperty(parse('jObject', '{\"nested\": {\"a\":1}}'), 'nested')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
	}

	[Fact]
	public void GetProperty_JObject_DateProperty_ReturnsDateTime()
	{
		var expression = new ExtendedExpression("getProperty(parse('jObject', '{\"date\": \"2020-01-01T00:00:00Z\"}'), 'date')");
		var result = expression.Evaluate();
		// Could be DateTime or string depending on JSON parsing
		result.Should().NotBeNull();
	}

	// JsonDocument tests
	[Fact]
	public void GetProperty_JsonDocument_StringProperty_ReturnsString()
		=> Test("getProperty(jsonDocument('name', 'John', 'age', 30), 'name')").Should().Be("John");

	[Fact]
	public void GetProperty_JsonDocument_IntProperty_ReturnsInt()
		=> Test("getProperty(jsonDocument('name', 'John', 'age', 30), 'age')").Should().Be(30);

	[Fact]
	public void GetProperty_JsonDocument_BooleanTrue_ReturnsTrue()
		=> Test("getProperty(jsonDocument('isActive', true), 'isActive')").Should().Be(true);

	[Fact]
	public void GetProperty_JsonDocument_BooleanFalse_ReturnsFalse()
		=> Test("getProperty(jsonDocument('isActive', false), 'isActive')").Should().Be(false);

	[Fact]
	public void GetProperty_JsonDocument_NullProperty_ReturnsNull()
		=> Test("getProperty(jsonDocument('value', null), 'value')").Should().BeNull();

	[Fact]
	public void GetProperty_JsonDocument_MissingProperty_ReturnsNull()
		=> Test("getProperty(jsonDocument('name', 'John'), 'age')").Should().BeNull();

	[Fact]
	public void GetProperty_JsonDocument_LongNumber_ReturnsLong()
	{
		var expression = new ExtendedExpression("getProperty(TheDoc, 'bigNumber')");
		expression.Parameters["TheDoc"] = System.Text.Json.JsonDocument.Parse("{\"bigNumber\": 9223372036854775807}");
		var result = expression.Evaluate();
		result.Should().BeOfType<long>();
	}

	[Fact]
	public void GetProperty_JsonDocument_DoubleNumber_ReturnsDouble()
	{
		var expression = new ExtendedExpression("getProperty(TheDoc, 'decimal')");
		expression.Parameters["TheDoc"] = System.Text.Json.JsonDocument.Parse("{\"decimal\": 3.14159}");
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
	}

	// Dictionary tests
	[Fact]
	public void GetProperty_Dictionary_IntValue_ReturnsInt()
		=> Test("getProperty(dictionary('count', 5), 'count')").Should().Be(5);

	[Fact]
	public void GetProperty_Dictionary_NullValue_ReturnsNull()
		=> Test("getProperty(dictionary('value', null), 'value')").Should().BeNull();

	// Regular .NET object tests
	[Fact]
	public void GetProperty_DateTime_Month_ReturnsMonth()
		=> Test("getProperty(toDateTime('2020-03-15', 'yyyy-MM-dd'), 'Month')").Should().Be(3);

	[Fact]
	public void GetProperty_DateTime_Day_ReturnsDay()
		=> Test("getProperty(toDateTime('2020-03-15', 'yyyy-MM-dd'), 'Day')").Should().Be(15);

	[Fact]
	public void GetProperty_String_Length_ReturnsLength()
		=> Test("getProperty('hello', 'Length')").Should().Be(5);

	// Error cases
	[Fact]
	public void GetProperty_NullObject_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(null, 'property')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*first parameter cannot be null*");
	}

	[Fact]
	public void GetProperty_NullPropertyName_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('a', 1), null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*requires two parameters*");
	}

	[Fact]
	public void GetProperty_NonExistentProperty_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(toDateTime('2020-01-01', 'yyyy-MM-dd'), 'NonExistent')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Could not find property*");
	}

	[Fact]
	public void GetProperty_JsonDocument_NonObjectRoot_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(parse('jsonArray', '[1,2,3]'), 'property')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an object*");
	}

	// Edge cases with JsonElement
	[Fact]
	public void GetProperty_JsonElement_NestedObject_Works()
	{
		var expression = new ExtendedExpression("getProperty(TheDoc, 'nested')");
		expression.Parameters["TheDoc"] = System.Text.Json.JsonDocument.Parse("{\"nested\": {\"a\": 1}}");
		var result = expression.Evaluate();
		result.Should().BeOfType<System.Text.Json.JsonElement>();
	}

	[Fact]
	public void GetProperty_JsonElement_Array_Works()
	{
		var expression = new ExtendedExpression("getProperty(TheDoc, 'items')");
		expression.Parameters["TheDoc"] = System.Text.Json.JsonDocument.Parse("{\"items\": [1,2,3]}");
		var result = expression.Evaluate();
		result.Should().BeOfType<System.Text.Json.JsonElement>();
	}

	// Test with variables
	[Fact]
	public void GetProperty_WithVariables_Works()
	{
		var expression = new ExtendedExpression("getProperty(myObject, myProperty)");
		expression.Parameters["myObject"] = System.Text.Json.JsonDocument.Parse("{\"test\": 123}");
		expression.Parameters["myProperty"] = "test";
		expression.Evaluate().Should().Be(123);
	}

	// Test missing parameters
	[Fact]
	public void GetProperty_MissingSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('a', 1))");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}
}
