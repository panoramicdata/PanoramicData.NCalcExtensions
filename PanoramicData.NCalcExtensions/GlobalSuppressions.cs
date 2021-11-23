// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: SuppressMessage(
	 "Globalization",
	 "CA1308:Normalize strings to uppercase",
	 Justification = "This is a toLower function.",
	 Scope = "member",
	 Target = "~M:PanoramicData.NCalcExtensions.Extensions.ToLower.Evaluate(NCalc.FunctionArgs)")]
[assembly: SuppressMessage(
	 "Globalization",
	 "CA1308:Normalize strings to uppercase",
	 Justification = "This is the purpose of the function",
	 Scope = "member",
	 Target = "~M:PanoramicData.NCalcExtensions.Extensions.Capitalize.Evaluate(NCalc.FunctionArgs)")]
[assembly: SuppressMessage(
	 "Globalization",
	 "CA1303:Do not pass literals as localized parameters",
	 Justification = "The code library will not be localized.  See https://docs.microsoft.com/en-gb/visualstudio/code-quality/ca1303.",
	 Scope = "namespaceanddescendants",
	 Target = "~N:PanoramicData.NCalcExtensions")]
