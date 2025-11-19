using PanoramicData.NCalcExtensions.Exceptions;

namespace PanoramicData.NCalcExtensions.Test;

public class NCalcExtensionsExceptionTests
{
	[Fact]
	public void NCalcExtensionsException_DefaultConstructor_CreatesException()
	{
		var exception = new NCalcExtensionsException();
		exception.Should().NotBeNull();
		exception.Message.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void NCalcExtensionsException_MessageConstructor_CreatesExceptionWithMessage()
	{
		var message = "Test error message";
		var exception = new NCalcExtensionsException(message);
		exception.Should().NotBeNull();
		exception.Message.Should().Be(message);
	}

	[Fact]
	public void NCalcExtensionsException_InnerExceptionConstructor_CreatesExceptionWithInnerException()
	{
		var message = "Outer exception";
		var innerException = new InvalidOperationException("Inner exception");
		var exception = new NCalcExtensionsException(message, innerException);
		
		exception.Should().NotBeNull();
		exception.Message.Should().Be(message);
		exception.InnerException.Should().Be(innerException);
		exception.InnerException.Message.Should().Be("Inner exception");
	}

	[Fact]
	public void NCalcExtensionsException_IsException_CanBeThrown()
	{
		Action throwAction = () => throw new NCalcExtensionsException("Test throw");
		throwAction.Should().Throw<NCalcExtensionsException>()
			.WithMessage("Test throw");
	}

	[Fact]
	public void NCalcExtensionsException_IsCaughtAsException_CanBeCaughtAsBaseException()
	{
		Action throwAction = () => throw new NCalcExtensionsException("Test throw");
		throwAction.Should().Throw<Exception>();
	}
}
