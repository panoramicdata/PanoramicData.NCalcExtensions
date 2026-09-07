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

	/// <summary>
	/// The time source used by time-dependent functions (now, dateTimeIsInPast, dateTimeIsInFuture).
	/// Defaults to <see cref="System.TimeProvider.System"/> so behaviour is unchanged unless a provider is supplied.
	/// </summary>
	private readonly TimeProvider _timeProvider;

	internal const string StorageDictionaryParameterName = "__storageDictionary";
	public static readonly ExpressionOptions ExtendedExpressionDefaults = ExpressionOptions.None;

	protected BaseExtendedExpression(
		string expression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo,
		TimeProvider? timeProvider = null) : base(expression, expressionOptions, cultureInfo)
	{
		CultureInfo = cultureInfo;
		_timeProvider = timeProvider ?? TimeProvider.System;
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
	/// Set parameters from typed definitions.
	/// Used by both ExtendedExpression and SimpleExtendedExpression.
	/// </summary>
	protected void SetParametersFromDefinitions(Dictionary<string, TypedDefinition> parameterDefinitions)
	{
		foreach (var kvp in parameterDefinitions)
		{
			var parameterName = kvp.Key;
			var definition = kvp.Value;

			if (!definition.HasValue)
			{
				continue;
			}

			Parameters[parameterName] = ConvertDefinitionValue($"Parameter '{parameterName}'", definition);
		}
	}

	/// <summary>
	/// Parses a typed definition's literal value, keyed by the definition's underlying type.
	/// </summary>
	/// <remarks>
	/// A lookup table rather than a chain of 15 type tests, which carried no logic beyond choosing
	/// the parser to call.
	/// </remarks>
	private static readonly FrozenDictionary<Type, Func<string, CultureInfo, object>> DefinitionValueParsers =
		new Dictionary<Type, Func<string, CultureInfo, object>>
		{
			[typeof(string)] = static (value, _) => value,
			[typeof(bool)] = static (value, _) => bool.Parse(value),
			[typeof(Guid)] = static (value, _) => Guid.Parse(value),
			[typeof(sbyte)] = static (value, culture) => sbyte.Parse(value, culture),
			[typeof(byte)] = static (value, culture) => byte.Parse(value, culture),
			[typeof(short)] = static (value, culture) => short.Parse(value, culture),
			[typeof(ushort)] = static (value, culture) => ushort.Parse(value, culture),
			[typeof(int)] = static (value, culture) => int.Parse(value, culture),
			[typeof(uint)] = static (value, culture) => uint.Parse(value, culture),
			[typeof(long)] = static (value, culture) => long.Parse(value, culture),
			[typeof(ulong)] = static (value, culture) => ulong.Parse(value, culture),
			[typeof(float)] = static (value, culture) => float.Parse(value, culture),
			[typeof(double)] = static (value, culture) => double.Parse(value, culture),
			[typeof(decimal)] = static (value, culture) => decimal.Parse(value, culture),
			[typeof(DateTime)] = static (value, culture) => DateTime.Parse(value, culture),
		}.ToFrozenDictionary();

	protected object? ConvertDefinitionValue(string definitionName, TypedDefinition definition)
	{
		try
		{
			if (!TypedDefinitionTypeResolver.TryResolve(definition.TypeName, out var resolvedType))
			{
				throw new FormatException($"{definitionName}: Type '{definition.TypeName}' could not be resolved.");
			}

			if (!definition.HasValue)
			{
				return null;
			}

			if (definition.IsNull)
			{
				return resolvedType.AllowsNull
					? null
					: throw new FormatException($"{definitionName}: Type '{definition.TypeName}' does not allow null values.");
			}

			var valueString = definition.Value
				?? throw new FormatException($"{definitionName}: A literal value was expected.");

			if (!DefinitionValueParsers.TryGetValue(resolvedType.UnderlyingType, out var parse))
			{
				throw new FormatException($"{definitionName}: Type '{definition.TypeName}' is not supported for typed definitions.");
			}

			return parse(valueString, CultureInfo);
		}
		catch (Exception ex) when (ex is not FormatException)
		{
			throw new FormatException($"{definitionName}: Failed to parse value '{definition.Value}' as type '{definition.TypeName}'. {ex.Message}", ex);
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
		FunctionEventArgs functionArgs,
		int? minPropertyCount,
		int? maxPropertyCount)
	{
		if (minPropertyCount is not null && functionArgs.Parameters.Count < minPropertyCount)
		{
			throw new FormatException($"{functionName}: At least {minPropertyCount} parameter{(minPropertyCount == 1 ? "" : "s")} required.");
		}

		if (maxPropertyCount is not null && functionArgs.Parameters.Count > maxPropertyCount)
		{
			throw new FormatException($"{functionName}: No more than {maxPropertyCount} parameter{(maxPropertyCount == 1 ? "" : "s")} permitted.");
		}
	}

	/// <summary>
	/// Evaluates one extension function, writing its result to <paramref name="functionArgs"/>.
	/// </summary>
	private delegate void FunctionHandler(BaseExtendedExpression expression, FunctionEventArgs functionArgs);

	/// <summary>
	/// Maps each extension function name to the handler that evaluates it.
	/// </summary>
	/// <remarks>
	/// A lookup table rather than a switch statement: the dispatch carries no logic of its own, and
	/// a table cannot fall through or omit a return the way a 100-case switch can. Entries are
	/// grouped by what the function operates on, and alphabetical within each group.
	/// </remarks>
	private static readonly FrozenDictionary<string, FunctionHandler> FunctionHandlers = BuildFunctionHandlers();

	private static FrozenDictionary<string, FunctionHandler> BuildFunctionHandlers()
	{
		var handlers = new Dictionary<string, FunctionHandler>(StringComparer.Ordinal);
		AddSequenceHandlers(handlers);
		AddTextHandlers(handlers);
		AddDateAndTimeHandlers(handlers);
		AddJsonAndObjectHandlers(handlers);
		AddValueAndTypeHandlers(handlers);
		AddControlFlowHandlers(handlers);
		return handlers.ToFrozenDictionary(StringComparer.Ordinal);
	}

	/// <summary>Handlers for functions over sequences and collections.</summary>
	private static void AddSequenceHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.All] = static (_, args) => All.Evaluate(args);
		handlers[ExtensionFunction.Any] = static (_, args) => Any.Evaluate(args);
		handlers[ExtensionFunction.Average] = static (_, args) => AverageFunction.Evaluate(args);
		handlers[ExtensionFunction.Concat] = static (_, args) => Concat.Evaluate(args);
		handlers[ExtensionFunction.Contains] = static (_, args) => Contains.Evaluate(args);
		handlers[ExtensionFunction.Count] = static (_, args) => Count.Evaluate(args);
		handlers[ExtensionFunction.CountBy] = static (_, args) => CountBy.Evaluate(args);
		handlers[ExtensionFunction.Distinct] = static (_, args) => Distinct.Evaluate(args);
		handlers[ExtensionFunction.First] = static (_, args) => First.Evaluate(args);
		handlers[ExtensionFunction.FirstOrDefault] = static (_, args) => FirstOrDefault.Evaluate(args);
		handlers[ExtensionFunction.Flatten] = static (_, args) => FlattenFunction.Evaluate(args);
		handlers[ExtensionFunction.In] = static (_, args) => In.Evaluate(args);
		handlers[ExtensionFunction.ItemAtIndex] = static (_, args) => ItemAtIndex.Evaluate(args);
		handlers[ExtensionFunction.Join] = static (_, args) => Join.Evaluate(args);
		handlers[ExtensionFunction.Last] = static (_, args) => Last.Evaluate(args);
		handlers[ExtensionFunction.LastOrDefault] = static (_, args) => LastOrDefault.Evaluate(args);
		handlers[ExtensionFunction.Length] = static (_, args) => Length.Evaluate(args);
		handlers[ExtensionFunction.List] = static (_, args) => List.Evaluate(args);
		handlers[ExtensionFunction.ListOf] = static (expression, args) => ListOf.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.Max] = static (_, args) => Max.Evaluate(args);
		handlers[ExtensionFunction.Min] = static (_, args) => Min.Evaluate(args);
		handlers[ExtensionFunction.OrderBy] = static (_, args) => OrderBy.Evaluate(args);
		handlers[ExtensionFunction.Reverse] = static (_, args) => Reverse.Evaluate(args);
		handlers[ExtensionFunction.Select] = static (_, args) => Extensions.Select.Evaluate(args);
		handlers[ExtensionFunction.SelectDistinct] = static (_, args) => SelectDistinct.Evaluate(args);
		handlers[ExtensionFunction.Skip] = static (_, args) => Skip.Evaluate(args);
		handlers[ExtensionFunction.Sort] = static (_, args) => Sort.Evaluate(args);
		handlers[ExtensionFunction.Sum] = static (_, args) => Sum.Evaluate(args);
		handlers[ExtensionFunction.Take] = static (_, args) => Take.Evaluate(args);
		handlers[ExtensionFunction.Where] = static (_, args) => Where.Evaluate(args);
	}

	/// <summary>Handlers for functions over strings and text.</summary>
	private static void AddTextHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.Capitalise] = static (_, args) => Capitalize.Evaluate(args);
		handlers[ExtensionFunction.Capitalize] = static (_, args) => Capitalize.Evaluate(args);
		handlers[ExtensionFunction.EndsWith] = static (_, args) => EndsWith.Evaluate(args);
		handlers[ExtensionFunction.Format] = static (expression, args) => Format.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.Humanise] = static (_, args) => Humanize.Evaluate(args);
		handlers[ExtensionFunction.Humanize] = static (_, args) => Humanize.Evaluate(args);
		handlers[ExtensionFunction.IndexOf] = static (_, args) => IndexOf.Evaluate(args);
		handlers[ExtensionFunction.LastIndexOf] = static (_, args) => LastIndexOf.Evaluate(args);
		handlers[ExtensionFunction.PadLeft] = static (_, args) => PadLeft.Evaluate(args);
		handlers[ExtensionFunction.PadRight] = static (_, args) => PadRight.Evaluate(args);
		handlers[ExtensionFunction.RegexGroup] = static (_, args) => RegexGroup.Evaluate(args);
		handlers[ExtensionFunction.RegexIsMatch] = static (_, args) => RegexIsMatch.Evaluate(args);
		handlers[ExtensionFunction.RegexReplace] = static (_, args) => RegexReplaceFunction.Evaluate(args);
		handlers[ExtensionFunction.Repeat] = static (_, args) => RepeatFunction.Evaluate(args);
		handlers[ExtensionFunction.Replace] = static (_, args) => Replace.Evaluate(args);
		handlers[ExtensionFunction.Sanitize] = static (_, args) => Sanitize.Evaluate(args);
		handlers[ExtensionFunction.Sha256] = static (_, args) => Sha256.Evaluate(args);
		handlers[ExtensionFunction.Split] = static (_, args) => Split.Evaluate(args);
		handlers[ExtensionFunction.StartsWith] = static (_, args) => StartsWith.Evaluate(args);
		handlers[ExtensionFunction.Substring] = static (_, args) => Substring.Evaluate(args);
		handlers[ExtensionFunction.TitleCase] = static (_, args) => TitleCaseFunction.Evaluate(args);
		handlers[ExtensionFunction.ToLower] = static (_, args) => ToLower.Evaluate(args);
		handlers[ExtensionFunction.ToString] = static (expression, args) => Extensions.ToString.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.ToUpper] = static (_, args) => ToUpper.Evaluate(args);
		handlers[ExtensionFunction.Trim] = static (_, args) => Trim.Evaluate(args);
		handlers[ExtensionFunction.Truncate] = static (_, args) => TruncateFunction.Evaluate(args);
	}

	/// <summary>Handlers for functions over dates, times and durations.</summary>
	private static void AddDateAndTimeHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.ChangeTimeZone] = static (_, args) => ChangeTimeZone.Evaluate(args);
		handlers[ExtensionFunction.DateAdd] = static (_, args) => DateAddMethods.Evaluate(args);
		handlers[ExtensionFunction.DateTime] = static (expression, args) => DateTimeMethods.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.DateTimeAsEpoch] = static (expression, args) => DateTimeAsEpoch.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.DateTimeAsEpochMs] = static (expression, args) => DateTimeAsEpochMs.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.DateTimeIsInFuture] = static (expression, args) => DateTimeIsInFuture.Evaluate(args, expression._timeProvider);
		handlers[ExtensionFunction.DateTimeIsInPast] = static (expression, args) => DateTimeIsInPast.Evaluate(args, expression._timeProvider);
		handlers[ExtensionFunction.DateTimeIsInWindow] = static (expression, args) => DateTimeIsInWindow.Evaluate(args, expression._timeProvider);
		handlers[ExtensionFunction.Now] = static (expression, args) => Now.Evaluate(args, expression._timeProvider);
		handlers[ExtensionFunction.TimeSpan] = static (expression, args) => Extensions.TimeSpan.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.TimeSpanCamel] = static (expression, args) => Extensions.TimeSpan.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.ToDateTime] = static (expression, args) => ToDateTime.Evaluate(args, expression.CultureInfo);
	}

	/// <summary>Handlers for functions over JSON documents and object properties.</summary>
	private static void AddJsonAndObjectHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.Dictionary] = static (_, args) => Dictionary.Evaluate(args);
		handlers[ExtensionFunction.Extend] = static (_, args) => ExtendObject.Evaluate(args);
		handlers[ExtensionFunction.GetProperties] = static (_, args) => GetProperties.Evaluate(args);
		handlers[ExtensionFunction.GetProperty] = static (_, args) => GetProperty.Evaluate(args);
		handlers[ExtensionFunction.JPath] = static (_, args) => JPath.Evaluate(args);
		handlers[ExtensionFunction.NewJArray] = static (_, args) => NewJArray.Evaluate(args);
		handlers[ExtensionFunction.NewJObject] = static (_, args) => NewJObject.Evaluate(args);
		handlers[ExtensionFunction.NewJsonArray] = static (_, args) => NewJsonArray.Evaluate(args);
		handlers[ExtensionFunction.NewJsonDocument] = static (_, args) => NewJsonDocument.Evaluate(args);
		handlers[ExtensionFunction.SetProperties] = static (_, args) => SetProperties.Evaluate(args);
	}

	/// <summary>Handlers for functions over individual values, their types and conversions between them.</summary>
	private static void AddValueAndTypeHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.Cast] = static (expression, args) => Cast.Evaluate(args, expression.CultureInfo);
		handlers[ExtensionFunction.Clamp] = static (_, args) => ClampFunction.Evaluate(args);
		handlers[ExtensionFunction.Convert] = static (_, args) => ConvertFunction.Evaluate(args);
		handlers[ExtensionFunction.IsGuid] = static (_, args) => IsGuid.Evaluate(args);
		handlers[ExtensionFunction.IsInfinite] = static (_, args) => IsInfinite.Evaluate(args);
		handlers[ExtensionFunction.IsNaN] = static (_, args) => IsNaN.Evaluate(args);
		handlers[ExtensionFunction.IsNull] = static (_, args) => IsNull.Evaluate(args);
		handlers[ExtensionFunction.IsNullOrEmpty] = static (_, args) => IsNullOrEmpty.Evaluate(args);
		handlers[ExtensionFunction.IsNullOrWhiteSpace] = static (_, args) => IsNullOrWhiteSpace.Evaluate(args);
		handlers[ExtensionFunction.IsSet] = static (_, args) => IsSet.Evaluate(args);
		handlers[ExtensionFunction.MaxValue] = static (_, args) => MaxValue.Evaluate(args);
		handlers[ExtensionFunction.MinValue] = static (_, args) => MinValue.Evaluate(args);
		handlers[ExtensionFunction.NullCoalesce] = static (_, args) => NullCoalesce.Evaluate(args);
		handlers[ExtensionFunction.Parse] = static (_, args) => Parse.Evaluate(args);
		handlers[ExtensionFunction.ParseInt] = static (_, args) => ParseInt.Evaluate(args);
		handlers[ExtensionFunction.TryParse] = static (expression, args) => TryParse.Evaluate(args, expression.StorageDictionary);
		handlers[ExtensionFunction.TypeOf] = static (_, args) => TypeOf.Evaluate(args);
	}

	/// <summary>Handlers for functions over control flow and stored state.</summary>
	private static void AddControlFlowHandlers(Dictionary<string, FunctionHandler> handlers)
	{
		handlers[ExtensionFunction.CanEvaluate] = static (_, args) => CanEvaluate.Evaluate(args);
		handlers[ExtensionFunction.If] = static (_, args) => Extensions.If.Evaluate(args);
		handlers[ExtensionFunction.Retrieve] = static (_, args) => Retrieve.Evaluate(args);
		handlers[ExtensionFunction.Store] = static (_, args) => Store.Evaluate(args);
		handlers[ExtensionFunction.Switch] = static (_, args) => Switch.Evaluate(args);
		handlers[ExtensionFunction.Throw] = static (_, args) => throw Throw.Evaluate(args);
		handlers[ExtensionFunction.Try] = static (_, args) => Try.Evaluate(args);
	}

	internal void Extend(string functionName, FunctionEventArgs functionArgs)
	{
		ArgumentNullException.ThrowIfNull(functionArgs);

		// A name with no handler is one of NCalc's own built-in functions. Leaving Result unset
		// is what tells NCalc to evaluate it itself, so an unknown name is not an error here.
		if (FunctionHandlers.TryGetValue(functionName, out var handler))
		{
			handler(this, functionArgs);
		}
	}
}
