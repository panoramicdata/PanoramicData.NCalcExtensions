namespace PanoramicData.NCalcExtensions.Helpers;
internal static class StringExtensions
{
	internal static string UpperCaseFirst(this string s)
	{
		if (s == null)
		{
			throw new ArgumentNullException(nameof(s));
		}

		return s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
	}
}
