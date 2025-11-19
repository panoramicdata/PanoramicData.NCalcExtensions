namespace PanoramicData.NCalcExtensions.Test;

public class TypeMixingTests
{
	[Fact]
	public void TextPlusInteger_Fails()
	{
		var expression = new ExtendedExpression($"'a' + 1");
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void TextMinusInteger_Fails()
	{
		var expression = new ExtendedExpression($"'a' - 1");
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void TextPlusFloat_Fails()
	{
		var expression = new ExtendedExpression($"'a' + 1.5");
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void TextMinusFloat_Fails()
	{
		var expression = new ExtendedExpression($"'a' - 1.5");
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void IntegerPlusText_Fails()
	{
		var expression = new ExtendedExpression($"1 + 'a'");
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}

	[Theory]
	[InlineData(typeof(int))]
	[InlineData(typeof(uint))]
	[InlineData(typeof(long))]
	[InlineData(typeof(ulong))]
	[InlineData(typeof(float))]
	[InlineData(typeof(double))]
	[InlineData(typeof(decimal))]
	[InlineData(typeof(bool))]
	public void StringPlusDefault_Fails(Type type)
	{
		var expression = new ExtendedExpression($"'a' + value");
		expression.Parameters["value"] = Activator.CreateInstance(type);
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}
}
