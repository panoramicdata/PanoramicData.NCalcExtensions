using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions;

public class ExtendedExpression : Expression
{
	private readonly Dictionary<string, object?> _storageDictionary = new();
	internal const string StorageDictionaryParameterName = "__storageDictionary";

	public ExtendedExpression(string expression) : base(expression)
	{
		Parameters[StorageDictionaryParameterName] = _storageDictionary;
		EvaluateFunction += Extend;
		CacheEnabled = false;
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
		if (functionArgs == null)
		{
			throw new ArgumentNullException(nameof(functionArgs));
		}

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
				Cast.Evaluate(functionArgs);
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
			case ExtensionFunction.DateTime:
				DateTimeMethods.Evaluate(functionArgs);
				return;
			case ExtensionFunction.DateTimeAsEpochMs:
				DateTimeAsEpochMs.Evaluate(functionArgs);
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
				Format.Evaluate(functionArgs);
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
			case ExtensionFunction.LastIndexOf:
				LastIndexOf.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Length:
				Length.Evaluate(functionArgs);
				return;
			case ExtensionFunction.List:
				List.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ListOf:
				ListOf.Evaluate(functionArgs);
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
			case ExtensionFunction.NullCoalesce:
				NullCoalesce.Evaluate(functionArgs);
				return;
			case ExtensionFunction.NewJObject:
				NewJObject.Evaluate(functionArgs);
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
			case ExtensionFunction.Select:
				Select.Evaluate(functionArgs);
				return;
			case ExtensionFunction.SelectDistinct:
				SelectDistinct.Evaluate(functionArgs);
				return;
			case ExtensionFunction.SetProperties:
				SetProperties.Evaluate(functionArgs);
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
				Extensions.TimeSpan.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ToDateTime:
				ToDateTime.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ToLower:
				ToLower.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ToString:
				Extensions.ToString.Evaluate(functionArgs);
				return;
			case ExtensionFunction.ToUpper:
				ToUpper.Evaluate(functionArgs);
				return;
			case ExtensionFunction.Try:
				Try.Evaluate(functionArgs);
				return;
			case ExtensionFunction.TryParse:
				TryParse.Evaluate(functionArgs, _storageDictionary);
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
