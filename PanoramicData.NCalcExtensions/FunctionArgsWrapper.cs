namespace PanoramicData.NCalcExtensions;

internal class FunctionArgsWrapper : IFunctionArgs
{
	readonly FunctionArgs _args;

	public FunctionArgsWrapper(FunctionArgs args)
	{
		_args = args;
	}

	public object? Result { get => _args.Result; set => _args.Result = value; }
	public IExpression[] Parameters { get => _args.Parameters.Select(a => new ExpressionWrapper(a)).ToArray(); }
}
