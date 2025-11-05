using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("tryParse")]
	[Description("Returns a boolean result of an attempted cast.")]
	object TryParse(
		[Description("The name of the data type.")]
		string type,
		[Description("The text value to be parsed.")]
		string text,
		[Description("The name of the output variable where the result will be stored.")]
		string key
	);
}

internal static class TryParse
{
	internal static void Evaluate(FunctionArgs functionArgs, Dictionary<string, object?> dictionary)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - requires exactly three string parameters.");
		}

		var parameterIndex = 0;
		var typeString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - first parameter should be a string.");
		var text = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - second parameter should be a string.");
		var outputVariableName = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - third parameter should be a string.");
		try
		{
			// Assume false until proven otherwise
			functionArgs.Result = false;
			switch (typeString)
			{
				case "bool":
				case "System.Boolean":
					{
						functionArgs.Result = bool.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "sbyte":
				case "System.SByte":
					{
						functionArgs.Result = sbyte.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "byte":
				case "System.Byte":
					{
						functionArgs.Result = byte.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "short":
				case "System.Int16":
					{
						functionArgs.Result = short.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "ushort":
				case "System.UInt16":
					{
						functionArgs.Result = ushort.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "int":
				case "System.Int32":
					{
						functionArgs.Result = int.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "uint":
				case "System.UInt32":
					{
						functionArgs.Result = uint.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "long":
				case "System.Int64":
					{
						functionArgs.Result = long.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "ulong":
				case "System.UInt64":
					{
						functionArgs.Result = ulong.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "double":
				case "System.Double":
					{
						functionArgs.Result = double.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "float":
				case "System.Single":
					{
						functionArgs.Result = float.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "decimal":
				case "System.Decimal":
					{
						functionArgs.Result = decimal.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "Guid":
				case "System.Guid":
					{
						functionArgs.Result = Guid.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "JObject" or "jObject":
					{
						try
						{
							var result = JsonConvert.DeserializeObject(text);
							if (result is JObject jObject)
							{
								functionArgs.Result = true;
								dictionary[outputVariableName] = jObject;
							}
						}
						catch (JsonReaderException)
						{
							// Leave functionArgs.Result as false
						}

						break;
					}
				case "JArray" or "jArray":
					{
						try
						{
							var result = JsonConvert.DeserializeObject(text);
							if (result is JArray jArray)
							{
								functionArgs.Result = true;
								dictionary[outputVariableName] = jArray;
							}
						}
						catch (JsonReaderException)
						{
							// Leave functionArgs.Result as false
						}

						break;
					}
				default:
					throw new FormatException($"type '{typeString}' not supported.");
			}
		}
		catch (FormatException e)
		{
			if (functionArgs.Parameters.Length >= 3)
			{
				functionArgs.Result = functionArgs.Parameters[parameterIndex].Evaluate();
				return;
			}

			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'.", e);
		}
	}
}
