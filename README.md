# PanoramicData.NCalcExtensions

Extension functions for NCalc.

Please submit your requests for new functions in the form of pull requests.

To use:

````C#
using PanoramicData.NCalcExtensions;

...
var calculation = "lastIndexOf('abcdefg', 'def')";
var nCalcExpression = new Expression(calculation);

// Add the extension functions
nCalcExpression.EvaluateFunction += NCalcExtensions.NCalcExtensions.Extend;

if (nCalcExpression.HasErrors())
{
	throw new FormatException($"Could not evaluate expression: '{calculation}' due to {nCalcExpression.Error}.");
}

return nCalcExpression.Evaluate();
````

Supported functions:

# if()

## Purpose
Return one of two values, depending on the input function.

## Parameters
- condition
- output if true
- output if false

## Examples
- if(1 == 1, 'yes', 'no') : 'yes'
- if(1 == 2, 3, 4) : 4

# in()

## Purpose

Determines whether a value is in a set of other values.

## Parameters
- list
- item

## Examples
- in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') : true
- in('needle', 'haystack', 'with', 'only', 'hay') : false

# isInfinite()

## Purpose

Determines whether a value is infinite

## Parameters
- value

## Examples
- isInfinite(1/0) : true
- isInfinite(0/1) : false

# isNaN()

## Purpose

Determines whether a value is not a number.

## Parameters
- value

## Examples
- isNaN(null) : true
- isNaN(1) : false

# contains()

## Purpose

Determines whether a value is in a set of other values.

## Parameters
- list
- item

## Examples
- in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') : true
- in('needle', 'haystack', 'with', 'only', 'hay') : false

# indexOf()

## Purpose

Determines the first position of a string within another string.  Returns -1 if not present.

## Parameters
- longString
- shortString

## Examples
- firstIndexOf('#abcabc#', 'abc') : 1
- firstIndexOf('#abcabc#', 'abcd') : -1

# lastIndexOf()

## Purpose

Determines the last position of a string within another string.  Returns -1 if not present.

## Parameters
- longString
- shortString

## Examples
- lastIndexOf('#abcabc#', 'abc') : 4
- lastIndexOf('#abcabc#', 'abcd') : -1

# length()

## Purpose

Determines length of a string.

## Parameters
- string

## Examples
- length('a piece of string') : 17

# startsWith()

## Purpose

Determines whether a string starts with another string.

## Parameters
- longString
- shortString

## Examples
- endsWith('abcdefg', 'ab') : true
- endsWith('abcdefg', 'cd') : false

# endsWith()

## Purpose

Determines whether a string ends with another string.

## Parameters
- longString
- shortString

## Examples
- endsWith('abcdefg', 'fg') : true
- endsWith('abcdefg', 'fgh') : false

# timeSpan()

## Purpose

Determines the amount of time between two DateTimes.

## Parameters
- startDateTime
- endDateTime
- timeUnit

## Examples
- in('2019-01-01 00:01:00', '2019-01-01 00:02:00', 'seconds') : 3600

# toLower()

## Purpose

Converts a string to lower case.

## Parameters
- string

## Examples
- toLower('PaNToMIMe') : 'pantomime'

# toUpper()

## Purpose

Converts a string to upper case.

## Parameters
- string

## Examples
- toUpper('PaNToMIMe') : 'PANTOMIME'

# capitalize()

## Purpose

Capitalizes a string.

## Parameters
- string

## Examples
- capitalize('new year') : 'New Year'

# humanize()

## Purpose

Humanizes the value text.

## Parameters
- value
- timeUnit

## Examples
- humanize(3600, 'seconds') : '1 hour'

