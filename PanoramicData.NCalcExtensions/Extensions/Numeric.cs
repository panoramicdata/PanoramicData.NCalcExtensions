namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Coercion shared by the functions that reduce a list of boxed values to a number.
/// </summary>
internal static class Numeric
{
	/// <summary>
	/// The value of a boxed list item as a double. Nulls count as zero.
	/// </summary>
	internal static double AsDouble(object? item, string function) => item switch
	{
		byte value => value,
		short value => value,
		int value => value,
		long value => value,
		float value => value,
		double value => value,
		decimal value => (double)value,
		JValue jValue => jValue.Type switch
		{
			JTokenType.Float => jValue.Value<float>(),
			JTokenType.Integer => jValue.Value<int>(),
			_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing {function}.")
		},
		null => 0,
		_ => throw new FormatException($"Found unsupported type '{item.GetType().Name}' when completing {function}.")
	};
}
