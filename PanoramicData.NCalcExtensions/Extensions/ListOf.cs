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
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		var typeString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string.");

		var remainingParameters = functionArgs.Parameters.Skip(1).ToArray();
		switch (typeString)
		{
			case "sbyte":
				functionArgs.Result = GetListOf<sbyte>(remainingParameters, cultureInfo);
				return;
			case "sbyte?":
				functionArgs.Result = GetListOf<sbyte?>(remainingParameters, cultureInfo);
				return;
			case "byte":
				functionArgs.Result = GetListOf<byte>(remainingParameters, cultureInfo);
				return;
			case "byte?":
				functionArgs.Result = GetListOf<byte?>(remainingParameters, cultureInfo);
				return;
			case "short":
				functionArgs.Result = GetListOf<short>(remainingParameters, cultureInfo);
				return;
			case "short?":
				functionArgs.Result = GetListOf<short?>(remainingParameters, cultureInfo);
				return;
			case "ushort":
				functionArgs.Result = GetListOf<ushort>(remainingParameters, cultureInfo);
				return;
			case "ushort?":
				functionArgs.Result = GetListOf<ushort?>(remainingParameters, cultureInfo);
				return;
			case "int":
				functionArgs.Result = GetListOf<int>(remainingParameters, cultureInfo);
				return;
			case "int?":
				functionArgs.Result = GetListOf<int?>(remainingParameters, cultureInfo);
				return;
			case "uint":
				functionArgs.Result = GetListOf<uint>(remainingParameters, cultureInfo);
				return;
			case "uint?":
				functionArgs.Result = GetListOf<uint?>(remainingParameters, cultureInfo);
				return;
			case "long":
				functionArgs.Result = GetListOf<long>(remainingParameters, cultureInfo);
				return;
			case "long?":
				functionArgs.Result = GetListOf<long?>(remainingParameters, cultureInfo);
				return;
			case "ulong":
				functionArgs.Result = GetListOf<ulong>(remainingParameters, cultureInfo);
				return;
			case "ulong?":
				functionArgs.Result = GetListOf<ulong?>(remainingParameters, cultureInfo);
				return;
			case "float":
				functionArgs.Result = GetListOf<float>(remainingParameters, cultureInfo);
				return;
			case "float?":
				functionArgs.Result = GetListOf<float?>(remainingParameters, cultureInfo);
				return;
			case "double":
				functionArgs.Result = GetListOf<double>(remainingParameters, cultureInfo);
				return;
			case "double?":
				functionArgs.Result = GetListOf<double?>(remainingParameters, cultureInfo);
				return;
			case "decimal":
				functionArgs.Result = GetListOf<decimal>(remainingParameters, cultureInfo);
				return;
			case "decimal?":
				functionArgs.Result = GetListOf<decimal?>(remainingParameters, cultureInfo);
				return;
			case "string":
				functionArgs.Result = GetListOf<string>(remainingParameters, cultureInfo);
				return;
			case "string?":
				functionArgs.Result = GetListOf<string?>(remainingParameters, cultureInfo);
				return;
			case "object":
				functionArgs.Result = GetListOf<object>(remainingParameters, cultureInfo);
				return;
			case "object?":
				functionArgs.Result = GetListOf<object?>(remainingParameters, cultureInfo);
				return;
			default:
				throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string of a numeric or string type.");
		}
	}

	private static List<T> GetListOf<T>(Expression[] remainingParameters, CultureInfo cultureInfo)
	{
		var list = new List<T>();
		var targetType = typeof(T);
		var underlyingType = Nullable.GetUnderlyingType(targetType);
		var actualTargetType = underlyingType ?? targetType;

		foreach (var parameter in remainingParameters)
		{
			var value = parameter.Evaluate();
			
			if (targetType == typeof(object))
			{
				list.Add((T)value!);
			}
			else if (value == null)
			{
				if (underlyingType != null || !targetType.IsValueType)
				{
					list.Add(default!);
				}
				else
				{
					throw new FormatException($"Cannot convert null to non-nullable type {TypeHelper.AsHumanString<T>()}.");
				}
			}
			else if (value is T tValue)
			{
				list.Add(tValue);
			}
			else
			{
				// Special handling for large unsigned types
				if (actualTargetType == typeof(ulong) && value is double doubleValue)
				{
					list.Add((T)(object)Convert.ToUInt64(doubleValue));
				}
				else if (actualTargetType == typeof(uint) && value is double doubleValue2)
				{
					list.Add((T)(object)Convert.ToUInt32(doubleValue2));
				}
				else if (actualTargetType == typeof(ushort) && value is double doubleValue3)
				{
					list.Add((T)(object)Convert.ToUInt16(doubleValue3));
				}
				else if (actualTargetType == typeof(sbyte) && value is double doubleValue4)
				{
					list.Add((T)(object)Convert.ToSByte(doubleValue4));
				}
				else if (Convert.ChangeType(value, actualTargetType, cultureInfo) is var convertedValue)
				{
					list.Add((T)convertedValue!);
				}
				else
				{
					throw new FormatException($"Parameter must be of type {TypeHelper.AsHumanString<T>()}.");
				}
			}
		}

		return list;
	}
}
