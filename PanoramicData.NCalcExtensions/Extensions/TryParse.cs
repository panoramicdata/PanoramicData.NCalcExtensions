using Newtonsoft.Json;
using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

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
					{
						functionArgs.Result = bool.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "sbyte":
					{
						functionArgs.Result = sbyte.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "byte":
					{
						functionArgs.Result = byte.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "short":
					{
						functionArgs.Result = short.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "ushort":
					{
						functionArgs.Result = ushort.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "int":
					{
						functionArgs.Result = int.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "uint":
					{
						functionArgs.Result = uint.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "long":
					{
						functionArgs.Result = long.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "ulong":
					{
						functionArgs.Result = ulong.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "double":
					{
						functionArgs.Result = double.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "float":
					{
						functionArgs.Result = float.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "decimal":
					{
						functionArgs.Result = decimal.TryParse(text, out var result);
						dictionary[outputVariableName] = result;
						break;
					}
				case "Guid":
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
								dictionary[outputVariableName] = result;
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
								dictionary[outputVariableName] = result;
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
			};
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
