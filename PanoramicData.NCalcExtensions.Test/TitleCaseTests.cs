namespace PanoramicData.NCalcExtensions.Test;

public class TitleCaseTests : NCalcTest
{
   [Theory]
   [InlineData("new year", "New Year")]
   [InlineData("hello world", "Hello World")]
   [InlineData("the quick brown fox", "The Quick Brown Fox")]
   [InlineData("already Title Case", "Already Title Case")]
   [InlineData("ALL CAPS INPUT", "All Caps Input")]
   [InlineData("single", "Single")]
   [InlineData("", "")]
   [InlineData("a b c", "A B C")]
   public void TitleCase_UsingInlineData_MatchesExpectedResult(string input, string expected)
   {
      var expression = new ExtendedExpression($"titleCase('{input}')");

      var result = expression.Evaluate();

      result.Should().Be(expected);
   }

   [Fact]
   public void TitleCase_NullParameter_ThrowsException()
   {
      var expression = new ExtendedExpression("titleCase(null)");
      expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
   }

   [Fact]
   public void TitleCase_NonStringParameter_ThrowsException()
   {
      var expression = new ExtendedExpression("titleCase(123)");
      expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
   }
}
