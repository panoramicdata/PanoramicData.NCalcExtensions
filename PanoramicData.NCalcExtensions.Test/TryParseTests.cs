namespace PanoramicData.NCalcExtensions.Test;

public partial class TryParseTests : NCalcTest
{
	[Theory]
	[InlineData("")]
	[InlineData("'int', 1")]
	[InlineData("'xxx', '1'")]
	[InlineData("'Guid', '1'")]
	[InlineData("'Guid', '1', 'x', 'y'")]
	[InlineData("1")]
	public void TryParse_IncorrectParameterCountOrType_Throws(string parameters)
		=> new ExtendedExpression($"tryParse({parameters})")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	// An unsupported type name used to surface as IndexOutOfRangeException: the 'not supported'
	// FormatException was caught by a fallback handler that then read a fourth parameter tryParse()
	// does not have.
	[Theory]
	[InlineData("xxx")]
	[InlineData("System.Object")]
	[InlineData("INT")]
	public void TryParse_UnsupportedType_ThrowsFormatException(string typeName)
		=> new ExtendedExpression($"tryParse('{typeName}', '1', 'outputVariable')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>()
			.WithMessage($"type '{typeName}' not supported.");

	[Theory]
	[InlineData("short")]
	[InlineData("ushort")]
	[InlineData("long")]
	[InlineData("ulong")]
	[InlineData("int")]
	[InlineData("uint")]
	[InlineData("byte")]
	[InlineData("sbyte")]
	[InlineData("double")]
	[InlineData("float")]
	[InlineData("decimal")]
	[InlineData("jObject")]
	[InlineData("jArray")]
	[InlineData("Guid")]
	public void TryParse_DoesNotParse_ReturnsFalse(string parameters)
		=> Test($"tryParse('{parameters}', 'x', 'outputVariable')").Should().Be(false);

	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("false")]
	[InlineData("False")]
	public void TryParse_Bool_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"tryParse('bool', '{text}', 'outputVariable')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	// Test System.* type names
	[Fact]
	public void TryParse_SystemBoolean_Succeeds()
		=> Test("tryParse('System.Boolean', 'true', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemInt32_Succeeds()
		=> Test("tryParse('System.Int32', '42', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemInt64_Succeeds()
		=> Test("tryParse('System.Int64', '123456789', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemUInt32_Succeeds()
		=> Test("tryParse('System.UInt32', '42', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemUInt64_Succeeds()
		=> Test("tryParse('System.UInt64', '123456789', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemDouble_Succeeds()
		=> Test("tryParse('System.Double', '3.14', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemSingle_Succeeds()
		=> Test("tryParse('System.Single', '3.14', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemDecimal_Succeeds()
		=> Test("tryParse('System.Decimal', '3.14', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemByte_Succeeds()
		=> Test("tryParse('System.Byte', '255', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemSByte_Succeeds()
		=> Test("tryParse('System.SByte', '-128', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemInt16_Succeeds()
		=> Test("tryParse('System.Int16', '32767', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemUInt16_Succeeds()
		=> Test("tryParse('System.UInt16', '65535', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_SystemGuid_Succeeds()
		=> Test("tryParse('System.Guid', 'fa2e60e8-dd1e-4cbb-b53c-e69c63f14866', 'outputVariable')").Should().Be(true);
}
