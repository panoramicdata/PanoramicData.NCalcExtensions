namespace PanoramicData.NCalcExtensions.Test;

public class TypeMixingTests
{
	[Fact]
	public void TextPlusInteger_Succeeds()
	{
		var expression = new ExtendedExpression($"'a' + 1");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a1");
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
	public void TextPlusFloat_Succeeds()
	{
		var expression = new ExtendedExpression($"'a' + 1.5");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a1.5");
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
	public void IntegerPlusText_Succeeds()
	{
		var expression = new ExtendedExpression($"1 + 'a'");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("1a");
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
	public void StringPlusDefault_Succeeds(Type type)
	{
		var expression = new ExtendedExpression($"'a' + value");
		var value = Activator.CreateInstance(type);
		expression.Parameters["value"] = value;
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a" + value?.ToString() ?? "");
	}
}
