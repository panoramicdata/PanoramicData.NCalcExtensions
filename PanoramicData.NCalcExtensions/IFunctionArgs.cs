namespace PanoramicData.NCalcExtensions;

internal interface IFunctionArgs
{
	IExpression[] Parameters { get; }
	object? Result { get; set; }
}