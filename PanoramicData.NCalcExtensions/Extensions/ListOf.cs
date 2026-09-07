using PanoramicData.NCalcExtensions.Helpers;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("listOf")]
	[Description("Emits a List<T>.")]
	List<object?> ListOf(
		[Description("The name of the data type.")]
		string typeName,
		[Description("The list parameters.")]
		IEnumerable<object?> parameters
	);
}

internal static class ListOf
{
	/// <summary>
	/// The element type names listOf() accepts, mapped to the builder for each.
	/// </summary>
	/// <remarks>
	/// A lookup table rather than a 26-case switch: selecting the generic instantiation by name is
	/// all the dispatch ever did. Matching is case-sensitive and ordinal, as the switch was.
	/// </remarks>
	private static readonly FrozenDictionary<string, Func<object?[], CultureInfo, object>> ListBuilders =
		new Dictionary<string, Func<object?[], CultureInfo, object>>(StringComparer.Ordinal)
		{
			["sbyte"] = static (values, culture) => GetListOf<sbyte>(values, culture),
			["sbyte?"] = static (values, culture) => GetListOf<sbyte?>(values, culture),
			["byte"] = static (values, culture) => GetListOf<byte>(values, culture),
			["byte?"] = static (values, culture) => GetListOf<byte?>(values, culture),
			["short"] = static (values, culture) => GetListOf<short>(values, culture),
			["short?"] = static (values, culture) => GetListOf<short?>(values, culture),
			["ushort"] = static (values, culture) => GetListOf<ushort>(values, culture),
			["ushort?"] = static (values, culture) => GetListOf<ushort?>(values, culture),
			["int"] = static (values, culture) => GetListOf<int>(values, culture),
			["int?"] = static (values, culture) => GetListOf<int?>(values, culture),
			["uint"] = static (values, culture) => GetListOf<uint>(values, culture),
			["uint?"] = static (values, culture) => GetListOf<uint?>(values, culture),
			["long"] = static (values, culture) => GetListOf<long>(values, culture),
			["long?"] = static (values, culture) => GetListOf<long?>(values, culture),
			["ulong"] = static (values, culture) => GetListOf<ulong>(values, culture),
			["ulong?"] = static (values, culture) => GetListOf<ulong?>(values, culture),
			["float"] = static (values, culture) => GetListOf<float>(values, culture),
			["float?"] = static (values, culture) => GetListOf<float?>(values, culture),
			["double"] = static (values, culture) => GetListOf<double>(values, culture),
			["double?"] = static (values, culture) => GetListOf<double?>(values, culture),
			["decimal"] = static (values, culture) => GetListOf<decimal>(values, culture),
			["decimal?"] = static (values, culture) => GetListOf<decimal?>(values, culture),
			["string"] = static (values, culture) => GetListOf<string>(values, culture),
			["string?"] = static (values, culture) => GetListOf<string?>(values, culture),
			["object"] = static (values, culture) => GetListOf<object>(values, culture),
			["object?"] = static (values, culture) => GetListOf<object?>(values, culture),
		}.ToFrozenDictionary(StringComparer.Ordinal);

	/// <summary>
	/// Types that convert from a double via their own Convert method rather than through
	/// Convert.ChangeType. Preserved from the original implementation.
	/// </summary>
	private static readonly FrozenDictionary<Type, Func<double, object>> DoubleConverters =
		new Dictionary<Type, Func<double, object>>
		{
			[typeof(ulong)] = static value => Convert.ToUInt64(value),
			[typeof(uint)] = static value => Convert.ToUInt32(value),
			[typeof(ushort)] = static value => Convert.ToUInt16(value),
			[typeof(sbyte)] = static value => Convert.ToSByte(value),
		}.ToFrozenDictionary();

	internal static void Evaluate(FunctionEventArgs functionArgs, CultureInfo cultureInfo)
	{
		var typeString = functionArgs.Parameters.Evaluate(0) as string
			?? throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string.");

		// The remaining parameters are evaluated before the type name is checked, as they were when
		// this was a switch, so a parameter's side effects still happen for an unsupported type.
		var remainingParameters = Enumerable
			.Range(1, functionArgs.Parameters.Count - 1)
			.Select(functionArgs.Parameters.Evaluate)
			.ToArray();

		if (!ListBuilders.TryGetValue(typeString, out var build))
		{
			throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string of a numeric or string type.");
		}

		functionArgs.Result = build(remainingParameters, cultureInfo);
	}

	private static List<T> GetListOf<T>(object?[] remainingParameters, CultureInfo cultureInfo)
	{
		var list = new List<T>(remainingParameters.Length);
		foreach (var value in remainingParameters)
		{
			list.Add(ConvertTo<T>(value, cultureInfo));
		}

		return list;
	}

	/// <summary>
	/// Converts a single listOf() parameter to the list's element type.
	/// </summary>
	private static T ConvertTo<T>(object? value, CultureInfo cultureInfo)
	{
		var targetType = typeof(T);

		// object and object? are the same type at runtime, and take any value unchanged.
		if (targetType == typeof(object))
		{
			return (T)value!;
		}

		var underlyingType = Nullable.GetUnderlyingType(targetType);
		if (value is null)
		{
			return underlyingType is not null || !targetType.IsValueType
				? default!
				: throw new FormatException($"Cannot convert null to non-nullable type {TypeHelper.AsHumanString<T>()}.");
		}

		if (value is T typedValue)
		{
			return typedValue;
		}

		var actualTargetType = underlyingType ?? targetType;
		if (value is double doubleValue && DoubleConverters.TryGetValue(actualTargetType, out var convert))
		{
			return (T)convert(doubleValue);
		}

		// Convert.ChangeType throws for a value it cannot convert, which is the reported failure.
		return (T)Convert.ChangeType(value, actualTargetType, cultureInfo)!;
	}
}
