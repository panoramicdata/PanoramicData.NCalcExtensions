namespace PanoramicData.NCalcExtensions.Test;

public class MultiLineTests : NCalcTest
{
	[Theory]
	[InlineData("""true""")]
	[InlineData("""
// This is a comment
true
""")]
	[InlineData("""
// This has a blank line

true
""")]
	[InlineData("""
// Split mid-calculation
1 ==
1
""")]
	[InlineData("""
// Indented
if(
	1 == 1,
	true,
	false
)
""")]
	public void All_LessThanFive_Succeeds(string multiLineExpression)
		=> new ExtendedExpression(multiLineExpression)
		.Evaluate()
		.Should()
		.Be(true);

	[Fact]
	public void ParameterDefinition_Int32_Succeeds()
	{
		var expression = new ExtendedExpression("""
// theAnswer:System.Int32:42
theAnswer + 1
""");
		var result = expression.Evaluate();
		result.Should().Be(43);
	}

	[Fact]
	public void ParameterDefinition_MultipleParameters_Succeeds()
	{
		var expression = new ExtendedExpression("""
// theAnswer:System.Int32:42
// theAnswer2:System.Int32:2
// theAnswer3:System.String:Blah
// theAnswer4:System.Int32:23
theAnswer + theAnswer2 + theAnswer4
""");
		var result = expression.Evaluate();
		result.Should().Be(67);
	}

	[Fact]
	public void ParameterDefinition_String_Succeeds()
	{
		var expression = new ExtendedExpression("""
// name:System.String:John
name + ' Doe'
""");
		var result = expression.Evaluate();
		result.Should().Be("John Doe");
	}

	[Fact]
	public void ParameterDefinition_Double_Succeeds()
	{
		var expression = new ExtendedExpression("""
// price:System.Double:19.99
price * 2
""");
		var result = expression.Evaluate();
		result.Should().Be(39.98);
	}

	[Fact]
	public void ParameterDefinition_Decimal_Succeeds()
	{
		var expression = new ExtendedExpression("""
// amount:System.Decimal:100.50
amount * 2
""");
		var result = expression.Evaluate();
		result.Should().Be(201.00m);
	}

	[Fact]
	public void ParameterDefinition_Bool_Succeeds()
	{
		var expression = new ExtendedExpression("""
// isActive:System.Boolean:true
isActive
""");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void MultiLineComment_WithParameterDefinition_Succeeds()
	{
		var expression = new ExtendedExpression("""
// theAnswer:System.Int32:42
// theAnswer2:System.Int32:2
// theAnswer3:System.String:Blah
// theAnswer4:System.Int32:23

/*
# The answer plus one plus one
This add one twice to theAnswer, which is a 32-bit integer in this example.
*/

theAnswer + 1 + 1
""");
		var result = expression.Evaluate();
		result.Should().Be(44);
	}

	[Fact]
	public void MultiLineComment_Removed_Succeeds()
	{
		var expression = new ExtendedExpression("""
/*
This is a multi-line comment
that spans multiple lines
*/
5 + 3
""");
		var result = expression.Evaluate();
		result.Should().Be(8);
	}

	[Fact]
	public void MultiLineComment_InlineWithExpression_Removed()
	{
		var expression = new ExtendedExpression("""
5 /* this is a comment */ + 3
""");
		var result = expression.Evaluate();
		result.Should().Be(8);
	}

	[Fact]
	public void ParameterDefinition_InvalidFormat_NotParsed()
	{
		// This should not be parsed as a parameter because it doesn't have the correct format
		var expression = new ExtendedExpression("""
// This is just a comment
5 + 3
""");
		var result = expression.Evaluate();
		result.Should().Be(8);
	}

	[Fact]
	public void ParameterDefinition_WithComments_Succeeds()
	{
		var expression = new ExtendedExpression("""
// x:System.Int32:10
// This is just a regular comment
// y:System.Int32:5
x + y
""");
		var result = expression.Evaluate();
		result.Should().Be(15);
	}

	[Fact]
	public void Document_Accessors_AreExposed()
	{
		var expression = new ExtendedExpression("""
// x:System.Int32:10
// this is a regular comment
/*
# Sample documentation
*/
x + 5
""");

		expression.Document.OriginalExpression.Should().Contain("x:System.Int32:10");
		expression.Documentation.Should().Contain("Sample documentation");
		expression.ParameterDefinitions["x"].Should().Be(("System.Int32", "10"));
		expression.Comments.Should().Contain("this is a regular comment");
	}

	[Fact]
	public void SimpleExtendedExpression_Accessor_CanEvaluate()
	{
		var expression = new ExtendedExpression("""
// x:System.Int32:10
x + 5
""");

		var result = expression.SimpleExtendedExpression.Evaluate();

		result.Should().Be(15);
	}
}
