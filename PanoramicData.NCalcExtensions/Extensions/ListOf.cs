using PanoramicData.NCalcExtensions.Helpers;
using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ListOf
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var typeString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string.");

		var remainingParameters = functionArgs.Parameters.Skip(1).ToArray();
		switch (typeString)
		{
			case "byte":
				functionArgs.Result = GetListOf<byte>(remainingParameters);
				return;
			case "byte?":
				functionArgs.Result = GetListOf<byte?>(remainingParameters);
				return;
			case "short":
				functionArgs.Result = GetListOf<short>(remainingParameters);
				return;
			case "short?":
				functionArgs.Result = GetListOf<short?>(remainingParameters);
				return;
			case "int":
				functionArgs.Result = GetListOf<int>(remainingParameters);
				return;
			case "int?":
				functionArgs.Result = GetListOf<int?>(remainingParameters);
				return;
			case "long":
				functionArgs.Result = GetListOf<long>(remainingParameters);
				return;
			case "long?":
				functionArgs.Result = GetListOf<long?>(remainingParameters);
				return;
			case "float":
				functionArgs.Result = GetListOf<float>(remainingParameters);
				return;
			case "float?":
				functionArgs.Result = GetListOf<float?>(remainingParameters);
				return;
			case "double":
				functionArgs.Result = GetListOf<double>(remainingParameters);
				return;
			case "double?":
				functionArgs.Result = GetListOf<double?>(remainingParameters);
				return;
			case "decimal":
				functionArgs.Result = GetListOf<decimal>(remainingParameters);
				return;
			case "decimal?":
				functionArgs.Result = GetListOf<decimal?>(remainingParameters);
				return;
			case "string":
				functionArgs.Result = GetListOf<string>(remainingParameters);
				return;
			case "string?":
				functionArgs.Result = GetListOf<string?>(remainingParameters);
				return;
			case "object":
				functionArgs.Result = GetListOf<object>(remainingParameters);
				return;
			case "object?":
				functionArgs.Result = GetListOf<object?>(remainingParameters);
				return;
			default:
				throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string of a numeric or string type.");
		}
	}

	private static List<T> GetListOf<T>(Expression[] remainingParameters)
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
				list.Add(default);
			}
			else if (Nullable.GetUnderlyingType(typeof(T)) != null && value != null)
			{
				var underlyingType = Nullable.GetUnderlyingType(typeof(T));
				if (underlyingType != null)
				{
					var convertedValue = Convert.ChangeType(value, underlyingType);
					list.Add((T)convertedValue);
				}
			}
			else if (value is T tValue)
			{
				list.Add(tValue);
			}
			else if (Convert.ChangeType(value, typeof(T)) is T convertedValue)
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
