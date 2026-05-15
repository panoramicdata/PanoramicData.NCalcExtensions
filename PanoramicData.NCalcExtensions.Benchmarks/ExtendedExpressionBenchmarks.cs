using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using NCalc;
using PanoramicData.NCalcExtensions;
using System.Globalization;
using System.Text.Json;

[MemoryDiagnoser]
[ShortRunJob]
[Config(typeof(BenchmarkConfig))]
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
	private const string WhereExpressionText = """
length(
  where(
    list(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20),
    'n',
    'n % 2 == 0'
  )
)
""";
	private const string RegexExpressionText = "regexIsMatch('abc:def:2019-01-01', '^.+?:.+?:(.+)$')";

	private ExtendedExpression? _extendedExpression;
	private SimpleExtendedExpression? _simpleExtendedExpression;
	private ExtendedExpression? _whereExpression;
	private ExtendedExpression? _regexExpression;
	private ExtendedExpression? _getPropertyExpression;
	private JsonDocument? _getPropertyDocument;
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

	[GlobalSetup(Targets = [nameof(Evaluate_Where_Reused), nameof(Evaluate_RegexIsMatch_Reused), nameof(Evaluate_GetProperty_JsonDocument_Reused)])]
	public void SetupExtensionBenchmarks()
	{
		_whereExpression = new ExtendedExpression(WhereExpressionText, ExpressionOptions.None, CultureInfo.InvariantCulture);
		_regexExpression = new ExtendedExpression(RegexExpressionText, ExpressionOptions.None, CultureInfo.InvariantCulture);
		_getPropertyExpression = new ExtendedExpression("getProperty(doc, 'age')", ExpressionOptions.None, CultureInfo.InvariantCulture);
		_getPropertyDocument = JsonDocument.Parse("{\"name\":\"John\",\"age\":30,\"active\":true}");
		_getPropertyExpression.Parameters["doc"] = _getPropertyDocument;
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

	[Benchmark]
	public object? Evaluate_Where_Reused()
		=> _whereExpression!.Evaluate();

	[Benchmark]
	public object? Evaluate_RegexIsMatch_Reused()
		=> _regexExpression!.Evaluate();

	[Benchmark]
	public object? Evaluate_GetProperty_JsonDocument_Reused()
		=> _getPropertyExpression!.Evaluate();
}

internal sealed class BenchmarkConfig : ManualConfig
{
	public BenchmarkConfig()
	{
		Options |= ConfigOptions.DisableOptimizationsValidator;
	}
}
