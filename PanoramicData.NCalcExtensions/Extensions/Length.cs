using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Length
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1 switch
			{
				string a => a.Length,
				IList b => b.Count,
				_ => throw new Exception()
			};
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
		}
	}
}
