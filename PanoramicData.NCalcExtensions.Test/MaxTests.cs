namespace PanoramicData.NCalcExtensions.Test;

public class MaxTests : ExtremumTests
{
	protected override string Function => "max";

	public static TheoryData<string, object?> EquivalentCases => new()
	{
		{ "max(listOf('double?', 1, 2, 3), 'x', 'x')", 3 },
		{ "max(listOf('double?', 3, 2, 1), 'x', 'x')", 3 },
		{ "max(listOf('double?', 1, 3, 2), 'x', 'x')", 3 },
		{ "max(listOf('double?', 1, 1, 1), 'x', 'x')", 1 },
		{ "max(listOf('double?', 1, 1, 2), 'x', 'x')", 2 },
		{ "max(listOf('double?', 1, null, 2), 'x', 'x')", 2 },
		{ "max(listOf('double?', 1.1, null, 2), 'x', 'x')", 2 },
		{ "max(listOf('double?', null, null, null), 'x', 'x')", null },

		{ "max(list(1, 2, 3), 'x', 'x')", 3 },
		{ "max(list(3, 2, 1), 'x', 'x')", 3 },
		{ "max(list(1, 3, 2), 'x', 'x')", 3 },
		{ "max(list(1, 1, 1), 'x', 'x')", 1 },
		{ "max(list(1, 1, 2), 'x', 'x')", 2 },

		{ "max(list(1, 2, 3))", 3 },
		{ "max(list(3, 2, 1))", 3 },
		{ "max(list(1, 3, 2))", 3 },
		{ "max(list(1, 1, 1))", 1 },
		{ "max(list(1, 1, 2))", 2 },

		{ "max(list('1', '2', '3'))", "3" },
		{ "max(list('3', '2', '1'))", "3" },
		{ "max(list('1', '3', null))", "3" },
		{ "max(list('abc', 'raf', 'bbc'))", "raf" },
		{ "max(list('abc', 'ABC', null))", "ABC" },

		{ "max(null)", null },
		{ "max(list())", null },
		{ "max(list(), 'x', 'x')", null },
		{ "max(listOf('int?', null, null, null))", null },
	};

	public static TheoryData<string, object?> ExactCases => new()
	{
		{ "max(listOf('int', 1, 2, 3), 'x', 'x + 1')", 4 },
		{ "max(listOf('string', '1', '2', '3'), 'x', 'x + x')", "6" },

		// One list of each numeric type.
		{ "max(listOf('byte', 1, 255, 100))", (byte)255 },
		{ "max(listOf('sbyte', -128, 127, 0))", (sbyte)127 },
		{ "max(listOf('short', -100, 32767, 100))", (short)32767 },
		{ "max(listOf('ushort', 1, 65535, 100))", (ushort)65535 },
		{ "max(listOf('uint', 1, 4294967295, 100))", 4294967295u },
		{ "max(listOf('long', -1000, 9223372036854775807, 1000))", 9223372036854775807L },
		// Values that can be safely represented in the double NCalc uses for numeric literals.
		{ "max(listOf('ulong', 1, 9999999999999, 100))", 9999999999999UL },
		{ "max(listOf('decimal', 1.1, 2.2, 3.3))", 3.3m },

		{ "max(listOf('int?', 1, null, 3, null, 2))", 3 },
		{ "max(listOf('int?', 1, 2, 3), 'x', 'if(x == 2, null, x)')", 3 },
		{ "max(listOf('long', 9223372036854775806, 9223372036854775807))", 9223372036854775807L },
		{ "max(listOf('int', -100, -50, -200))", -50 },
		{ "max(listOf('int', 42))", 42 },
		{ "max(listOf('int', 1, 2, 3), 'x', 'x * x')", 9 },

		// The lambda form, over one list of each type.
		{ "max(listOf('sbyte', 10, -5, 3), 'x', 'x')", 10 },
		{ "max(listOf('sbyte?', 10, null, -5, 3), 'x', 'x')", 10 },
		{ "max(listOf('byte', 100, 50, 200), 'x', 'x')", 200 },
		{ "max(listOf('byte?', 100, null, 50, 200), 'x', 'x')", 200 },
		{ "max(listOf('short', 1000, 500, 2000), 'x', 'x')", 2000 },
		{ "max(listOf('short?', 1000, null, 500, 2000), 'x', 'x')", 2000 },
		{ "max(listOf('ushort', 1000, 500, 2000), 'x', 'x')", 2000 },
		{ "max(listOf('ushort?', 1000, null, 500, 2000), 'x', 'x')", 2000 },
		{ "max(listOf('uint', 100, 50, 200), 'x', 'x')", 200u },
		{ "max(listOf('uint?', 100, null, 50, 200), 'x', 'x')", 200u },
		{ "max(listOf('long', 1000, 500, 2000), 'x', 'x')", 2000L },
		{ "max(listOf('long?', 1000, null, 500, 2000), 'x', 'x')", 2000L },
		{ "max(listOf('ulong', 1000, 500, 2000), 'x', 'x')", 2000UL },
		{ "max(listOf('ulong?', 1000, null, 500, 2000), 'x', 'x')", 2000UL },
		{ "max(listOf('double', 3.3, 1.1, 2.2), 'x', 'x')", 3.3 },
		{ "max(listOf('double?', 3.3, null, 1.1, 2.2), 'x', 'x')", 3.3 },
		{ "max(listOf('decimal', 3.3, 1.1, 2.2), 'x', 'x')", 3.3m },
		{ "max(listOf('decimal?', 3.3, null, 1.1, 2.2), 'x', 'x')", 3.3m },
		{ "max(listOf('string', 'abc', 'xyz', 'def'), 'x', 'x')", "xyz" },
		{ "max(listOf('string?', 'abc', null, 'xyz', 'def'), 'x', 'x')", "xyz" },
	};

	public static TheoryData<string, float> FloatCases => new()
	{
		{ "max(listOf('float', 1.1, 2.2, 3.3))", 3.3f },
		{ "max(listOf('float', 3.3, 1.1, 2.2), 'x', 'x')", 3.3f },
		{ "max(listOf('float?', 3.3, null, 1.1, 2.2), 'x', 'x')", 3.3f },
	};

	public static TheoryData<string, string> StringListParameterCases => new()
	{
		{ "1,2,3", "3" },
		{ "3,2,1", "3" },
		{ "1,3,null", "3" },
		{ "abc,raf,bbc", "raf" },
		{ "abc,ABC,null", "ABC" },
	};

	public static TheoryData<string, string> FailureCases => new()
	{
		{ "max('not a list')", "*must be an IEnumerable*" },
		{ "max(list(1, 2, 3), 123, 'x')", "*Second*parameter must be a string*" },
		{ "max(list(1, 2, 3), 'x', 456)", "*Third*parameter must be a string*" },
	};
}
