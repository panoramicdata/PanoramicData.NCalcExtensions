using System.Linq;
using NCalc;
using PanoramicData.NCalcExtensions.Extensions;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests for the Parameters helper class.
/// Note: This class contains helper methods designed to simplify parameter extraction,
/// but most methods are currently unused in the codebase. Only CheckParameterCount is actively used.
/// </summary>
public class ParametersHelperTests
{
	#region CheckParameterCount Tests - The only actively used method

	[Fact]
	public void CheckParameterCount_ExactCount_DoesNotThrow()
	{
		// Use ExtendedExpression which inherits from Expression
		var param1 = new ExtendedExpression("1");
		var param2 = new ExtendedExpression("2");
		Expression[] parameters = [param1, param2];
		
		var act = () => Parameters.CheckParameterCount(2, 2, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_BelowMinimum_ThrowsFormatException()
	{
		var param1 = new ExtendedExpression("1");
		Expression[] parameters = [param1];
		
		var act = () => Parameters.CheckParameterCount(2, 5, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at least 2 parameters.");
	}

	[Fact]
	public void CheckParameterCount_AboveMaximum_ThrowsFormatException()
	{
		Expression[] parameters = 
		[
			new ExtendedExpression("1"),
			new ExtendedExpression("2"),
			new ExtendedExpression("3"),
			new ExtendedExpression("4")
		];
		
		var act = () => Parameters.CheckParameterCount(1, 3, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at most 3 parameters.");
	}

	[Fact]
	public void CheckParameterCount_NoMinimum_OnlyChecksMaximum()
	{
		var parameters = Array.Empty<Expression>();
		
		var act = () => Parameters.CheckParameterCount(null, 2, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_NoMaximum_OnlyChecksMinimum()
	{
		Expression[] parameters = 
		[
			new ExtendedExpression("1"),
			new ExtendedExpression("2"),
			new ExtendedExpression("3"),
			new ExtendedExpression("4"),
			new ExtendedExpression("5")
		];
		
		var act = () => Parameters.CheckParameterCount(2, null, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_NoBounds_AlwaysPasses()
	{
		Expression[] parameters = [new ExtendedExpression("1")];
		
		var act = () => Parameters.CheckParameterCount(null, null, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_ExactlyMinimum_DoesNotThrow()
	{
		Expression[] parameters = [new ExtendedExpression("1"), new ExtendedExpression("2")];
		
		var act = () => Parameters.CheckParameterCount(2, 5, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_ExactlyMaximum_DoesNotThrow()
	{
		Expression[] parameters = 
		[
			new ExtendedExpression("1"),
			new ExtendedExpression("2"),
			new ExtendedExpression("3")
		];
		
		var act = () => Parameters.CheckParameterCount(1, 3, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_EmptyArray_BelowMinimum_Throws()
	{
		var parameters = Array.Empty<Expression>();
		
		var act = () => Parameters.CheckParameterCount(1, 5, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at least 1 parameters.");
	}

	[Fact]
	public void CheckParameterCount_MultipleViolations_ThrowsMinimumError()
	{
		// When both constraints are violated, minimum check happens first
		Expression[] parameters = [new ExtendedExpression("1")];
		
		var act = () => Parameters.CheckParameterCount(5, 3, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at least 5 parameters.");
	}

	#endregion

	#region CallerMemberName Tests

	[Fact]
	public void CheckParameterCount_UsesCallerMemberName()
	{
		var parameters = Array.Empty<Expression>();
		
		var act = () => TestMethodForCallerName(parameters);
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestMethodForCallerName requires at least 1 parameters.");
	}

	private static void TestMethodForCallerName(Expression[] parameters)
	{
		Parameters.CheckParameterCount(1, 5, parameters);
	}

	[Fact]
	public void CheckParameterCount_ExplicitCallerName_UsesProvidedName()
	{
		var parameters = Array.Empty<Expression>();
		
		var act = () => Parameters.CheckParameterCount(1, 5, parameters, "MyCustomFunction");
		
		act.Should().Throw<FormatException>()
			.WithMessage("MyCustomFunction requires at least 1 parameters.");
	}

	#endregion

	#region Boundary Tests

	[Fact]
	public void CheckParameterCount_LargeParameterCount_Works()
	{
		var parameters = Enumerable.Range(0, 100).Select(i => new ExtendedExpression(i.ToString(System.Globalization.CultureInfo.InvariantCulture)) as Expression).ToArray();
		
		var act = () => Parameters.CheckParameterCount(50, 150, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_ZeroMinimum_ValidatesCorrectly()
	{
		var parameters = Array.Empty<Expression>();
		
		var act = () => Parameters.CheckParameterCount(0, 5, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_SameMinAndMax_Works()
	{
		Expression[] parameters = [new ExtendedExpression("1"), new ExtendedExpression("2"), new ExtendedExpression("3")];
		
		var act = () => Parameters.CheckParameterCount(3, 3, parameters, "TestFunc");
		
		act.Should().NotThrow();
	}

	[Fact]
	public void CheckParameterCount_SameMinAndMax_TooFew_Throws()
	{
		Expression[] parameters = [new ExtendedExpression("1"), new ExtendedExpression("2")];
		
		var act = () => Parameters.CheckParameterCount(3, 3, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at least 3 parameters.");
	}

	[Fact]
	public void CheckParameterCount_SameMinAndMax_TooMany_Throws()
	{
		Expression[] parameters = 
		[
			new ExtendedExpression("1"),
			new ExtendedExpression("2"),
			new ExtendedExpression("3"),
			new ExtendedExpression("4")
		];
		
		var act = () => Parameters.CheckParameterCount(3, 3, parameters, "TestFunc");
		
		act.Should().Throw<FormatException>()
			.WithMessage("TestFunc requires at most 3 parameters.");
	}

	#endregion

	#region Documentation Tests for Unused Methods

	[Fact]
	public void GetParameter_MethodExists_Documentation()
	{
		// This test documents that GetParameter<T> exists but is currently unused in the codebase.
		// The method is designed to extract and validate a single typed parameter from FunctionArgs,
		// but all current functions implement their own parameter extraction logic.
		
		var methodExists = typeof(Parameters)
			.GetMethod("GetParameter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		
		methodExists.Should().NotBeNull("GetParameter<T> method should exist for potential future use");
		methodExists!.IsGenericMethod.Should().BeTrue("GetParameter should be a generic method");
		
		var parameters = methodExists.GetParameters();
		parameters.Should().HaveCount(2, "GetParameter should have FunctionArgs and callerName parameters");
	}

	[Fact]
	public void GetParameters_MethodsExist_Documentation()
	{
		// This test documents that GetParameters<T1,T2...> overloads exist but are currently unused.
		// These methods were designed to simplify extraction of multiple typed parameters,
		// but the codebase uses manual parameter extraction instead.
		
		var methods = typeof(Parameters)
			.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
			.Where(m => m.Name == "GetParameters")
			.ToList();
		
		methods.Should().NotBeEmpty("GetParameters<T1,T2...> methods should exist for potential future use");
		methods.Should().HaveCount(6, "Should have overloads for 2 through 7 parameters");
		methods.Should().OnlyContain(m => m.IsGenericMethod, "All GetParameters methods should be generic");
	}

	[Fact]
	public void GetParameters_OverloadsHaveCorrectGenericCount_Documentation()
	{
		// Validates that the GetParameters overloads match their intended generic parameter counts
		var methods = typeof(Parameters)
			.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
			.Where(m => m.Name == "GetParameters")
			.ToList();
		
		// Should have overloads for 2, 3, 4, 5, 6, and 7 type parameters
		var genericCounts = methods.Select(m => m.GetGenericArguments().Length).OrderBy(x => x).ToList();
		genericCounts.Should().BeEquivalentTo([2, 3, 4, 5, 6, 7]);
	}

	[Fact]
	public void GetParameter_HasCorrectSignature_Documentation()
	{
		// Documents the expected signature of GetParameter for future implementers
		var method = typeof(Parameters)
			.GetMethod("GetParameter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		
		method.Should().NotBeNull();
		var parameters = method!.GetParameters();
		parameters.Should().HaveCount(2, "GetParameter should have 2 parameters: FunctionArgs and callerName");
		parameters[0].ParameterType.Name.Should().Be("FunctionArgs");
		parameters[1].ParameterType.Should().Be<string>();
		parameters[1].HasDefaultValue.Should().BeTrue("callerName should have a default value");
	}

	[Fact]
	public void GetParameters_AllReturnTuples_Documentation()
	{
		// Documents that all GetParameters overloads return Tuple types
		var methods = typeof(Parameters)
			.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
			.Where(m => m.Name == "GetParameters")
			.ToList();
		
		foreach (var method in methods)
		{
			// Generic methods don't have a concrete return type until invoked,
			// but we can verify the method declaration structure
			method.ReturnType.Name.Should().StartWith("Tuple", 
				$"GetParameters<{method.GetGenericArguments().Length}> should return a Tuple");
		}
	}

	#endregion
}
