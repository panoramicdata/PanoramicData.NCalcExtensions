namespace PanoramicData.NCalcExtensions.Helpers;

internal static class StringExtensions
{
	internal static string UpperCaseFirst(this string s)
		=> s == null
			? throw new ArgumentNullException(nameof(s))
			: s.Length == 0
				? string.Empty
				: $"{s[..1].ToUpperInvariant()}{s[1..]}";
}
