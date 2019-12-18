using NCalc;
using System;
using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class Format
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 2)
			{
				throw new ArgumentException($"{ExtensionFunction.Format} function - expected two arguments");
			}

			if (!(functionArgs.Parameters[1].Evaluate() is string formatFormat))
			{
				throw new ArgumentException($"{ExtensionFunction.Format} function - expected second argument to be a format string");
			}

			var inputObject = functionArgs.Parameters[0].Evaluate();
			switch (inputObject)
			{
				case int inputInt:
					functionArgs.Result = inputInt.ToString(formatFormat, CultureInfo.InvariantCulture);
					return;
				case double inputDouble:
					functionArgs.Result = inputDouble.ToString(formatFormat, CultureInfo.InvariantCulture);
					return;
				case System.DateTime dateTime:
					functionArgs.Result = dateTime.ToString(formatFormat, CultureInfo.InvariantCulture);
					return;
				case string inputString:
					// Assume this is a number
					if (long.TryParse(inputString, out var longValue))
					{
						functionArgs.Result = longValue.ToString(formatFormat, CultureInfo.InvariantCulture);
						return;
					}
					if (double.TryParse(inputString, out var doubleValue))
					{
						functionArgs.Result = doubleValue.ToString(formatFormat, CultureInfo.InvariantCulture);
						return;
					}
					if (DateTimeOffset.TryParse(
						inputString,
						CultureInfo.InvariantCulture.DateTimeFormat,
						DateTimeStyles.AssumeUniversal,
						out var dateTimeOffsetValue))
					{
						functionArgs.Result = dateTimeOffsetValue.ToString(formatFormat, CultureInfo.InvariantCulture);
						return;
					}
					throw new FormatException($"Could not parse '{inputString}' as a number or date.");
				default:
					throw new NotSupportedException($"Unsupported input type {inputObject.GetType().Name}");
			}
		}
	}
}