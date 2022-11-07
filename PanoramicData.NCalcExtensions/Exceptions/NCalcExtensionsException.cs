namespace PanoramicData.NCalcExtensions.Exceptions;

public class NCalcExtensionsException : Exception
{
	public NCalcExtensionsException()
	{
	}

	public NCalcExtensionsException(string message) : base(message)
	{
	}

	public NCalcExtensionsException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
