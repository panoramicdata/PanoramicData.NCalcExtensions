using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public partial class OrderByTests : NCalcTest
{
	// Every numeric type ObjectKeyComparer knows how to widen.

	[Fact]
	public void OrderBy_ByteValues_SortsCorrectly()
		=> ShouldSortTo([(byte)255, (byte)1, (byte)128], (byte)1, (byte)128, (byte)255);

	[Fact]
	public void OrderBy_SByteValues_SortsCorrectly()
		=> ShouldSortTo([(sbyte)127, (sbyte)-128, (sbyte)0], (sbyte)-128, (sbyte)0, (sbyte)127);

	[Fact]
	public void OrderBy_ShortValues_SortsCorrectly()
		=> ShouldSortTo([(short)32767, (short)-100, (short)0], (short)-100, (short)0, (short)32767);

	[Fact]
	public void OrderBy_UShortValues_SortsCorrectly()
		=> ShouldSortTo([(ushort)65535, (ushort)1, (ushort)100], (ushort)1, (ushort)100, (ushort)65535);

	[Fact]
	public void OrderBy_UIntValues_SortsCorrectly()
		=> ShouldSortTo([4294967295u, 1u, 100u], 1u, 100u, 4294967295u);

	[Fact]
	public void OrderBy_LongValues_SortsCorrectly()
		=> ShouldSortTo([9223372036854775807L, -1000L, 0L], -1000L, 0L, 9223372036854775807L);

	[Fact]
	public void OrderBy_ULongValues_SortsCorrectly()
		=> ShouldSortTo([18446744073709551615UL, 1UL, 100UL], 1UL, 100UL, 18446744073709551615UL);

	[Fact]
	public void OrderBy_DecimalValues_SortsCorrectly()
		=> ShouldSortTo([3.3m, 1.1m, 2.2m], 1.1m, 2.2m, 3.3m);

	[Fact]
	public void OrderBy_DateTimes_SortsCorrectly()
	{
		var dt1 = new DateTime(2020, 1, 1);
		var dt2 = new DateTime(2021, 1, 1);
		var dt3 = new DateTime(2019, 1, 1);

		ShouldSortTo([dt2, dt1, dt3], dt3, dt1, dt2);
	}

	[Fact]
	public void OrderBy_FloatValues_SortsCorrectly()
	{
		var result = EvaluateToList("orderBy(myList, 'n', 'n')", new List<object?> { 3.3f, 1.1f, 2.2f }, 3);
		((float)result[0]!).Should().BeApproximately(1.1f, 0.01f);
		((float)result[1]!).Should().BeApproximately(2.2f, 0.01f);
		((float)result[2]!).Should().BeApproximately(3.3f, 0.01f);
	}

	// Error cases

	[Theory]
	[InlineData("orderBy(null, 'n', 'n')", "*must be an IEnumerable*")]
	[InlineData("orderBy(list(1, 2, 3), null, 'n')", "*parameter must be a string*")]
	[InlineData("orderBy(list(1, 2, 3), 'n', null)", "*parameter must be a string*")]
	public void OrderBy_WithInvalidArguments_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrow<FormatException>(expressionText, messagePattern);

	/// <summary>
	/// Asserts that ordering <paramref name="values"/> by their own value yields exactly
	/// <paramref name="expectedOrder"/>.
	/// </summary>
	private static void ShouldSortTo(List<object?> values, params object[] expectedOrder)
		=> Test("orderBy(myList, 'n', 'n')", "myList", values)
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> to a list of <paramref name="expectedCount"/>
	/// entries.
	/// </summary>
	private static List<object?> EvaluateToList(string expressionText, int expectedCount)
		=> AsList(Test(expressionText), expectedCount);

	/// <summary>
	/// As above, with the list under test supplied as the "myList" parameter.
	/// </summary>
	private static List<object?> EvaluateToList(string expressionText, object parameterValue, int expectedCount)
		=> AsList(Test(expressionText, "myList", parameterValue), expectedCount);

	private static List<object?> AsList(object? result, int expectedCount)
	{
		var list = result as List<object?>;
		list.Should().NotBeNull();
		list.Should().HaveCount(expectedCount);
		return list!;
	}
}
