namespace PanoramicData.NCalcExtensions;

internal class AsyncFunctionArgsWrapper : IFunctionArgs
{
	readonly AsyncFunctionArgs _args;

	public AsyncFunctionArgsWrapper(AsyncFunctionArgs args)
	{
		_args = args;
	}

	public object? Result { get => _args.Result; set => _args.Result = value; }
	public IExpression[] Parameters { get => _args.Parameters.Select(a => new AsyncExpressionWrapper(a)).ToArray(); }
}
