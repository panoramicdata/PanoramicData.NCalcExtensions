using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NCalc;
using PanoramicData.NCalcExtensions;
using System.Globalization;

[MemoryDiagnoser]
[ShortRunJob]
public class ExtendedExpressionBenchmarks
{
	private const string ExpressionText = """
// x:System.Int32:10
// y:System.Int32:5
// this is a regular comment
/*
# Add two values
Used in benchmark tests.
*/
x + y
""";

	private ExtendedExpression? _extendedExpression;
	private SimpleExtendedExpression? _simpleExtendedExpression;
	private ExtendedExpressionDocument? _document;

	[GlobalSetup(Target = nameof(Evaluate_ExtendedExpression_Reused))]
	public void SetupExtendedExpression()
	{
		_extendedExpression = new ExtendedExpression(ExpressionText, ExpressionOptions.None, CultureInfo.InvariantCulture);
	}

	[GlobalSetup(Target = nameof(Evaluate_SimpleExtendedExpression_Reused))]
	public void SetupSimpleExtendedExpression()
	{
		_document = ExtendedExpressionDocumentParser.Parse(ExpressionText);
		_simpleExtendedExpression = new SimpleExtendedExpression(
			_document.TidiedExpression,
			_document,
			ExpressionOptions.None,
			CultureInfo.InvariantCulture);
	}

	[Benchmark]
	public ExtendedExpression Construct_ExtendedExpression()
		=> new(ExpressionText, ExpressionOptions.None, CultureInfo.InvariantCulture);

	[Benchmark]
	public object? Construct_And_Evaluate_ExtendedExpression()
		=> new ExtendedExpression(ExpressionText, ExpressionOptions.None, CultureInfo.InvariantCulture).Evaluate();

	[Benchmark]
	public object? Parse_Document_And_Evaluate_SimpleExtendedExpression()
	{
		var document = ExtendedExpressionDocumentParser.Parse(ExpressionText);
		var expression = new SimpleExtendedExpression(
			document.TidiedExpression,
			document,
			ExpressionOptions.None,
			CultureInfo.InvariantCulture);
		return expression.Evaluate();
	}

	[Benchmark]
	public object? Evaluate_ExtendedExpression_Reused()
		=> _extendedExpression!.Evaluate();

	[Benchmark]
	public object? Evaluate_SimpleExtendedExpression_Reused()
		=> _simpleExtendedExpression!.Evaluate();
}
