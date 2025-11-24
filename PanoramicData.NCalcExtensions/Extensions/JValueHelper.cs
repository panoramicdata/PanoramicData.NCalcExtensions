using Newtonsoft.Json.Linq;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Helper class for unwrapping JValue objects to their underlying values
/// </summary>
internal static class JValueHelper
{
	/// <summary>
	/// Unwraps a JValue to its underlying value, or returns the input unchanged if not a JValue
	/// </summary>
	/// <param name="value">The value to potentially unwrap</param>
	/// <returns>The unwrapped value or the original value</returns>
	internal static object? UnwrapJValue(object? value) => value switch
	{
		JValue jValue => jValue.Value,
		_ => value
	};
}
