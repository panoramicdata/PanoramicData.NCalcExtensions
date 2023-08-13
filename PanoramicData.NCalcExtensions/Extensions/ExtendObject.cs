using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ExtendObject
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		// Determine the type
		(
			var originalObject,
			var extensionObjectList
		) = Parameters.GetParameters<object, List<object>>(functionArgs);

		if (extensionObjectList.Count % 2 != 0)
		{
			throw new FormatException("Extension object list must have an even number of parameters, in the form string propertyName1, object value1, propertyName2, object value2, ...");
		}

		var jObject = JObject.FromObject(originalObject);

		for (var i = 0; i < extensionObjectList.Count; i += 2)
		{
			var propertyName = extensionObjectList[i] as string ?? throw new FormatException("Property name must be a string");
			var propertyValue = extensionObjectList[i + 1];
			jObject[propertyName] = JToken.FromObject(propertyValue);
		}

		functionArgs.Result = jObject;
	}
}
