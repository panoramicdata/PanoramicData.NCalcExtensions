namespace PanoramicData.NCalcExtensions.Test;

public class ExtendedExpressionDocumentTests : NCalcTest
{
	[Fact]
	public void Parse_SimpleExpression_ReturnsDocument()
	{
		var expression = "5 + 3";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.OriginalExpression.Should().Be(expression);
		document.TidiedExpression.Should().Be("5 + 3");
		document.Documentation.Should().BeNullOrEmpty();
		document.Parameters.Should().BeEmpty();
		document.Comments.Should().BeEmpty();
	}

	[Fact]
	public void Parse_WithParameters_ExtractsParameters()
	{
		var expression = """
// x:System.Int32:10
// y:System.Int32:5
x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Parameters.Should().HaveCount(2);
		document.Parameters["x"].Should().Be(("System.Int32", "10"));
		document.Parameters["y"].Should().Be(("System.Int32", "5"));
		document.TidiedExpression.Should().Be("x + y");
	}

	[Fact]
	public void Parse_WithDocumentation_ExtractsDocumentation()
	{
		var expression = """
/*
# Add two numbers
This adds two pre-defined parameters together.
*/
x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Documentation.Should().Contain("Add two numbers");
		document.Documentation.Should().Contain("pre-defined parameters");
	}

	[Fact]
	public void Parse_WithParametersAndDocumentation_ExtractsAll()
	{
		var expression = """
// x:System.Int32:10
// y:System.Int32:5

/*
# Add two numbers
This adds x and y together.
*/

x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Parameters.Should().HaveCount(2);
		document.Parameters["x"].Should().Be(("System.Int32", "10"));
		document.Parameters["y"].Should().Be(("System.Int32", "5"));
		document.Documentation.Should().Contain("Add two numbers");
		document.TidiedExpression.Should().Be("x + y");
	}

	[Fact]
	public void Parse_WithRegularComments_ExtractsComments()
	{
		var expression = """
// This is a regular comment
// Another comment
5 + 3
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Comments.Should().HaveCount(2);
		document.Comments.Should().Contain("This is a regular comment");
		document.Comments.Should().Contain("Another comment");
		document.TidiedExpression.Should().Be("5 + 3");
	}

	[Fact]
	public void Parse_WithMultipleCommentTypes_SeparatesCorrectly()
	{
		var expression = """
// x:System.Int32:10
// Just a comment
// y:System.String:hello
x + y
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Parameters.Should().HaveCount(2);
		document.Comments.Should().HaveCount(1);
		document.Comments[0].Should().Be("Just a comment");
	}

	[Fact]
	public void Parse_WithMultilineComment_IgnoresInnerComments()
	{
		var expression = """
// x:System.Int32:10

/*
# Calculate result
// This line inside the comment should not be parsed as a parameter
Another documentation line
*/

x + 1
""";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.Parameters.Should().HaveCount(1);
		document.Parameters["x"].Should().Be(("System.Int32", "10"));
		document.Documentation.Should().Contain("This line inside the comment");
	}

	[Fact]
	public void Parse_WithInlineMultilineComment_RemovesCorrectly()
	{
		var expression = "5 /* comment */ + 3";
		var document = ExtendedExpressionDocumentParser.Parse(expression);

		document.TidiedExpression.Should().Be("5   + 3");
	}

	[Fact]
	public void Parse_OriginalExpressionPreserved()
	{
		var originalExpression = """
// x:System.Int32:10
/*
# Documentation
*/
x + 1
""";
		var document = ExtendedExpressionDocumentParser.Parse(originalExpression);

		document.OriginalExpression.Should().Be(originalExpression);
	}
}
