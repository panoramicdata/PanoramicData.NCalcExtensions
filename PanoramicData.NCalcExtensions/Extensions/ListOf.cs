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
	internal static void Evaluate(IFunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		var typeString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string.");

		var remainingParameters = functionArgs.Parameters.Skip(1).ToArray();
		switch (typeString)
		{
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
			case "int":
				functionArgs.Result = GetListOf<int>(remainingParameters, cultureInfo);
				return;
			case "int?":
				functionArgs.Result = GetListOf<int?>(remainingParameters, cultureInfo);
				return;
			case "long":
				functionArgs.Result = GetListOf<long>(remainingParameters, cultureInfo);
				return;
			case "long?":
				functionArgs.Result = GetListOf<long?>(remainingParameters, cultureInfo);
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

	private static List<T> GetListOf<T>(IExpression[] remainingParameters, CultureInfo cultureInfo)
	{
		var list = new List<T>();
		foreach (var parameter in remainingParameters)
		{
			var value = parameter.Evaluate();
			if (typeof(T) == typeof(object))
			{
				list.Add((T)value);
			}
			else if (Nullable.GetUnderlyingType(typeof(T)) != null && value == null)
			{
				list.Add(default!);
			}
			else if (Nullable.GetUnderlyingType(typeof(T)) != null && value != null)
			{
				var underlyingType = Nullable.GetUnderlyingType(typeof(T));
				if (underlyingType != null)
				{
					var convertedValue = Convert.ChangeType(value, underlyingType, cultureInfo);
					list.Add((T)convertedValue);
				}
			}
			else if (value is T tValue)
			{
				list.Add(tValue);
			}
			else if (Convert.ChangeType(value, typeof(T), cultureInfo) is T convertedValue)
			{
				list.Add(convertedValue);
			}
			else
			{
				throw new FormatException($"Parameter must be of type {TypeHelper.AsHumanString<T>()}.");
			}
		}

		return list;
	}
}
