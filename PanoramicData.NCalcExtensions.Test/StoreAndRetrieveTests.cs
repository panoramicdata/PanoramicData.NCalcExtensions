namespace PanoramicData.NCalcExtensions.Test;

public class StoreAndRetrieveTests
{
	[Theory]
	[InlineData(null)]
	[InlineData(1)]
	[InlineData("a")]
	[InlineData(1.0)]
	public void Convert_Succeeds(
		object? value
	)
	{
		var insertedString =
			value is string ? $"'{value}'"
			: value is null ? "null"
			: value;
		var expression = $"convert(store('x', {insertedString}), retrieve('x'))";
		new ExtendedExpression(expression)
			.Evaluate()
			.Should()
			.Be(value);
	}

	[Theory]
	[InlineData("null")]
	[InlineData(1)]
	[InlineData("a")]
	[InlineData(1.0)]
	public void StoreAndRetrieve_Succeeds(
		object? value
	)
	{
		var insertedString =
			value is string ? $"'{value}'"
			: value is null ? "null"
			: value;
		var expression = $"store('x', {insertedString}) && retrieve('x') == {insertedString}";
		var result = new ExtendedExpression(expression)
			.Evaluate();

		result.Should().Be(true);
	}

	[Theory]
	[InlineData("")]
	[InlineData("1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public void Store_IncorrectParameterCount_Throws(string parameters) =>
		new ExtendedExpression($"store({parameters})")
		.Invoking(e => e.Evaluate())
		.Should()
		.Throw<FormatException>()
		.WithMessage($"{ExtensionFunction.Store}() requires two parameters.");

	[Theory]
	[InlineData("")]
	[InlineData("1, 1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public void Retrieve_IncorrectParameterCount_Throws(string parameters) =>
		new ExtendedExpression($"retrieve({parameters})")
		.Invoking(e => e.Evaluate())
		.Should()
		.Throw<FormatException>()
		.WithMessage($"{ExtensionFunction.Retrieve}() requires one string parameter.");
}
