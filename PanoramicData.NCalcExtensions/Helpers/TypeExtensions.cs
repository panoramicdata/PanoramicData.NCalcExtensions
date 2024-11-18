namespace PanoramicData.NCalcExtensions.Helpers;
internal static class TypeHelper
{
	internal static string AsHumanString<T>()
	{
		var typeName = typeof(T).IsGenericType
			? $"{typeof(T).GetGenericTypeDefinition().Name}<{string.Join(",", typeof(T).GetGenericArguments().Select(t => t.Name))}>"
			: typeof(T).Name;
		return typeName;
	}
}
