namespace PanoramicData.NCalcExtensions.Test;

public class MinTests : ExtremumTests
{
	protected override string Function => "min";

	public static TheoryData<string, object?> EquivalentCases => new()
	{
		{ "min(listOf('double?', 1, 2, 3), 'x', 'x')", 1 },
		{ "min(listOf('double?', 3, 2, 1), 'x', 'x')", 1 },
		{ "min(listOf('double?', 1, 3, 2), 'x', 'x')", 1 },
		{ "min(listOf('double?', 1, 1, 1), 'x', 'x')", 1 },
		{ "min(listOf('double?', 1, 1, 2), 'x', 'x')", 1 },
		{ "min(listOf('double?', 1, null, 2), 'x', 'x')", 1 },
		{ "min(listOf('double?', 1.1, null, 2), 'x', 'x')", 1.1d },
		{ "min(listOf('double?', null, null, null), 'x', 'x')", null },

		{ "min(list(1, 2, 3), 'x', 'x')", 1 },
		{ "min(list(3, 2, 1), 'x', 'x')", 1 },
		{ "min(list(1, 3, 2), 'x', 'x')", 1 },
		{ "min(list(1, 1, 1), 'x', 'x')", 1 },
		{ "min(list(1, 1, 2), 'x', 'x')", 1 },

		{ "min(list(1, 2, 3))", 1 },
		{ "min(list(3, 2, 1))", 1 },
		{ "min(list(1, 3, 2))", 1 },
		{ "min(list(1, 1, 1))", 1 },
		{ "min(list(1, 1, 2))", 1 },

		{ "min(list('1', '2', '3'))", "1" },
		{ "min(list('3', '2', '1'))", "1" },
		{ "min(list('1', '3', null))", "1" },
		{ "min(list('abc', 'raf', 'bbc'))", "abc" },
		{ "min(list('abc', 'ABC', null))", "abc" },

		{ "min(null)", null },
		{ "min(list())", null },
		{ "min(list(), 'x', 'x')", null },
		{ "min(listOf('int?', null, null, null))", null },
	};

	public static TheoryData<string, object?> ExactCases => new()
	{
		{ "min(listOf('int', 1, 2, 3), 'x', 'x + 1')", 2 },
		{ "min(listOf('string', '1', '2', '3'), 'x', 'x + x')", "2" },

		// One list of each numeric type.
		{ "min(listOf('byte', 255, 1, 100))", (byte)1 },
		{ "min(listOf('sbyte', 127, -128, 0))", (sbyte)-128 },
		{ "min(listOf('short', 32767, -100, 100))", (short)-100 },
		{ "min(listOf('ushort', 65535, 1, 100))", (ushort)1 },
		{ "min(listOf('uint', 4294967295, 1, 100))", 1u },
		{ "min(listOf('long', 9223372036854775807, -1000, 1000))", -1000L },
		// Values that can be safely represented in the double NCalc uses for numeric literals.
		{ "min(listOf('ulong', 100, 1, 50))", 1UL },
		{ "min(listOf('decimal', 3.3, 1.1, 2.2))", 1.1m },

		{ "min(listOf('int?', 3, null, 1, null, 2))", 1 },
		{ "min(listOf('int?', 1, 2, 3), 'x', 'if(x == 2, null, x)')", 1 },
		{ "min(listOf('long', 9223372036854775806, 9223372036854775807))", 9223372036854775806L },
		{ "min(listOf('int', -100, -50, -200))", -200 },
		{ "min(listOf('int', 42))", 42 },
		{ "min(listOf('int', 1, 2, 3), 'x', 'x * x')", 1 },

		// The lambda form, over one list of each type.
		{ "min(listOf('sbyte', 10, -5, 3), 'x', 'x')", (sbyte)-5 },
		{ "min(listOf('sbyte?', 10, null, -5, 3), 'x', 'x')", (sbyte)-5 },
		{ "min(listOf('byte', 100, 50, 200), 'x', 'x')", (byte)50 },
		{ "min(listOf('byte?', 100, null, 50, 200), 'x', 'x')", (byte)50 },
		{ "min(listOf('short', 1000, 500, 2000), 'x', 'x')", (short)500 },
		{ "min(listOf('short?', 1000, null, 500, 2000), 'x', 'x')", (short)500 },
		{ "min(listOf('ushort', 1000, 500, 2000), 'x', 'x')", (ushort)500 },
		{ "min(listOf('ushort?', 1000, null, 500, 2000), 'x', 'x')", (ushort)500 },
		{ "min(listOf('uint', 100, 50, 200), 'x', 'x')", 50u },
		{ "min(listOf('uint?', 100, null, 50, 200), 'x', 'x')", 50u },
		{ "min(listOf('long', 1000, 500, 2000), 'x', 'x')", 500L },
		{ "min(listOf('long?', 1000, null, 500, 2000), 'x', 'x')", 500L },
		{ "min(listOf('ulong', 1000, 500, 2000), 'x', 'x')", 500UL },
		{ "min(listOf('ulong?', 1000, null, 500, 2000), 'x', 'x')", 500UL },
		{ "min(listOf('double', 3.3, 1.1, 2.2), 'x', 'x')", 1.1 },
		{ "min(listOf('double?', 3.3, null, 1.1, 2.2), 'x', 'x')", 1.1 },
		{ "min(listOf('decimal', 3.3, 1.1, 2.2), 'x', 'x')", 1.1m },
		{ "min(listOf('decimal?', 3.3, null, 1.1, 2.2), 'x', 'x')", 1.1m },
		{ "min(listOf('string', 'abc', 'xyz', 'def'), 'x', 'x')", "abc" },
		{ "min(listOf('string?', 'abc', null, 'xyz', 'def'), 'x', 'x')", "abc" },
	};

	public static TheoryData<string, float> FloatCases => new()
	{
		{ "min(listOf('float', 3.3, 1.1, 2.2))", 1.1f },
		{ "min(listOf('float', 3.3, 1.1, 2.2), 'x', 'x')", 1.1f },
		{ "min(listOf('float?', 3.3, null, 1.1, 2.2), 'x', 'x')", 1.1f },
	};

	public static TheoryData<string, string> StringListParameterCases => new()
	{
		{ "1,2,3", "1" },
		{ "3,2,1", "1" },
		{ "1,3,null", "1" },
		{ "abc,raf,bbc", "abc" },
		{ "abc,ABC,null", "abc" },
	};

	public static TheoryData<string, string> FailureCases => new()
	{
		{ "min('not a list')", "*must be an IEnumerable*" },
		{ "min(list(1, 2, 3), 123, 'x')", "*Second*parameter must be a string*" },
		{ "min(list(1, 2, 3), 'x', 456)", "*Third*parameter must be a string*" },
	};
}
