namespace PanoramicData.NCalcExtensions.Test;

public class TryTests
{
	[Fact]
	public void Try_ToFewParameters_Fails()
		=> new ExtendedExpression("try()").Invoking(y => y.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("try: At least 1 parameter required.");

	[Fact]
	public void Try_ToManyParameters_Fails()
	  => new ExtendedExpression("try(throw('Woo'), 2, 3)").Invoking(y => y.Evaluate())
		  .Should().Throw<FormatException>()
		  .WithMessage("try: No more than 2 parameters permitted.");

	[Theory]
	[InlineData("1", 1)]
	[InlineData("throw('Woo')", null)]
	[InlineData("throw('Woo'), 1", 1)]
	[InlineData("throw('Woo'), exception_message", "Woo")]
	[InlineData("throw('Woo'), exception_typeName", nameof(NCalcExtensionsException))]
	[InlineData("throw('Woo'), exception_type", typeof(NCalcExtensionsException))]
	[InlineData("throw('Woo'), exception_typeFullName", "PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException")]
	public void Try_SimpleNoThrow_Succeeds(string parameters, object? expectedValue)
	{
		var result = new ExtendedExpression($"try({parameters})")
			.Evaluate();
		result.Should().Be(expectedValue);
	}
}
