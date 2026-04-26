namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Abstract base class for extended NCalc expressions.
/// Contains all common functionality between ExtendedExpression and SimpleExtendedExpression.
/// </summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
public abstract class BaseExtendedExpression : Expression
{
	protected readonly Dictionary<string, object?> StorageDictionary = [];
	protected new readonly CultureInfo CultureInfo;
#pragma warning restore CA1051 // Do not declare visible instance fields

	internal const string StorageDictionaryParameterName = "__storageDictionary";
	public static readonly ExpressionOptions ExtendedExpressionDefaults = ExpressionOptions.None;

	protected BaseExtendedExpression(
		string expression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : base(expression, expressionOptions, cultureInfo)
	{
		CultureInfo = cultureInfo;
		Parameters[StorageDictionaryParameterName] = StorageDictionary;
		EvaluateFunction += Extend;
		InitializeBuiltInParameters();
	}

	/// <summary>
	/// Initialize built-in parameters (null, True, False).
	/// </summary>
	protected void InitializeBuiltInParameters()
	{
		if (Parameters.ContainsKey("null"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'null', as it is a reserved keyword.");
		}

		Parameters["null"] = null;
		if (Parameters.ContainsKey("True"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'True', as it is a reserved keyword.");
		}

		Parameters["True"] = true;
		if (Parameters.ContainsKey("False"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'False', as it is a reserved keyword.");
		}

		Parameters["False"] = false;
	}

	/// <summary>
	/// Set parameters from a dictionary of (typeName, valueString) tuples.
	/// Used by both ExtendedExpression and SimpleExtendedExpression.
	/// </summary>
	protected void SetParametersFromDefinitions(Dictionary<string, (string TypeName, string Value)> parameterDefinitions)
	{
		foreach (var kvp in parameterDefinitions)
		{
			var parameterName = kvp.Key;
			var (typeName, valueString) = kvp.Value;

			try
			{
				var type = Type.GetType(typeName);
				if (type == null)
				{
					throw new FormatException($"Parameter '{parameterName}': Type '{typeName}' could not be resolved.");
				}

				// Convert the string value to the appropriate type
				object? convertedValue;
				if (type == typeof(string))
				{
					convertedValue = valueString;
				}
				else if (type == typeof(int))
				{
					convertedValue = int.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(long))
				{
					convertedValue = long.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(double))
				{
					convertedValue = double.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(decimal))
				{
					convertedValue = decimal.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(float))
				{
					convertedValue = float.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(bool))
				{
					convertedValue = bool.Parse(valueString);
				}
				else if (type == typeof(DateTime))
				{
					convertedValue = DateTime.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(Guid))
				{
					convertedValue = Guid.Parse(valueString);
				}
				else if (type == typeof(byte))
				{
					convertedValue = byte.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(short))
				{
					convertedValue = short.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(uint))
				{
					convertedValue = uint.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(ulong))
				{
					convertedValue = ulong.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(ushort))
				{
					convertedValue = ushort.Parse(valueString, CultureInfo);
				}
				else if (type == typeof(sbyte))
				{
					convertedValue = sbyte.Parse(valueString, CultureInfo);
				}
				else
				{
					throw new FormatException($"Parameter '{parameterName}': Type '{typeName}' is not supported for parameter definition.");
				}

				Parameters[parameterName] = convertedValue;
			}
			catch (FormatException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new FormatException($"Parameter '{parameterName}': Failed to parse value '{valueString}' as type '{typeName}'. {ex.Message}", ex);
			}
		}
	}

	/// <summary>
	/// Set a parameter value.
	/// </summary>
	public void SetParameter(string name, object? value)
	{
		Parameters[name] = value;
	}

	/// <summary>
	/// Set multiple parameters at once.
	/// </summary>
	public void SetParameters(Dictionary<string, object?> parameters)
	{
		foreach (var kvp in parameters)
		{
			Parameters[kvp.Key] = kvp.Value;
		}
	}

	/// <summary>
	/// Remove /* */ multi-line comments from the expression.
	/// </summary>
	protected static string RemoveMultilineComments(string expression)
	{
		return ExpressionCommentParsing.RemoveMultilineComments(expression);
	}

	internal static void CheckParameterCount(
		string functionName,
		FunctionArgs functionArgs,
		int? minPropertyCount,
		int? maxPropertyCount)
	{
		if (minPropertyCount is not null && functionArgs.Parameters.Length < minPropertyCount)
		{
			throw new FormatException($"{functionName}: At least {minPropertyCount} parameter{(minPropertyCount == 1 ? "" : "s")} required.");
		}

		if (maxPropertyCount is not null && functionArgs.Parameters.Length > maxPropertyCount)
		{
			throw new FormatException($"{functionName}: No more than {maxPropertyCount} parameter{(maxPropertyCount == 1 ? "" : "s")} permitted.");
		}
	}

	internal void Extend(string functionName, FunctionArgs functionArgs)
	{
		ArgumentNullException.ThrowIfNull(functionArgs);

		switch (functionName)
		{
			case ExtensionFunction.All:
				All.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Any:
				Any.Evaluate(functionArgs);
				return;
			case ExtensionFunction.CanEvaluate:
				CanEvaluate.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Capitalise:
			case ExtensionFunction.Capitalize:
				Capitalize.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Cast:
				Cast.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.ChangeTimeZone:
				ChangeTimeZone.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Concat:
				Concat.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Contains:
				Contains.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Convert:
				ConvertFunction.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Count:
				Count.Evaluate(functionArgs);
				return;
			case ExtensionFunction.CountBy:
				CountBy.Evaluate(functionArgs);
				return;
			case ExtensionFunction.DateAdd:
				DateAddMethods.Evaluate(functionArgs);
				return;
			case ExtensionFunction.DateTime:
				DateTimeMethods.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.DateTimeAsEpoch:
				DateTimeAsEpoch.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.DateTimeAsEpochMs:
				DateTimeAsEpochMs.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.DateTimeIsInPast:
				DateTimeIsInPast.Evaluate(functionArgs);
				return;
			case ExtensionFunction.DateTimeIsInFuture:
				DateTimeIsInFuture.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Dictionary:
				Dictionary.Evaluate(functionArgs);
				break;
			case ExtensionFunction.Distinct:
				Distinct.Evaluate(functionArgs);
				return;
			case ExtensionFunction.EndsWith:
				EndsWith.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Extend:
				ExtendObject.Evaluate(functionArgs);
				return;
			case ExtensionFunction.First:
				First.Evaluate(functionArgs);
				return;
			case ExtensionFunction.FirstOrDefault:
				FirstOrDefault.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Format:
				Format.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.GetProperty:
				GetProperty.Evaluate(functionArgs);
				return;
			case ExtensionFunction.GetProperties:
				GetProperties.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Humanise:
			case ExtensionFunction.Humanize:
				Humanize.Evaluate(functionArgs);
				return;
			case ExtensionFunction.In:
				In.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IndexOf:
				IndexOf.Evaluate(functionArgs);
				return;
			case ExtensionFunction.If:
				If.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsGuid:
				IsGuid.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsInfinite:
				IsInfinite.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsNaN:
				IsNaN.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsNull:
				IsNull.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsNullOrEmpty:
				IsNullOrEmpty.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsNullOrWhiteSpace:
				IsNullOrWhiteSpace.Evaluate(functionArgs);
				return;
			case ExtensionFunction.IsSet:
				IsSet.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ItemAtIndex:
				ItemAtIndex.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Join:
				Join.Evaluate(functionArgs);
				return;
			case ExtensionFunction.JPath:
				JPath.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Last:
				Last.Evaluate(functionArgs);
				return;
			case ExtensionFunction.LastIndexOf:
				LastIndexOf.Evaluate(functionArgs);
				return;
			case ExtensionFunction.LastOrDefault:
				LastOrDefault.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Length:
				Length.Evaluate(functionArgs);
				return;
			case ExtensionFunction.List:
				List.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ListOf:
				ListOf.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.Max:
				Max.Evaluate(functionArgs);
				return;
			case ExtensionFunction.MaxValue:
				MaxValue.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Min:
				Min.Evaluate(functionArgs);
				return;
			case ExtensionFunction.MinValue:
				MinValue.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Now:
				Now.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NullCoalesce:
				NullCoalesce.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NewJArray:
				NewJArray.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NewJObject:
				NewJObject.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NewJsonDocument:
				NewJsonDocument.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NewJsonArray:
				NewJsonArray.Evaluate(functionArgs);
				return;
			case ExtensionFunction.OrderBy:
				OrderBy.Evaluate(functionArgs);
				return;
			case ExtensionFunction.PadLeft:
				PadLeft.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Parse:
				Parse.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ParseInt:
				ParseInt.Evaluate(functionArgs);
				return;
			case ExtensionFunction.RegexGroup:
				RegexGroup.Evaluate(functionArgs);
				return;
			case ExtensionFunction.RegexIsMatch:
				RegexIsMatch.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Replace:
				Replace.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Retrieve:
				Retrieve.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Reverse:
				Reverse.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Sanitize:
				Sanitize.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Select:
				Select.Evaluate(functionArgs);
				return;
			case ExtensionFunction.SelectDistinct:
				SelectDistinct.Evaluate(functionArgs);
				return;
			case ExtensionFunction.SetProperties:
				SetProperties.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Sha256:
				Sha256.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Skip:
				Skip.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Sort:
				Sort.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Split:
				Split.Evaluate(functionArgs);
				return;
			case ExtensionFunction.StartsWith:
				StartsWith.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Store:
				Store.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Substring:
				Substring.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Sum:
				Sum.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Switch:
				Switch.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Take:
				Take.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Throw:
				throw Throw.Evaluate(functionArgs);
			case ExtensionFunction.TimeSpan:
			case ExtensionFunction.TimeSpanCamel:
				Extensions.TimeSpan.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.ToDateTime:
				ToDateTime.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.ToLower:
				ToLower.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ToString:
				Extensions.ToString.Evaluate(functionArgs, CultureInfo);
				return;
			case ExtensionFunction.ToUpper:
				ToUpper.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Trim:
				Trim.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Try:
				Try.Evaluate(functionArgs);
				return;
			case ExtensionFunction.TryParse:
				TryParse.Evaluate(functionArgs, StorageDictionary);
				return;
			case ExtensionFunction.TypeOf:
				TypeOf.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Where:
				Where.Evaluate(functionArgs);
				return;
			default:
				return;
		}
	}
}
