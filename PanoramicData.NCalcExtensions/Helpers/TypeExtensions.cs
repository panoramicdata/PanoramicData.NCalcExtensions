namespace PanoramicData.NCalcExtensions.Helpers;
internal static class TypeHelper
{
	internal static string AsHumanString<T>()
	{
		string typeName;
		if (typeof(T).IsGenericType)
		{
			typeName = $"{typeof(T).GetGenericTypeDefinition().Name}<{string.Join(",", typeof(T).GetGenericArguments().Select(t => t.Name))}>";
		}
		else
		{
			typeName = typeof(T).Name;
		}

		return typeName;
	}
}
