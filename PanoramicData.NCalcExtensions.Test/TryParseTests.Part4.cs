namespace PanoramicData.NCalcExtensions.Test;

public partial class TryParseTests : NCalcTest
{
	// Test invalid JSON for JObject/JArray
	[Fact]
	public void TryParse_JObject_InvalidJson_ReturnsFalse()
		=> Test("tryParse('jObject', '{invalid}', 'outputVariable')").Should().Be(false);

	[Fact]
	public void TryParse_JArray_InvalidJson_ReturnsFalse()
		=> Test("tryParse('jArray', '[invalid]', 'outputVariable')").Should().Be(false);

	// Test JObject when JSON is actually JArray
	[Fact]
	public void TryParse_JObject_WhenArray_ReturnsFalse()
		=> Test("tryParse('jObject', '[]', 'outputVariable')").Should().Be(false);

	// Test JArray when JSON is actually JObject
	[Fact]
	public void TryParse_JArray_WhenObject_ReturnsFalse()
		=> Test("tryParse('jArray', '{}', 'outputVariable')").Should().Be(false);

	// Test unsupported type
	[Fact]
	public void TryParse_UnsupportedType_ThrowsException()
	{
		var expression = new ExtendedExpression("tryParse('unsupportedType', '1', 'outputVariable')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>(); // Throws FormatException but wraps IndexOutOfRangeException
	}

	// Test edge values
	[Fact]
	public void TryParse_Int_MaxValue_Succeeds()
		=> Test("tryParse('int', '2147483647', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_Int_MinValue_Succeeds()
		=> Test("tryParse('int', '-2147483648', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_Int_Overflow_ReturnsFalse()
		=> Test("tryParse('int', '2147483648', 'outputVariable')").Should().Be(false);

	[Fact]
	public void TryParse_Byte_MaxValue_Succeeds()
		=> Test("tryParse('byte', '255', 'outputVariable')").Should().Be(true);

	[Fact]
	public void TryParse_Byte_Overflow_ReturnsFalse()
		=> Test("tryParse('byte', '256', 'outputVariable')").Should().Be(false);

	// Test retrieval of parsed values
	[Fact]
	public void TryParse_AndRetrieve_Int_Works()
		=> Test("if(tryParse('int', '42', 'myVar'), retrieve('myVar'), -1)").Should().Be(42);

	[Fact]
	public void TryParse_AndRetrieve_Bool_Works()
		=> Test("if(tryParse('bool', 'true', 'myVar'), retrieve('myVar'), false)").Should().Be(true);

	[Fact]
	public void TryParse_AndRetrieve_Guid_Works()
	{
		var guid = "fa2e60e8-dd1e-4cbb-b53c-e69c63f14866";
		var expression = new ExtendedExpression($"if(tryParse('Guid', '{guid}', 'myVar'), retrieve('myVar'), null)");
		expression.Evaluate().Should().Be(Guid.Parse(guid));
	}
}
