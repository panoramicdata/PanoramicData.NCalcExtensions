using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("canEvaluate")]
	[Description("Determines whether ALL of the parameters can be evaluated. This can be used, for example, to test whether a parameter is set.")]
	bool CanEvaluate(
		[Description("1 or more parameters to evaluate")]
		params object[] parameters
	);
}

internal static class CanEvaluate
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		try
		{
			foreach (var parameter in functionArgs.Parameters)
			{
				parameter.Evaluate();
			}

			functionArgs.Result = true;
		}
		catch
		{
			functionArgs.Result = false;
		}
	}
}
