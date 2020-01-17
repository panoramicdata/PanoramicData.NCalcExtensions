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

General functions:

- [cast()](#cast)
- [dateTime()](#dateTime)
- [dateTimeAsEpochMs()](#dateTimeAsEpochMs)
- [format()](#format)
- [if()](#if)
- [in()](#in)
- [isInfinite()](#isinfinite)
- [isNaN()](#isnan)
- [jPath()](#jPath)
- [regexGroup()](#regexGroup)
- [regexIsMatch()](#regexIsMatch)
- [switch()](#switch)
- [throw()](#throw)
- [timeSpan()](#timespan)
- [toDateTime()](#toDateTime)

String functions:
- [capitalize()](#capitalize)
- [contains()](#contains)
- [endsWith()](#endswith)
- [indexOf()](#indexof)
- [lastIndexOf()](#lastindexof)
- [length()](#length)
- [replace()](#replace)
- [startsWith()](#startswith)
- [substring()](#substring)
- [toLower()](#tolower)
- [toUpper()](#toupper)

Supported functions:

---
# cast()

## Purpose
Cast an object to another (e.g. float to decimal).
The method requires that conversion of value to target type be supported.

## Parameters
- inputObject
- typeString

## Examples
- cast(0.3, 'System.Decimal')

---
# dateTime()

## Purpose
Return the DateTime in the specified format as a string, with an optional offset.

## Parameters
- timeZone (only 'UTC' currently supported)
- format
- day offset
- hour offset
- minute offset
- second offset

## Examples
- dateTime('UTC', 'yyyy-MM-dd HH:mm:ss', -90, 0, 0, 0) : 90 days ago (e.g. '2019-03-14 05:09')
- dateTime('UTC', 'yyyy-MM-dd HH:mm:ss') : now (e.g. '2019-03-14 05:09')

---
# dateTimeAsEpochMs()

## Purpose
Parses the input DateTime and outputs as milliseconds since the Epoch (1st Jan 1970).

## Parameters
- input date string
- format

## Examples
- dateTimeAsEpochMs('20190702T000000', 'yyyyMMddTHHmmssK') : 1562025600000

---
# format()

## Purpose
Formats strings and numbers as output strings with the specified format

## Parameters
- object (number or text)
- format

## Examples
- format(1, '00') : '01'
- format(1.0, '00') : '01'
- format('01/01/2019', 'yyyy-MM-dd') : '2019-01-01'

---
# dateTimeAsEpochMs()

## Purpose
Parses the input DateTime and outputs as milliseconds since the Epoch (1st Jan 1970).

## Parameters
- input date string
- format

## Examples
- dateTimeAsEpochMs('20190702T000000', 'yyyyMMddTHHmmssK') : 1562025600000

---
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

---
# in()

## Purpose

Determines whether a value is in a set of other values.

## Parameters
- list
- item

## Examples
- in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') : true
- in('needle', 'haystack', 'with', 'only', 'hay') : false

---
# isInfinite()

## Purpose

Determines whether a value is infinite

## Parameters
- value

## Examples
- isInfinite(1/0) : true
- isInfinite(0/1) : false

---
# isNaN()

## Purpose

Determines whether a value is not a number.

## Parameters
- value

## Examples
- isNaN(null) : true
- isNaN(1) : false
---

# isNull()

## Purpose

Determines whether a value is null.

## Parameters
- value

## Examples
- isNull(1) : false
- isNull('text') : false
- isNull(bob) : true is bob is null
- isNull(null) : true

---
# in()

## Purpose

Determines whether a value is in a set of other values.

## Parameters
- list
- item

## Examples
- in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') : true
- in('needle', 'haystack', 'with', 'only', 'hay') : false

---
# indexOf()

## Purpose

Determines the first position of a string within another string.  Returns -1 if not present.

## Parameters
- longString
- shortString

## Examples
- firstIndexOf('#abcabc#', 'abc') : 1
- firstIndexOf('#abcabc#', 'abcd') : -1

---
# lastIndexOf()

## Purpose

Determines the last position of a string within another string.  Returns -1 if not present.

## Parameters
- longString
- shortString

## Examples
- lastIndexOf('#abcabc#', 'abc') : 4
- lastIndexOf('#abcabc#', 'abcd') : -1

---
# length()

## Purpose

Determines length of a string.

## Parameters
- string

## Examples
- length('a piece of string') : 17

---
# startsWith()

## Purpose

Determines whether a string starts with another string.

## Parameters
- longString
- shortString

## Examples
- startsWith('abcdefg', 'ab') : true
- startsWith('abcdefg', 'cd') : false

---
# endsWith()

## Purpose

Determines whether a string ends with another string.

## Parameters
- longString
- shortString

## Examples
- endsWith('abcdefg', 'fg') : true
- endsWith('abcdefg', 'fgh') : false

---

# jPath()

## Purpose

Selects a single value from a JObject using a [JPath](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) expression

## Parameters
- input JObject
- JPath string expression

## Examples
sourceJObject JSON:
```
{
  "name": "bob",
  "numbers": [ 1, 2 ]
  "arrayList": [ 
    { "key": "key1", "value": "value1" },
    { "key": "key2", "value": "value2" } 
  ]
}
```
- jPath(sourceJObject, 'name') : 'bob'
- jPath(sourceJObject, 'size') : an exception is thrown
- jPath(sourceJObject, 'size', True) : null is returned
- jPath(sourceJObject, 'numbers[0]') : 1
- jPath(sourceJObject, 'arrayList[?(@key==\\'key1\\')]') : "value1"

---
# regexGroup()

## Purpose

Selects a regex group capture

## Parameters
- input
- regex
- zero-based capture index (default: 0)

## Examples
- regexGroup('abcdef', '^ab(.+?)f$') : 'cde'
- regexGroup('abcdef', '^ab(.)+f$') : 'c'
- regexGroup('abcdef', '^ab(.)+f$', 1) : 'd'
- regexGroup('abcdef', '^ab(.)+f$', 2) : 'e'
- regexGroup('abcdef', '^ab(.)+f$', 10) : null

---
# regexIsMatch()

## Purpose

Determine whether a string matches a regex

## Parameters
- input
- regex

## Examples
- regexIsMatch('abcdef', '^ab.+') : true
- regexIsMatch('Zbcdef', '^ab.+') : false

---
# replace()

## Purpose

Replace a string with another string

## Parameters
- haystackString
- needleString
- betterNeedleString

## Examples
- replace('abcdefg', 'cde', 'CDE') : 'abCDEfg'
- replace('abcdefg', 'cde', '') : 'abfg'

---
# substring()

## Purpose

Retrieves part of a string.

## Parameters
- inputString
- startIndex
- length (optional)

## Examples
- substring('haystack', 3) : 'stack'
- substring('haystack', 0, 3) : 'hay'

---
# switch()

## Purpose
Return one of a number of values, depending on the input function.

## Parameters
- switched value
- a set of pairs: case_n, output_n
- if present, a final value can be used as a default.  If the default WOULD have been returned, but no default is present, an exception is thrown.

## Examples
- switch('yes', 'yes', 1, 'no', 2) : 1
- switch('blah', 'yes', 1, 'no', 2) : throws exception
- switch('blah', 'yes', 1, 'no', 2, 3) : 3

---
# toDateTime()

## Purpose

Converts a string to a DateTime

## Parameters
- inputString
- stringFormat

## Examples
- toDateTime('2019-01-01', 'yyyy-MM-dd') : The date

---
# throw()

## Purpose

Throws an NCalcExtensionsException.   Useful in an if().

## Parameters
- message (optional)

## Examples
- throw()
- throw('This is a message')
- if(problem, throw('There is a problem'), 5)

---
# timeSpan()

## Purpose

Determines the amount of time between two DateTimes.

## Parameters
- startDateTime
- endDateTime
- timeUnit

## Examples
- in('2019-01-01 00:01:00', '2019-01-01 00:02:00', 'seconds') : 3600

---
# toLower()

## Purpose

Converts a string to lower case.

## Parameters
- string

## Examples
- toLower('PaNToMIMe') : 'pantomime'

---
# toUpper()

## Purpose

Converts a string to upper case.

## Parameters
- string

## Examples
- toUpper('PaNToMIMe') : 'PANTOMIME'

---
# capitalize()

## Purpose

Capitalizes a string.

## Parameters
- string

## Examples
- capitalize('new year') : 'New Year'

---
# humanize()

## Purpose

Humanizes the value text.

## Parameters
- value
- timeUnit

## Examples
- humanize(3600, 'seconds') : '1 hour'

