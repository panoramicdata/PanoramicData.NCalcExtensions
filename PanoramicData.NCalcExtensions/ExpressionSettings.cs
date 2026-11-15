namespace PanoramicData.NCalcExtensions;

/// <summary>The options and culture of an expression, carried to the lambdas it creates.</summary>
internal sealed record ExpressionSettings(ExpressionOptions Options, CultureInfo CultureInfo)
{
	internal static ExpressionSettings From(IDictionary<string, object?> parameters)
		=> parameters.TryGetValue(BaseExtendedExpression.SettingsParameterName, out var value) && value is ExpressionSettings settings
			? settings
			: new ExpressionSettings(BaseExtendedExpression.ExtendedExpressionDefaults, CultureInfo.InvariantCulture);
}
