using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("concat")]
	[Description("Concatenates lists and objects.")]
	object Concat(
		[Description("The lists or objects to concatenate.")]
		params object[] parameters
	);
}

internal static class Concat
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var list = new List<object>();
		foreach (var parameter in functionArgs.Parameters)
		{
			var parameterValue = parameter.Evaluate();
			if (parameterValue is IList parameterValueAsIList)
			{
				foreach (var value in parameterValueAsIList)
				{
					list.Add(value);
				}
			}
			else
			{
				list.Add(parameterValue);
			}
		}

		functionArgs.Result = list;
	}
}