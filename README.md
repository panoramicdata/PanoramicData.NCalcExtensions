# PanoramicData.NCalcExtensions

Extension functions for NCalc.

[![Nuget](https://img.shields.io/nuget/v/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)

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

- [canEvaluate()](#canEvaluate)
- [cast()](#cast)
- [changeTimeZone()](#changeTimeZone)
- [dateTime()](#dateTime)
- [dateTimeAsEpochMs()](#dateTimeAsEpochMs)
- [format()](#format)
- [if()](#if)
- [in()](#in)
- [isInfinite()](#isinfinite)
- [isNaN()](#isnan)
- [isNull()](#isnull)
- [isSet()](#isset)
- [jPath()](#jPath)
- [list()](#list)
- [regexGroup()](#regexGroup)
- [regexIsMatch()](#regexIsMatch)
- [switch()](#switch)
- [throw()](#throw)
- [timeSpan()](#timespan)
- [toDateTime()](#toDateTime)
- [typeOf()](#typeOf)

String functions:
- [capitalize()](#capitalize)
- [contains()](#contains)
- [endsWith()](#endswith)
- [indexOf()](#indexof)
- [padLeft()](#padLeft)
- [parseInt()](#parseInt)
- [lastIndexOf()](#lastindexof)
- [length()](#length)
- [replace()](#replace)
- [startsWith()](#startswith)
- [substring()](#substring)
- [toLower()](#tolower)
- [toUpper()](#toupper)
- [toString()](#tostring)

Supported functions:

---
# canEvaluate()

## Purpose
Determines whether ALL of the parameters can be evaluated.  This can be used, for example, to test whether a parameter is set.

## Parameters
- parameter1, parameter2, ...

## Examples
- canEvaluate(nonExistent) : false
- canEvaluate(1) : true

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
# changeTimeZone()

## Purpose
Change a DateTime's time zone.
For a list of supported TimeZone names, see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

## Parameters
- source DateTime
- source TimeZone name
- destination TimeZone name

## Examples
- changeTimeZone(theDateTime, 'UTC', 'Eastern Standard Time')
- changeTimeZone(theDateTime, 'Eastern Standard Time', 'UTC')

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
- timeZone [optional] - see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

## Examples
- format(1, '00') : '01'
- format(1.0, '00') : '01'
- format('01/01/2019', 'yyyy-MM-dd') : '2019-01-01'
- format(theDateTime, 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') [where theDateTime is a .NET DateTime, set to DateTime.Parse("2020-03-13 16:00", CultureInfo.InvariantCulture)] : '2020-03-13 12:00'

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
- indexOf('#abcabc#', 'abc') : 1
- indexOf('#abcabc#', 'abcd') : -1
---

# isNull()

## Purpose

Determines whether a value is either:
- null; or
- it's a JObject and it's type is JTokenType.Null.

## Parameters
- value

## Examples
- isNull(1) : false
- isNull('text') : false
- isNull(bob) : true if bob is null
- isNull(null) : true
---

# isSet()

## Purpose

Determines whether a parameter is set:

## Parameters
- parameter name

## Examples
- isSet('a') : true/false depending on whether a is an available variable

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

# list()

## Purpose

Emits a List<object?> and collapses down lists of lists to a single list.

## Parameters
- the parameters

## Examples
- list('', 1, '0')
- list(null, 1, '0')
- list(list(null, 1, '0'), 1, '0')

---

# padLeft()

## Purpose

Pad the left of a string with a character to a desired string length.

## Parameters
- stringToPad
- desiredStringLength (must be >=1)
- paddingCharacter

## Examples
- padLeft('', 1, '0') : '0'
- padLeft('12', 5, '0') : '00012'
- padLeft('12345', 5, '0') : '12345'
- padLeft('12345', 3, '0') : '12345'

---

# parseInt()

## Purpose

Returns an integer version of a string.

## Parameters
- integerAsString

## Examples
- parseInt('1') : 1

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

Retrieves part of a string.  If more characters are requested than available at the end of the string, just the available characters are returned.

## Parameters
- inputString
- startIndex
- length (optional)

## Examples
- substring('haystack', 3) : 'stack'
- substring('haystack', 0, 3) : 'hay'
- substring('haystack', 3, 100) : 'stack'
- substring('haystack', 0, 100) : 'haystack'

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
- timeSpan('2019-01-01 00:01:00', '2019-01-01 00:02:00', 'seconds') : 3600

---
# toDateTime()

## Purpose

Converts a string to a UTC DateTime.  May take an optional inputTimeZone.

Note that when using numbers as the first input parameter, provide it as a decimal (see examples, below)
to avoid hitting an NCalc bug relating to longs being interpreted as floats.


## Parameters
- inputString
- stringFormat
- inputTimeZone (optional) See https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

## Examples
- toDateTime('2019-01-01', 'yyyy-MM-dd') : A date time representing 2019-01-01
- toDateTime('2020-02-29 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') : A date time representing 2020-02-29 17:00:00 UTC
- toDateTime('2020-03-13 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') : A date time representing 2020-03-13 16:00:00 UTC
- toDateTime(161827200.0, 's', 'UTC') : A date time representing 1975-02-17 00:00:00 UTC
- toDateTime(156816000000.0, 'ms', 'UTC') : A date time representing 1974-12-21 00:00:00 UTC
- toDateTime(156816000000000.0, 'us', 'UTC') : A date time representing 1974-12-21 00:00:00 UTC

---
# toLower()

## Purpose

Converts a string to lower case.

## Parameters
- string

## Examples
- toLower('PaNToMIMe') : 'pantomime'

---
# toString()

## Purpose

Converts any object to a string

## Parameters
- object

## Examples
- toString(1) : '1'

---
# toUpper()

## Purpose

Converts a string to upper case.

## Parameters
- string

## Examples
- toUpper('PaNToMIMe') : 'PANTOMIME'

---
# typeOf()

## Purpose

Determines the C# type of the object.

## Parameters
- parameter

## Examples
- typeOf('text') : 'String'
- typeOf(1) : 'Int32'
- typeOf(1.1) : 'Double'
- typeOf(null) : null

---
# capitalize()

## Purpose

Capitalizes a string.

## Parameters
- string

## Examples
- capitalize('new year') : 'New Year'

---
# contains()

## Purpose

Determines whether one string contains another.

## Parameters
- string searched-in text
- string searched-for text

## Examples
- contains('haystack containing needle', 'needle') : true
- contains('haystack containing only hay', 'needle') : false

---
# humanize()

## Purpose

Humanizes the value text.

## Parameters
- value
- timeUnit

## Examples
- humanize(3600, 'seconds') : '1 hour'

