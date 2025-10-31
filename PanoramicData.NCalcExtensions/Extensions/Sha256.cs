using System.Security.Cryptography;
using System.Text;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("sha256")]
	[Description("Converts a string to a SHA 256 hash.")]
	string Sha256(
		[Description("The string value to be converted.")]
		string value
	);
}

internal static class Sha256
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterCount = functionArgs.Parameters.Length;
		switch (parameterCount)
		{
			case 1:
				var parameter1 = functionArgs.Parameters[0].Evaluate();
				functionArgs.Result = parameter1 switch
				{
					// The SHA 256 of the string
					string text => GetSha256(text),
					_ => throw new FormatException($"{ExtensionFunction.Sha256} function -  requires one string parameter")
				};
				break;
			default:
				throw new FormatException($"{ExtensionFunction.Sha256} function -  requires one string parameter");
		}
	}

	private static string GetSha256(string text)
	{
		var encodedBytes = Encoding.UTF8.GetBytes(text);
		var hashBytes = SHA256.HashData(encodedBytes);
		var hash = new StringBuilder();
		foreach (var b in hashBytes)
		{
			hash.Append(b.ToString("x2", CultureInfo.InvariantCulture));
		}

		return hash.ToString();
	}
}
