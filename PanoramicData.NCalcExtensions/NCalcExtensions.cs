using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PanoramicData.NCalcExtensions
{
	public static class NCalcExtensions
	{
		[SuppressMessage("Design", "RCS1224:Make method an extension method.", Justification = "Nonsense")]
		public static void Extend(string functionName, FunctionArgs functionArgs)
		{
			string param1;
			string param2;
			switch (functionName)
			{
				case ExtensionFunction.Cast:
					{
						const int castParameterCount = 2;
						if (functionArgs.Parameters.Length != castParameterCount)
						{
							throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected {castParameterCount} arguments");
						}
						var inputObject = functionArgs.Parameters[0].Evaluate();
						if (!(functionArgs.Parameters[1].Evaluate() is string castTypeString))
						{
							throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a string.");
						}
						var castType = Type.GetType(castTypeString);
						if (castType == null)
						{
							throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a valid .NET type e.g. System.Decimal.");
						}
						var result = Convert.ChangeType(inputObject, castType);
						functionArgs.Result = result;
						return;
					}
				case ExtensionFunction.DateTimeAsEpochMs:
					var dateTimeOffset = DateTimeOffset.ParseExact(
						functionArgs.Parameters[0].Evaluate() as string, // Input date as string
						functionArgs.Parameters[1].Evaluate() as string,
						CultureInfo.InvariantCulture.DateTimeFormat,
						DateTimeStyles.AssumeUniversal);
					functionArgs.Result = dateTimeOffset.ToUnixTimeMilliseconds();
					break;
				case ExtensionFunction.DateTime:
					// Time Zone
					string timeZone;
					if (functionArgs.Parameters.Length > 0)
					{
						timeZone = functionArgs.Parameters[0].Evaluate() as string;
						if (timeZone == null)
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - The first argument should be a string, e.g. 'UTC'");
						}
						// TODO - support more than just UTC
						if (timeZone != "UTC")
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - Only UTC timeZone is currently supported.");
						}
					}
					else
					{
						timeZone = "UTC";
					}
					// Time zone has been determined

					// Format
					string format;
					if (functionArgs.Parameters.Length > 1)
					{
						format = functionArgs.Parameters[1].Evaluate() as string;
					}
					else
					{
						format = "YYYY-MM-dd HH:mm:ss";
					}
					// Format has been determined

					// Days to add
					double daysToAdd = 0;
					if (functionArgs.Parameters.Length > 2)
					{
						var daysToAddNullable = GetNullableDouble(functionArgs.Parameters[2]);
						if (!daysToAddNullable.HasValue)
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - Days to add must be a number.");
						}
						daysToAdd = daysToAddNullable.Value;
					}

					// Hours to add
					double hoursToAdd = 0;
					if (functionArgs.Parameters.Length > 3)
					{
						var hoursToAddNullable = GetNullableDouble(functionArgs.Parameters[3]);
						if (!hoursToAddNullable.HasValue)
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - Hours to add must be a number.");
						}
						hoursToAdd = hoursToAddNullable.Value;
					}

					// Minutes to add
					double minutesToAdd = 0;
					if (functionArgs.Parameters.Length > 4)
					{
						var minutesToAddNullable = GetNullableDouble(functionArgs.Parameters[4]);
						if (!minutesToAddNullable.HasValue)
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - Minutes to add must be a number.");
						}
						minutesToAdd = minutesToAddNullable.Value;
					}

					// Seconds to add
					double secondsToAdd = 0;
					if (functionArgs.Parameters.Length > 5)
					{
						var secondsToAddNullable = GetNullableDouble(functionArgs.Parameters[5]);
						if (!secondsToAddNullable.HasValue)
						{
							throw new FormatException($"{ExtensionFunction.DateTime} function - Seconds to add must be a number.");
						}
						secondsToAdd = secondsToAddNullable.Value;
					}

					functionArgs.Result = DateTimeOffset
						.UtcNow
						.AddDays(daysToAdd)
						.AddHours(hoursToAdd)
						.AddMinutes(minutesToAdd)
						.AddSeconds(secondsToAdd)
						.ToString(format);
					return;
				case ExtensionFunction.EndsWith:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");
					}
					functionArgs.Result = param1.EndsWith(param2, StringComparison.InvariantCulture);
					return;
				case ExtensionFunction.Format:
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
								functionArgs.Result = inputInt.ToString(formatFormat);
								return;
							case double inputDouble:
								functionArgs.Result = inputDouble.ToString(formatFormat);
								return;
							case DateTime dateTime:
								functionArgs.Result = dateTime.ToString(formatFormat);
								return;
							case string inputString:
								// Assume this is a number
								if (long.TryParse(inputString, out var longValue))
								{
									functionArgs.Result = longValue.ToString(formatFormat);
									return;
								}
								if (double.TryParse(inputString, out var doubleValue))
								{
									functionArgs.Result = doubleValue.ToString(formatFormat);
									return;
								}
								if (DateTimeOffset.TryParse(
									inputString,
									CultureInfo.InvariantCulture.DateTimeFormat,
									DateTimeStyles.AssumeUniversal,
									out var dateTimeOffsetValue))
								{
									functionArgs.Result = dateTimeOffsetValue.ToString(formatFormat);
									return;
								}
								throw new FormatException($"Could not parse '{inputString}' as a number or date.");
							default:
								throw new NotSupportedException($"Unsupported input type {inputObject.GetType().Name}");
						}
					}
				case ExtensionFunction.In:
					if (functionArgs.Parameters.Length < 2)
					{
						throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
					}
					try
					{
						var item = functionArgs.Parameters[0].Evaluate();
						var list = functionArgs.Parameters.Skip(1).Select(p => p.Evaluate()).ToList();
						functionArgs.Result = list.Contains(item);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
					}
				case ExtensionFunction.If:
					bool boolParam1;
					if (functionArgs.Parameters.Length != 3)
					{
						throw new FormatException($"{ExtensionFunction.If}() requires three parameters.");
					}
					try
					{
						boolParam1 = (bool)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
					}
					if (boolParam1)
					{
						try
						{
							functionArgs.Result = functionArgs.Parameters[1].Evaluate();
							return;
						}
						catch (NCalcExtensionsException)
						{
							throw;
						}
						catch (Exception e)
						{
							throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 2 '{functionArgs.Parameters[1].ParsedExpression}' due to {e.Message}.", e);
						}
					}
					try
					{
						functionArgs.Result = functionArgs.Parameters[2].Evaluate();
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 3 '{functionArgs.Parameters[2].ParsedExpression}' due to {e.Message}.", e);
					}
				case ExtensionFunction.IsInfinite:
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException($"{ExtensionFunction.IsInfinite}() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result =
							outputObject is double x && (
								double.IsPositiveInfinity(x)
								|| double.IsNegativeInfinity(x)
							);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case ExtensionFunction.IsNaN:
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException($"{ExtensionFunction.IsNaN}() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result = !(outputObject is double) || double.IsNaN((double)outputObject);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case ExtensionFunction.IsNull:
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException($"{ExtensionFunction.IsNull}() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result = outputObject == null;
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case ExtensionFunction.Contains:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");
					}
					functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture) >= 0;
					return;
				case ExtensionFunction.IndexOf:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.IndexOf}() requires two string parameters.");
					}
					functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture);
					return;
				case ExtensionFunction.LastIndexOf:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.LastIndexOf}() requires two string parameters.");
					}
					functionArgs.Result = param1.LastIndexOf(param2, StringComparison.InvariantCulture);
					return;
				case ExtensionFunction.Length:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Length}() requires one string parameter.");
					}
					functionArgs.Result = param1.Length;
					return;
				case ExtensionFunction.StartsWith:
					if (functionArgs.Parameters.Length != 2)
					{
						throw new FormatException($"{ExtensionFunction.StartsWith}() requires two parameters.");
					}
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate() as string;
						param2 = (string)functionArgs.Parameters[1].Evaluate() as string;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"Unexpected exception in {ExtensionFunction.StartsWith}(): {e.Message}", e);
					}
					if (param1 == null)
					{
						throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 1 is not a string");
					}
					if (param2 == null)
					{
						throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 2 is not a string");
					}
					functionArgs.Result = param1.StartsWith(param2, StringComparison.InvariantCulture);
					return;
				case ExtensionFunction.RegexGroup:
					try
					{
						var input = (string)functionArgs.Parameters[0].Evaluate();
						var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
						var regexCaptureIndex = functionArgs.Parameters.Length == 3
							? (int)functionArgs.Parameters[2].Evaluate()
							: 0;
						var regex = new Regex(regexExpression);
						if (!regex.IsMatch(input))
						{
							functionArgs.Result = null;
						}
						else
						{
							Group group = regex
								.Match(input)
								.Groups[1];
							functionArgs.Result = regexCaptureIndex >= group.Captures.Count
								? null
								: group.Captures[regexCaptureIndex].Value;
						}
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
					}
					return;
				case ExtensionFunction.RegexIsMatch:
					try
					{
						var input = (string)functionArgs.Parameters[0].Evaluate();
						var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
						var regex = new Regex(regexExpression);
						functionArgs.Result = regex.IsMatch(input);
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
					}
					return;
				case ExtensionFunction.Replace:
					try
					{
						var haystack = (string)functionArgs.Parameters[0].Evaluate();
						var needle = (string)functionArgs.Parameters[1].Evaluate();
						var newNeedle = (string)functionArgs.Parameters[2].Evaluate();
						functionArgs.Result = haystack.Replace(needle, newNeedle);
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
					}
					return;
				case ExtensionFunction.Substring:
					int startIndex;
					int length;
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						startIndex = (int)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
					}
					if (functionArgs.Parameters.Length > 2)
					{
						length = (int)functionArgs.Parameters[2].Evaluate();
						functionArgs.Result = param1.Substring(startIndex, length);
						return;
					}

					functionArgs.Result = param1.Substring(startIndex);
					return;
				case ExtensionFunction.Switch:
					{
						if (functionArgs.Parameters.Length < 3)
						{
							throw new FormatException($"{ExtensionFunction.Switch}() requires at least three parameters.");
						}
						object valueParam;
						try
						{
							valueParam = functionArgs.Parameters[0].Evaluate();
						}
						catch (NCalcExtensionsException)
						{
							throw;
						}
						catch (Exception e)
						{
							throw new FormatException($"Could not evaluate {ExtensionFunction.Switch} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
						}

						// Determine the pair count
						var pairCount = (functionArgs.Parameters.Length - 1) / 2;
						for (var pairIndex = 0; pairIndex < pairCount * 2; pairIndex += 2)
						{
							var caseIndex = 1 + pairIndex;
							var @case = functionArgs.Parameters[caseIndex].Evaluate();
							if (@case.Equals(valueParam))
							{
								functionArgs.Result = functionArgs.Parameters[caseIndex + 1].Evaluate();
								return;
							}
						}

						var defaultIsPresent = functionArgs.Parameters.Length % 2 == 0;
						if (defaultIsPresent)
						{
							functionArgs.Result = functionArgs.Parameters.Last().Evaluate();
							return;
						}

						throw new FormatException($"Default {ExtensionFunction.Switch} condition occurred, but no default case specified.");
					}
				case ExtensionFunction.Throw:
					switch (functionArgs.Parameters.Length)
					{
						case 0:
							throw new NCalcExtensionsException();
						case 1:
							if (!(functionArgs.Parameters[0].Evaluate() is string exceptionMessageText))
							{
								throw new FormatException($"{ExtensionFunction.Throw} function - parameter must be a string.");
							}
							throw new NCalcExtensionsException(exceptionMessageText);

						default:
							throw new FormatException($"{ExtensionFunction.Throw} function - takes zero or one parameters.");
					}
				case ExtensionFunction.TimeSpan:
					if (functionArgs.Parameters.Length != 3)
					{
						throw new FormatException($"{ExtensionFunction.TimeSpan} function - requires three parameters.");
					}
					string fromString;
					string toString;
					string timeUnitString;
					try
					{
						fromString = functionArgs.Parameters[0].Evaluate().ToString();
						toString = functionArgs.Parameters[1].Evaluate().ToString();
						timeUnitString = functionArgs.Parameters[2].Evaluate().ToString();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not extract three parameters into strings: {e.Message}");
					}

					if (!DateTime.TryParse(fromString, out var fromDateTime))
					{
						throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{fromString}' to DateTime");
					}
					if (!DateTime.TryParse(toString, out var toDateTime))
					{
						throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{toString}' to DateTime");
					}
					if (!Enum.TryParse(timeUnitString, true, out TimeUnit timeUnit))
					{
						throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{timeUnitString}' to a supported time unit");
					}

					// Determine the timespan
					var timeSpan = toDateTime - fromDateTime;
					functionArgs.Result = GetUnits(timeSpan, timeUnit);
					return;
				case ExtensionFunction.ToDateTime:
					{
						const int toDateTimeParameterCount = 2;
						if (functionArgs.Parameters.Length != toDateTimeParameterCount)
						{
							throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected {toDateTimeParameterCount} arguments");
						}
						if (!(functionArgs.Parameters[0].Evaluate() is string inputString))
						{
							throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected first argument to be a string.");
						}
						if (!(functionArgs.Parameters[1].Evaluate() is string formatString))
						{
							throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string.");
						}
						if (!DateTime.TryParseExact(inputString, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outputDateTime))
						{
							throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format.");
						}
						functionArgs.Result = outputDateTime;
						return;
					}
				case ExtensionFunction.ToLower:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.ToLower} function -  requires one string parameter.");
					}
					functionArgs.Result = param1.ToLowerInvariant();
					return;
				case ExtensionFunction.ToUpper:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.ToUpper} function -  requires one string parameter.");
					}
					functionArgs.Result = param1.ToUpperInvariant();
					return;
				case ExtensionFunction.Capitalise:
				case ExtensionFunction.Capitalize:
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Capitalize} function -  requires one string parameter.");
					}
					functionArgs.Result = param1.ToLowerInvariant().UpperCaseFirst();
					return;
				case ExtensionFunction.Humanise:
				case ExtensionFunction.Humanize:
					double param1Double;
					try
					{
						if (double.TryParse(functionArgs.Parameters[0].Evaluate().ToString(), out var result))
						{
							param1Double = result;
							param2 = (string)functionArgs.Parameters[1].Evaluate();
						}
						else
						{
							throw new Exception();
						}
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"{ExtensionFunction.Humanize} function - The first number should be a valid floating-point number and the second should be a time unit ({string.Join(", ", Enum.GetNames(typeof(TimeUnit)))}).");
					}

					if (!Enum.TryParse<TimeUnit>(param2, true, out var param2TimeUnit))
					{
						throw new FormatException($"{ExtensionFunction.Humanize} function - Parameter 2 must be a time unit - one of {string.Join(", ", Enum.GetNames(typeof(TimeUnit)).Select(n => $"'{n}'"))}.");
					}

					functionArgs.Result = Humanize(param1Double, param2TimeUnit);
					return;
			}
		}

		private static double? GetNullableDouble(Expression expression)
			=> (expression.Evaluate()) switch
			{
				double doubleResult => (double?)doubleResult,
				int intResult => (double?)intResult,
				_ => null,
			};

		public static string UpperCaseFirst(this string s)
			=> s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);

		private static double GetUnits(TimeSpan timeSpan, TimeUnit timeUnit)
			=> timeUnit switch
			{
				TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => timeSpan.TotalSeconds,
				TimeUnit.Minutes => timeSpan.TotalMinutes,
				TimeUnit.Hours => timeSpan.TotalHours,
				TimeUnit.Days => timeSpan.TotalDays,
				TimeUnit.Weeks => timeSpan.TotalDays / 7,
				TimeUnit.Years => timeSpan.TotalDays / 365.25,
				_ => throw new ArgumentOutOfRangeException($"Time unit not supported: '{timeUnit}'"),
			};

		[SuppressMessage("Design", "RCS1224:Make method an extension method.", Justification = "Nonsense")]
		internal static string Humanize(double param1Double, TimeUnit timeUnit)
		{
			try
			{
				return timeUnit switch
				{
					TimeUnit.Milliseconds => TimeSpan.FromMilliseconds(param1Double).Humanize(),
					TimeUnit.Seconds => TimeSpan.FromSeconds(param1Double).Humanize(),
					TimeUnit.Minutes => TimeSpan.FromMinutes(param1Double).Humanize(),
					TimeUnit.Hours => TimeSpan.FromHours(param1Double).Humanize(),
					TimeUnit.Days => TimeSpan.FromDays(param1Double).Humanize(),
					TimeUnit.Weeks => TimeSpan.FromDays(param1Double * 7).Humanize(),
					TimeUnit.Years => TimeSpan.FromDays(param1Double * 365.25).Humanize(),
					_ => throw new FormatException($"{timeUnit} is not a supported time unit for humanization."),
				};
			}
			catch (OverflowException)
			{
				throw new FormatException("The value is too big to use humanize. It must be a double (a 64-bit, floating point number)");
			}
		}

		public static string Humanize(this TimeSpan t)
		{
			// Humanize a TimeSpan into days, hours, minutes and seconds.
			var durationString = t.Days >= 1 ? $"{t.Days} day{(t.Days > 1 ? "s" : "")}" : "";

			if (t.Hours >= 1)
			{
				durationString += $" {t.Hours} hour{(t.Hours > 1 ? "s" : "")}";
			}

			if (t.Minutes >= 1)
			{
				durationString += $" {t.Minutes} minute{(t.Minutes > 1 ? "s" : "")}";
			}

			if (t.Seconds >= 1)
			{
				durationString += $" {t.Seconds} second{(t.Seconds > 1 ? "s" : "")}";
			}

			return durationString.Trim();
		}
	}
}
