using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
   [DisplayName("titleCase")]
   [Description("Converts a string to title case, capitalizing the first letter of each word.")]
   string TitleCase(
      [Description("The text to convert to title case.")]
      string text
   );
}

internal static class TitleCaseFunction
{
   private static readonly TextInfo TextInfo = CultureInfo.InvariantCulture.TextInfo;

   internal static void Evaluate(FunctionArgs functionArgs)
   {
      try
      {
         var param1 = functionArgs.Parameters[0].Evaluate() as string
            ?? throw new FormatException($"{ExtensionFunction.TitleCase} function - requires one string parameter.");

         functionArgs.Result = TextInfo.ToTitleCase(param1.ToLowerInvariant());
      }
      catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
      {
         throw new FormatException($"{ExtensionFunction.TitleCase} function - requires one string parameter.");
      }
   }
}
