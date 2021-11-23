namespace PanoramicData.NCalcExtensions.Exceptions;

public class NCalcExtensionsException : Exception
{
	public NCalcExtensionsException() : base()
	{
	}

	public NCalcExtensionsException(string message) : base(message)
	{
	}

	public NCalcExtensionsException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
