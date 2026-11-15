namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>A type's TryParse method.</summary>
internal delegate bool TryParseValue<T>(string? text, out T value);

/// <summary>Receives each value type that parse() and tryParse() accept.</summary>
internal interface IParsableValueTypeVisitor
{
	void Add<T>(TryParseValue<T> tryParse, params string[] typeNames);
}

/// <summary>The value types, and the names for each, that parse() and tryParse() accept.</summary>
internal static class ParsableValueTypes
{
	internal static void Visit(IParsableValueTypeVisitor visitor)
	{
		visitor.Add<bool>(bool.TryParse, "bool", "System.Boolean");
		visitor.Add<sbyte>(sbyte.TryParse, "sbyte", "System.SByte");
		visitor.Add<byte>(byte.TryParse, "byte", "System.Byte");
		visitor.Add<short>(short.TryParse, "short", "System.Int16");
		visitor.Add<ushort>(ushort.TryParse, "ushort", "System.UInt16");
		visitor.Add<int>(int.TryParse, "int", "System.Int32");
		visitor.Add<uint>(uint.TryParse, "uint", "System.UInt32");
		visitor.Add<long>(long.TryParse, "long", "System.Int64");
		visitor.Add<ulong>(ulong.TryParse, "ulong", "System.UInt64");
		visitor.Add<double>(double.TryParse, "double", "System.Double");
		visitor.Add<float>(float.TryParse, "float", "System.Single");
		visitor.Add<decimal>(decimal.TryParse, "decimal", "System.Decimal");
		visitor.Add<Guid>(Guid.TryParse, "Guid", "System.Guid");
	}
}

/// <summary>Reads the text arguments that parse() and tryParse() share.</summary>
internal static class ParseArguments
{
	internal static string ReadString(FunctionEventArgs functionArgs, int index, string ordinal)
		=> functionArgs.Parameters.Evaluate(index) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - {ordinal} parameter should be a string.");
}
