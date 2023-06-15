namespace PanoramicData.NCalcExtensions.Test;

public class IsGuidTests
{
	[Fact]
	public void IsGuid_EmptyStringLiteral_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid('')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_EmptyStringParameter_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "";
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_NonGuidStringLiteral_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid('abc')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_NonGuidStringParameter_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "abc";
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_InvalidGuidStringLiteralWithoutBraces_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid('9384EF0Z-38AD-4E8E-A24E-0ACD3273A401')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_InvalidGuidStringLiteralWithBraces_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid('{9384EF0Z-38AD-4E8E-A24E-0ACD3273A401}')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_InvalidGuidStringParameterWithoutBraces_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "9384EF0Z-38AD-4E8E-A24E-0ACD3273A401";
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_InvalidGuidStringParameterWithBraces_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "{9384EF0Z-38AD-4E8E-A24E-0ACD3273A401}";
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_GuidStringLiteralWithoutBraces_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isGuid('9384EF0A-38AD-4E8E-A24E-0ACD3273A401')");
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_GuidStringLiteralWithBraces_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isGuid('{9384EF0A-38AD-4E8E-A24E-0ACD3273A401}')");
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_GuidStringParameterWithoutBraces_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "9384EF0A-38AD-4E8E-A24E-0ACD3273A401";
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_GuidStringParameterWithBraces_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = "{9384EF0A-38AD-4E8E-A24E-0ACD3273A401}";
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsGuid_GuidParameter_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = new Guid("{9384EF0A-38AD-4E8E-A24E-0ACD3273A401}");
		Assert.True(expression.Evaluate() as bool?);
	}
}
