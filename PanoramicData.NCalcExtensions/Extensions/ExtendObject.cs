using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("extendObject")]
	[Description("Extends an existing object into a JObject with both the original and additional properties.")]
	bool ExtendObject(
		[Description("The original object.")]
		object originalObject,
		[Description("The new properties to be added: Interlaced names and values. You must provide an even number of parameters, and names must evaluate to strings")]
		JObject newProperties
	);
}

internal static class ExtendObject
{
	internal static void Evaluate(IFunctionArgs functionArgs)
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
			jObject[propertyName] = propertyValue is null ? JValue.CreateNull() : JToken.FromObject(propertyValue);
		}

		functionArgs.Result = jObject;
	}
}
