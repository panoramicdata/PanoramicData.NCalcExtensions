using NCalc;
using PanoramicData.NCalcExtensions.Extensions;
using System;

namespace PanoramicData.NCalcExtensions
{
	public static class NCalcExtensions
	{
		public static void Extend(string functionName, FunctionArgs functionArgs)
		{
			if (functionArgs == null)
			{
				throw new ArgumentNullException(nameof(functionArgs));
			}

			switch (functionName)
			{
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
				case ExtensionFunction.Contains:
					Contains.Evaluate(functionArgs);
					return;
				case ExtensionFunction.DateTime:
					DateTimeMethods.Evaluate(functionArgs);
					return;
				case ExtensionFunction.DateTimeAsEpochMs:
					DateTimeAsEpochMs.Evaluate(functionArgs);
					return;
				case ExtensionFunction.EndsWith:
					EndsWith.Evaluate(functionArgs);
					return;
				case ExtensionFunction.Format:
					Format.Evaluate(functionArgs);
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
				case ExtensionFunction.IsInfinite:
					IsInfinite.Evaluate(functionArgs);
					return;
				case ExtensionFunction.IsNaN:
					IsNaN.Evaluate(functionArgs);
					return;
				case ExtensionFunction.IsNull:
					IsNull.Evaluate(functionArgs);
					return;
				case ExtensionFunction.IsSet:
					IsSet.Evaluate(functionArgs);
					return;
				case ExtensionFunction.CanEvaluate:
					CanEvaluate.Evaluate(functionArgs);
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
				case ExtensionFunction.StartsWith:
					StartsWith.Evaluate(functionArgs);
					return;
				case ExtensionFunction.Substring:
					Substring.Evaluate(functionArgs);
					return;
				case ExtensionFunction.Switch:
					Switch.Evaluate(functionArgs);
					return;
				case ExtensionFunction.Throw:
					throw Throw.Evaluate(functionArgs);
				case ExtensionFunction.TimeSpan:
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
			}
		}

		public static string UpperCaseFirst(this string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException(nameof(s));
			}

			return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
		}

		public static string Humanise(this System.TimeSpan t)
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
