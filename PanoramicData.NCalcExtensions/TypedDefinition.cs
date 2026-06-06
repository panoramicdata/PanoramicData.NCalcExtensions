namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Represents a typed definition parsed from expression comments.
/// Supports absent values, explicit null values, and literal values.
/// </summary>
public sealed record TypedDefinition
{
	/// <summary>
	/// The type name as written in the source definition.
	/// </summary>
	public required string TypeName { get; init; }

	/// <summary>
	/// Whether the definition includes a value segment.
	/// </summary>
	public required bool HasValue { get; init; }

	/// <summary>
	/// Whether the definition explicitly specifies a null value.
	/// </summary>
	public required bool IsNull { get; init; }

	/// <summary>
	/// The literal value text when a non-null value was specified.
	/// </summary>
	public string? Value { get; init; }

	/// <summary>
	/// True when a non-null literal value is present.
	/// </summary>
	public bool HasLiteralValue => HasValue && !IsNull;

	/// <summary>
	/// Create a definition with an explicit non-null literal value.
	/// </summary>
	public static TypedDefinition FromLiteral(string typeName, string value) => new()
	{
		TypeName = typeName,
		HasValue = true,
		IsNull = false,
		Value = value
	};

	/// <summary>
	/// Create a definition with an explicit null value.
	/// </summary>
	public static TypedDefinition FromNull(string typeName) => new()
	{
		TypeName = typeName,
		HasValue = true,
		IsNull = true,
		Value = null
	};

	/// <summary>
	/// Create a definition that only declares a type.
	/// </summary>
	public static TypedDefinition FromTypeOnly(string typeName) => new()
	{
		TypeName = typeName,
		HasValue = false,
		IsNull = false,
		Value = null
	};
}