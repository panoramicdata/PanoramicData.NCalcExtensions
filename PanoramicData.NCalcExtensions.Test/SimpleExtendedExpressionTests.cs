namespace PanoramicData.NCalcExtensions.Test;

public class SimpleExtendedExpressionTests : NCalcTest
{
	[Fact]
	public void Create_SimpleExpression_Evaluates()
	{
		var expression = new SimpleExtendedExpression("5 + 3");
		var result = expression.Evaluate();

		result.Should().Be(8);
	}

	[Fact]
	public void Create_WithDocument_SetsParameters()
	{
		var multilineExpression = """
// x:System.Int32:10
// y:System.Int32:5
x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		var result = expression.Evaluate();

		result.Should().Be(15);
	}

	[Fact]
	public void Create_WithCustomOptions_Evaluates()
	{
		var expression = new SimpleExtendedExpression(
			"5 + 3",
			ExpressionOptions.None,
			CultureInfo.InvariantCulture
		);
		var result = expression.Evaluate();

		result.Should().Be(8);
	}

	[Fact]
	public void SetParameter_SingleParameter_Works()
	{
		var expression = new SimpleExtendedExpression("x + 5");
		expression.SetParameter("x", 10);
		var result = expression.Evaluate();

		result.Should().Be(15);
	}

	[Fact]
	public void SetParameters_MultipleParameters_Works()
	{
		var expression = new SimpleExtendedExpression("x + y");
		expression.SetParameters(new Dictionary<string, object?> { { "x", 10 }, { "y", 5 } });
		var result = expression.Evaluate();

		result.Should().Be(15);
	}

	[Fact]
	public void Create_WithDocumentAndCustomOptions_Works()
	{
		var multilineExpression = """
// x:System.Int32:10
// y:System.Int32:5
x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(
			document.TidiedExpression,
			document,
			ExpressionOptions.None,
			CultureInfo.InvariantCulture
		);
		var result = expression.Evaluate();

		result.Should().Be(15);
	}

	[Fact]
	public void Parameters_ContainsBuiltInValues_True()
	{
		var expression = new SimpleExtendedExpression("True");
		var result = expression.Evaluate();

		result.Should().Be(true);
	}

	[Fact]
	public void Parameters_ContainsBuiltInValues_False()
	{
		var expression = new SimpleExtendedExpression("False");
		var result = expression.Evaluate();

		result.Should().Be(false);
	}

	[Fact]
	public void Parameters_ContainsBuiltInValues_Null()
	{
		var expression = new SimpleExtendedExpression("null");
		var result = expression.Evaluate();

		result.Should().BeNull();
	}

	[Fact]
	public void WithDocument_StringParameter_Works()
	{
		var multilineExpression = """
// name:System.String:John
name + ' Doe'
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		var result = expression.Evaluate();

		result.Should().Be("John Doe");
	}

	[Fact]
	public void WithDocument_BooleanParameter_Works()
	{
		var multilineExpression = """
// isActive:System.Boolean:true
isActive
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		var result = expression.Evaluate();

		result.Should().Be(true);
	}

	[Fact]
	public void WithDocument_DecimalParameter_Works()
	{
		var multilineExpression = """
// price:System.Decimal:19.99
price * 2
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		var result = expression.Evaluate();

		result.Should().Be(39.98m);
	}

	[Fact]
	public void ParseAndEvaluate_Workflow_PerformanceOptimized()
	{
		// This demonstrates the performance optimization use case:
		// 1. Parse once
		var multilineExpression = """
// x:System.Int32:10
// y:System.Int32:5

/*
# Add two numbers
*/

x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);

		// 2. Create SimpleExtendedExpression (no re-parsing needed)
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);

		// 3. Evaluate
		var result = expression.Evaluate();

		result.Should().Be(15);
		document.TidiedExpression.Should().Be("x + y");
		document.Documentation.Should().Contain("Add two numbers");
	}

	[Fact]
	public void WithDocument_OverrideParameter_Works()
	{
		var multilineExpression = """
// x:System.Int32:10
x + 5
""";
		var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);
		var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		
		// Override the parameter
		expression.SetParameter("x", 20);
		var result = expression.Evaluate();

		result.Should().Be(25);
	}

	[Fact]
	public void Constructor_WithNewline_ThrowsFormatException()
	{
		Action action = () => new SimpleExtendedExpression("x +\n5");

		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void Constructor_StartingWithDoubleSlash_ThrowsFormatException()
	{
		Action action = () => new SimpleExtendedExpression("// x:System.Int32:10");

		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void Constructor_StartingWithSlashAsterisk_ThrowsFormatException()
	{
		Action action = () => new SimpleExtendedExpression("/* doc */ x + 1");

		action.Should().Throw<FormatException>();
	}
}
