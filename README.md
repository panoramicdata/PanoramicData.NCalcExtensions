# PanoramicData.NCalcExtensions

[![Nuget](https://img.shields.io/nuget/v/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
[![Nuget](https://img.shields.io/nuget/dt/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5b0ad600b19d42e2b735e4199b795fa2)](https://www.codacy.com/gh/panoramicdata/PanoramicData.NCalcExtensions/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=panoramicdata/PanoramicData.NCalcExtensions&amp;utm_campaign=Badge_Grade)

## Introduction

This nuget package provides extension functions for NCalc.

The NCalc documentation can be found [here (source code)](https://github.com/ncalc/ncalc) and [here (good explanation of built-in functions)](https://github.com/pitermarx/NCalc-Edge/wiki/Functions).

## Release Process

Use [RELEASE_NOTES.md](RELEASE_NOTES.md) to maintain upcoming changes under `## Unreleased`.

- Run `Publish.ps1` when the next publish should use the version currently reported by NBGV and no extra release-notes commit is needed.
- Run `PublishWithReleaseNote.ps1` when you want to finalize `RELEASE_NOTES.md` first. It stamps the current `## Unreleased` section with the next package version and date, creates a fresh `## Unreleased` section, commits that change, pushes `main`, verifies the NBGV increment, and then calls `Publish.ps1`.
- `PublishWithReleaseNote.ps1` expects `main`, fetches `origin/main`, and only allows a clean working tree or changes limited to [RELEASE_NOTES.md](RELEASE_NOTES.md).


## Additional benefits

When using ExtendedExpression, you can:
- use "//" comments
- use "/* */" multi-line comments for documentation
- split expressions over multiple lines
- include empty lines
- indent lines
- define typed parameters and answers directly in comments using formats like: `// parameterName:type:value`, `// parameterName:type`, and `// answer:type:value`

For example:

````NCalc
// This calculates the presence of a needle in a haystack
contains(

	// Complicated haystack creation
	'haystack ' + 'containing needle',

	// The needle we are looking for
	'needle'

)
````

### Parameter Definitions in Comments

You can provide typed parameter definitions in comment lines using either of these formats:
`// parameterName:type:value`
`// parameterName:type`

The legacy `// parameterName:TypeName:value` format is still supported.

You can also provide a typed answer using:
`// answer:type:value`
`// answer:type`

Examples:
- `// count:int:5`
- `// customerName:string:Ada`
- `// optionalName:string?:null`
- `// answer:string?`

`null` is reserved as the explicit null marker for typed definitions. A literal string value of `"null"` is not supported in this format.

Multi-line documentation blocks (if used) should use markdown format:

````NCalc
// theAnswer:int:42
// theAnswer2:int:2
// theAnswer3:string:Blah
// theAnswer4:int:23
// answer:int:44

/*
# The answer plus one plus one
This adds one twice to theAnswer, which is a 32-bit integer in this example.
*/

theAnswer + 1 + 1
````

**Supported simple types:**
- `string`, `string?`
- `bool`, `bool?`
- `byte`, `byte?`
- `sbyte`, `sbyte?`
- `short`, `short?`
- `ushort`, `ushort?`
- `int`, `int?`
- `uint`, `uint?`
- `long`, `long?`
- `ulong`, `ulong?`
- `float`, `float?`
- `double`, `double?`
- `decimal`, `decimal?`
- `DateTime`, `DateTime?`
- `Guid`, `Guid?`

Equivalent CLR type names such as `System.Int32` and `System.String?` are also supported.

**Example:**
```csharp
using PanoramicData.NCalcExtensions;

var expression = new ExtendedExpression("""
// count:int:5
// multiplier:decimal:2.5
// answer:decimal:12.5

/*
# Calculate total amount
Multiplies the count by the multiplier value.
*/

count * multiplier
""");

var result = expression.Evaluate(); // Returns: 12.5 (5 * 2.5)
```

### ExtendedExpression Property Accessors

`ExtendedExpression` parses the input once at construction time and exposes the results as properties:

```csharp
var expression = new ExtendedExpression("""
// x:int:10
// answer:int:11

/*
# My expression
Adds one to x.
*/

x + 1
""");

// Access parsed metadata
string? docs = expression.Documentation;   // "# My expression\nAdds one to x."
var parameters = expression.ParameterDefinitions; // { "x": TypedDefinition.FromLiteral("int", "10") }
var answer = expression.AnswerDefinition;  // TypedDefinition.FromLiteral("int", "11")
var hasExpectedAnswer = expression.HasExpectedAnswer; // true
var hasExpectedAnswerValue = expression.HasExpectedAnswerValue; // true
var expectedAnswer = expression.ExpectedAnswer; // 11
var comments = expression.Comments;        // any non-parameter // comment lines
var doc = expression.Document;             // full ExtendedExpressionDocument

// Access a cached SimpleExtendedExpression for repeated evaluation
var simple = expression.SimpleExtendedExpression; // lazy, same options/culture
var result = simple.Evaluate(); // 11
```

The `SimpleExtendedExpression` accessor is created lazily on first access and reuses the same parsed document, so there is no double-parsing cost.

## Parsing and Performance Optimization

For advanced use cases where you need to:
1. Extract documentation and parameters separately
2. Reuse the same expression multiple times
3. Cache tidied expressions for better performance

Use `ExtendedExpressionDocumentParser` to parse the expression once, and then use `SimpleExtendedExpression` for evaluation:

```csharp
using PanoramicData.NCalcExtensions;

// Step 1: Parse once (including documentation and parameter extraction)
var multilineExpression = """
// x:int:10
// y:int:5

/*
# Add two numbers
This demonstrates adding two pre-defined parameters.
*/

x + y
""";

var document = ExtendedExpressionDocumentParser.Parse(multilineExpression);

// Step 2: Access parsed information
Console.WriteLine("Documentation: " + document.Documentation);
Console.WriteLine("Parameters: " + string.Join(", ", document.Parameters.Keys));

// Step 3: Create a SimpleExtendedExpression for evaluation
// (no re-parsing overhead)
var expression = new SimpleExtendedExpression(document.TidiedExpression, document);
var result = expression.Evaluate(); // Returns: 15
```

### ExtendedExpressionDocument Structure

The parsed document contains:
- **OriginalExpression**: The original multi-line input
- **TidiedExpression**: The cleaned expression ready for evaluation
- **Documentation**: Extracted from `/* */` comments (markdown format)
- **Parameters**: Dictionary of typed definitions from `// name:type` or `// name:type:value` lines
- **Answer**: Optional typed definition from `// answer:type` or `// answer:type:value`
- **Comments**: Regular comment lines (not parameter definitions)

### SimpleExtendedExpression

Use `SimpleExtendedExpression` for performance-critical scenarios where you:
- Have already parsed the expression with `ExtendedExpressionDocumentParser`
- Need to evaluate the same expression multiple times
- Want to avoid the overhead of re-parsing

The main difference from `ExtendedExpression`:
- Takes a pre-tidied expression (no parsing/tidying overhead)
- Can be created from an `ExtendedExpressionDocument` for automatic parameter setup
- Still supports all extension functions
- Lightweight and optimized for repeated evaluations

**Input contract**: `SimpleExtendedExpression` requires a pre-tidied expression. The constructor throws `FormatException` if:
- The expression contains newline characters (`\n` or `\r`) — use `ExtendedExpression` or `ExtendedExpressionDocumentParser` to tidy multi-line input first
- The expression (after trimming whitespace) starts with `//` or `/*` — comment markers must be stripped before constructing a `SimpleExtendedExpression`

## Breaking Changes in v5.8+

Starting with version 5.8, this library has migrated from `CoreCLR-NCalc` to the **`NCalcSync`** package.
This brings improved performance, better maintainability, and bug fixes, but requires awareness when upgrading.
Version numbers align with the NCalcSync package for clarity.

### What Changed

1. **NuGet Package Dependency**: The underlying NCalc implementation changed from `CoreCLR-NCalc` to `NCalcSync`
2. **Target Framework**: Now targets .NET 9 only (previously supported .NET Standard 2.0)
3. **Constructor Options**: `ExtendedExpression` now provides two constructors for different use cases

### Migration Guide

#### Basic Usage (No Changes Required)

For most users, **no code changes are required**. The default constructor continues to work with sensible defaults:

```csharp
using PanoramicData.NCalcExtensions;

var expression = new ExtendedExpression("1 + 1");
var result = expression.Evaluate(); // Returns: 2
```

The default constructor automatically applies these `ExpressionOptions`:
- `ExpressionOptions.NoCache` - Expressions are not cached (safer for dynamic expressions)
- `ExpressionOptions.NoStringTypeCoercion` - Prevents automatic string-to-number conversions
- `ExpressionOptions.StrictTypeMatching` - Enforces type safety (e.g., `1 != '1'`)

#### Advanced Usage (Custom Options)

If you need custom `ExpressionOptions`, use the overloaded constructor:

```csharp
using PanoramicData.NCalcExtensions;
using NCalc;
using System.Globalization;

var expression = new ExtendedExpression(
    "1 + 1",
    ExpressionOptions.None, // Or combine options with |
    CultureInfo.InvariantCulture
);
var result = expression.Evaluate();
```

#### Understanding ExpressionOptions

The `ExpressionOptions` enum from NCalcSync provides fine-grained control over expression evaluation:

- **`None`**: Default NCalc behavior (allows caching, type coercion, etc.)
- **`NoCache`**: Disables expression caching (recommended for dynamic expressions)
- **`NoStringTypeCoercion`**: Prevents implicit string-to-number conversions
- **`StrictTypeMatching`**: Enforces strict type comparisons
- **`IgnoreCase`**: Makes string comparisons case-insensitive
- **`AllowNullParameter`**: Allows null parameter values
- **`IterateParameters`**: Enables parameter iteration in expressions

For complete documentation on `ExpressionOptions`, see the [NCalcSync documentation](https://github.com/ncalc/ncalc/wiki).

### Why This Change?

The `CoreCLR-NCalc` package is no longer actively maintained. Migrating to `NCalcSync` provides:
- Active maintenance and bug fixes
- Better performance  
- Improved standard compliance
- Enhanced type safety options

## Documentation

The generated documentation index is now on the dedicated page:

- [Documentation/README.md](Documentation/README.md)
- [Function documentation folder](Documentation/)

That page contains the per-function indexes and the example links to NCalc101.

## Usage

````C#
using PanoramicData.NCalcExtensions;

...
var calculation = "lastIndexOf('abcdefg', 'def')";
var nCalcExpression = new ExtendedExpression(calculation);

if (nCalcExpression.HasErrors())
{
	throw new FormatException($"Could not evaluate expression: '{calculation}' due to {nCalcExpression.Error}.");
}

return nCalcExpression.Evaluate();
````
## Namespace safety

Note that we regularly extend this library to add new functions.
For those that *further* extend the functions, it is important to avoid future potential namespace conflicts.
For this reason, we guarantee that we will never use the underscore character in any future function name and we recommend that any functions that you add DO include an underscore.  For example, if you had a project called "My Project", we suggest that you name your functions as follows:

* mp_myFunction1()
* mp_myFunction2()

## Contributing

Pull requests are welcome!
Please submit your requests for new functions in the form of pull requests.

## Function documentation

Detailed per-function documentation is maintained in [Documentation/README.md](Documentation/README.md) and generated pages under [Documentation/](Documentation/).