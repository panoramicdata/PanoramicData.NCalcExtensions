# PanoramicData.NCalcExtensions

Extension functions for NCalc, documentation for which can be found [here (source code)](https://github.com/sklose/NCalc2) and [here (good explanation of built-in functions)](https://github.com/pitermarx/NCalc-Edge/wiki/Functions).

[![Nuget](https://img.shields.io/nuget/v/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
[![Nuget](https://img.shields.io/nuget/dt/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
![License](https://img.shields.io/github/license/panoramicdata/PanoramicData.NCalcExtensions)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5b0ad600b19d42e2b735e4199b795fa2)](https://www.codacy.com/gh/panoramicdata/PanoramicData.NCalcExtensions/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=panoramicdata/PanoramicData.NCalcExtensions&amp;utm_campaign=Badge_Grade)

Please submit your requests for new functions in the form of pull requests.

## Functions

| Function | Purpose | Notes |
| -------- | ------- | ----- |
| [all()](#all) | Returns true if all values match the lambda expression, otherwise false. |
| [any()](#any) | Returns true if any values match the lambda expression, otherwise false. |
| [canEvaluate()](#canEvaluate) | Determines whether ALL of the parameters can be evaluated.  This can be used, for example, to test whether a parameter is set. |
| [capitalize()](#capitalize) | Capitalizes a string. |
| [cast()](#cast) | Cast an object to another (e.g. float to decimal). |
| [changeTimeZone()](#changeTimeZone) | Change a DateTime's time zone. |
| [concat()](#concat) | Concatenates lists and objects. |
| [contains()](#contains) | Determines whether one string contains another. |
| [convert()](#convert) | Converts the output of parameter 1 into the result of parameter 2. |
| [count()](#count) | Counts the number of items.  Optionally, only count those that match a lambda. |
| [dateTime()](#dateTime) | Return the DateTime in the specified format as a string, with an optional offset. |
| [dateTimeAsEpochMs()](#dateTimeAsEpochMs) | Parses the input DateTime and outputs as milliseconds since the Epoch (1970-01-01T00:00Z). |
| [distinct()](#distinct) | Returns only distinct items from the input. |
| [endsWith()](#endswith) | Determines whether a string ends with another string. |
| [format()](#format) | Formats strings and numbers as output strings with the specified format. |
| [if()](#if) | Return one of two values, depending on the input function. |
| [in()](#in) | Determines whether a value is in a set of other values. |
| [indexOf()](#indexof) | Determines the first position of a string within another string. |
| [isGuid()](#isGuid) | Determines whether a value is a GUID, or is a string that can be converted to a GUID. |
| [isInfinite()](#isinfinite) | Determines whether a value is infinite. |
| [isNaN()](#isnan) | Determines whether a value is not a number. |
| [isNull()](#isnull) | - |
| [isNullOrEmpty()](#isnullOrEmpty) | - |
| [isNullOrWhiteSpace()](#isnullOrWhiteSpace) | - |
| [isSet()](#isset) | - |
| [itemAtIndex()](#itemAtIndex) | - |
| [jObject()](#jObject) | - |
| [join()](#join) | - |
| [jPath()](#jPath) | - |
| [lastIndexOf()](#lastindexof) | - |
| [length()](#length) | - |
| [list()](#list) | - |
| [listOf()](#listOf) | - |
| [max()](#list) | - |
| [min()](#list) | - |
| [nullCoalesce()](#nullCoalesce) | - |
| [orderBy()](#orderBy) | - |
| [padLeft()](#padLeft) | - |
| [parse()](#parse) | - |
| [parseInt()](#parseInt) (deprecated - use "parse()" instead) | - |
| [regexGroup()](#regexGroup) | - |
| [regexIsMatch()](#regexIsMatch) | - |
| [replace()](#replace) | - |
| [retrieve()](#retrieve) | - |
| [select()](#select) | - |
| [selectDistinct()](#selectDistinct) | - |
| [skip()](#skip) | - |
| [sort()](#sort) | - |
| [split()](#split) | - |
| [startsWith()](#startswith) | - |
| [store()](#store) | - |
| [substring()](#substring) | - |
| [switch()](#switch) | - |
| [take()](#take) | - |
| [throw()](#throw) | - |
| [timeSpan()](#timespan) | - |
| [toDateTime()](#toDateTime) | - |
| [toLower()](#tolower) | - |
| [toString()](#tostring) | - |
| [toUpper()](#toupper) | - |
| [try()](#try) | - |
| [typeOf()](#typeOf) | - |
| [where()](#where) | - |

## Usage

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
## Namespace safety

Note that we regularly extend this library to add new functions.
For those that *further* extend the functions, it is important to avoid future potential namespace conflicts.
For this reason, we guarantee that we will never use the underscore character in any future function name and we recommend that any functions that you add DO include an underscore.  For example, if you had a project called "My Project", we suggest that you name your functions as follows:

* mp_myFunction1()
* mp_myFunction2()

## Function documentation

### all()

#### Purpose
{{ all }}

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the string to evaluate

#### Examples
  * all(list(1, 2, 3, 4, 5), 'n', 'n < 3') : false
  * all(list(1, 2, 3, 4, 5), 'n', 'n > 0 && n < 10') : true

---
### any()

#### Purpose

Returns true if any values match the lambda expression, otherwise false.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the string to evaluate

#### Examples
  * any(list(1, 2, 3, 4, 5), 'n', 'n < 3') : true
  * any(list(1, 2, 3, 4, 5), 'n', 'n > 11') : false

---
### canEvaluate()

#### Purpose
Determines whether ALL of the parameters can be evaluated.  This can be used, for example, to test whether a parameter is set.

#### Parameters
  * parameter1, parameter2, ...

#### Examples
  * canEvaluate(nonExistent) : false
  * canEvaluate(1) : true

---
### capitalize()

#### Purpose

Capitalizes a string.

#### Parameters
  * string

#### Examples
  * capitalize('new year') : 'New Year'

---
### cast()

#### Purpose
Cast an object to another (e.g. float to decimal).

#### Notes

The method requires that conversion of value to target type be supported.

#### Parameters
  * inputObject
  * typeString

#### Examples
  * cast(0.3, 'System.Decimal')

---
### changeTimeZone()

#### Purpose
Change a DateTime's time zone.

#### Notes
For a list of supported TimeZone names, see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

#### Parameters
  * source DateTime
  * source TimeZone name
  * destination TimeZone name

#### Examples
  * changeTimeZone(theDateTime, 'UTC', 'Eastern Standard Time')
  * changeTimeZone(theDateTime, 'Eastern Standard Time', 'UTC')

---
### contains()

#### Purpose
Determines whether one string contains another.

#### Parameters
  * string searched-in text
  * string searched-for text

#### Examples
  * contains('haystack containing needle', 'needle') : true
  * contains('haystack containing only hay', 'needle') : false

---
### concat()

#### Purpose
Concatenates lists and objects.

#### Notes
The examples all result in a List<object?> containing 4 integers: 1, 2, 3 and 4.

#### Parameters
  * the lists or objects to concatenate

#### Examples
  * concat(list(1, 2), list(3, 4))
  * concat(list(1, 2, 3, 4))
  * concat(1, 2, 3, 4)
  * concat(list(1, 2, 3), 4)
  * concat(1, list(2, 3, 4))

---
### convert()

#### Purpose
Converts the output of parameter 1 into the result of parameter 2.

#### Notes
Can be used to return an empty string instead of the result of parameter 1,
which can be useful when the return value is not useful.
The result of parameter 1 is available as the variable "value".

#### Parameters
  * the value to calculate
  * destination TimeZone name

#### Examples
  * convert(anyFunction(), 'XYZ'): 'XYZ'
  * convert(1 + 1, value + 1): 3

---
### count()

#### Purpose
Counts the number of items.  Optionally, only count those that match a lambda.

#### Parameters
  * list - the original list
  * predicate (optional) - a string to represent the value to be evaluated
  * nCalcString (optional) - the string to evaluate

#### Examples
  * count(list(1, 2, 3, 4, 5)) : 5
  * count(list(1, 2, 3, 4, 5), 'n', 'n > 3') : 2

---
### dateTime()

#### Purpose
Return the DateTime in the specified format as a string, with an optional offset.

#### Parameters
  * timeZone (only 'UTC' currently supported)
  * format
  * day offset
  * hour offset
  * minute offset
  * second offset

#### Examples
  * dateTime('UTC', 'yyyy-MM-dd HH:mm:ss', -90, 0, 0, 0) : 90 days ago (e.g. '2019-03-14 05:09')
  * dateTime('UTC', 'yyyy-MM-dd HH:mm:ss') : now (e.g. '2019-03-14 05:09')

---
### dateTimeAsEpochMs()

#### Purpose
Parses the input DateTime and outputs as milliseconds since the Epoch (1970-01-01T00:00Z).

#### Parameters
  * input date string
  * format

#### Examples
  * dateTimeAsEpochMs('20190702T000000', 'yyyyMMddTHHmmssK') : 1562025600000

---
### distinct()

#### Purpose
Returns only distinct items from the input.

#### Parameters
  * list - the original list

#### Examples
  * distinct(list(1, 2, 3, 3, 3)) : list(1, 2, 3)

---
### format()

#### Purpose
Formats strings and numbers as output strings with the specified format.

#### Parameters
  * object (number or text)
  * format: the format to use
  - see C# number and date/time formatting
  - weekOfMonth is the numeric week of month as would be shown on a calendar with one row per week with weeks starting on a Sunday
  - weekOfMonthText is the same as weekOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last'
  - weekDayOfMonth is the number of times this weekday has occurred within the month so far, including this one
  - weekDayOfMonthText is the same as weekDayOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last'
  * timeZone [optional] - see https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

#### Examples
  * format(1, '00') : '01'
  * format(1.0, '00') : '01'
  * format('2021-11-29', 'dayOfYear') : '333'
  * format('2021-11-01', 'weekOfMonth') : 1
  * format('2021-11-01', 'weekOfMonthText') : 'first'
  * format('2021-11-28', 'weekOfMonth') : 5
  * format('2021-11-28', 'weekOfMonthText') : 'last'
  * format('2021-11-28', 'weekDayOfMonth') : 4
  * format('2021-11-28', 'weekDayOfMonthText') : 'forth'
  * format('01/01/2019', 'yyyy-MM-dd') : '2019-01-01'
  * format(theDateTime, 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') [where theDateTime is a .NET DateTime, set to DateTime.Parse("2020-03-13 16:00", CultureInfo.InvariantCulture)] : '2020-03-13 12:00'

---
### humanize()

#### Purpose

Humanizes the value text.

#### Parameters
  * value
  * timeUnit

#### Examples
  * humanize(3600, 'seconds') : '1 hour'

---
### if()

#### Purpose
Return one of two values, depending on the input function.

#### Parameters
  * condition
  * output if true
  * output if false

#### Examples
  * if(1 == 1, 'yes', 'no') : 'yes'
  * if(1 == 2, 3, 4) : 4

---
### in()

#### Purpose
Determines whether a value is in a set of other values.

#### Parameters
  * list
  * item

#### Examples
  * in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') : true
  * in('needle', 'haystack', 'with', 'only', 'hay') : false

---
### indexOf()

#### Purpose
Determines the first position of a string within another string.

#### Notes
The first character is position 0.  Returns -1 if not present.

#### Parameters
  * longString
  * shortString

#### Examples
  * indexOf('#abcabc#', 'abc') : 1
  * indexOf('#abcabc#', 'abcd') : -1

---
### isGuid()

#### Purpose
Determines whether a value is a GUID, or is a string that can be converted to a GUID.

#### Parameters
  * value

#### Examples
  * isGuid('9384EF0Z-38AD-4E8E-A24E-0ACD3273A401') : true
  * isGuid('{9384EF0Z-38AD-4E8E-A24E-0ACD3273A401}') : true
  * isGuid('abc') : false

---
### isInfinite()

#### Purpose
Determines whether a value is infinite.

#### Parameters
  * value

#### Examples
  * isInfinite(1/0) : true
  * isInfinite(0/1) : false

---
### isNaN()

#### Purpose
Determines whether a value is not a number.

#### Parameters
  * value

#### Examples
  * isNaN(null) : true
  * isNaN('abc') : true
  * isNaN(1) : false
---

### isNull()

#### Purpose

Determines whether a value is either:
  * null; or
  * it's a JObject and it's type is JTokenType.Null.

#### Parameters
  * value

#### Examples
  * isNull(1) : false
  * isNull('text') : false
  * isNull(bob) : true if bob is null
  * isNull(null) : true
---

### isNullOrEmpty()

#### Purpose

Determines whether a value is either:
  * null; or
  * it's a JObject and it's type is JTokenType.Null or;
  * it's a string and it's empty.

#### Parameters
  * value

#### Examples
  * isNullOrEmpty(null) : true
  * isNullOrEmpty('') : true
  * isNullOrEmpty(' ') : false
  * isNullOrEmpty(bob) : true if bob is null or whitespace
  * isNullOrEmpty(1) : false
  * isNullOrEmpty('text') : false
---

### isNullOrWhiteSpace()

#### Purpose

Determines whether a value is either:
  * null; or
  * it's a JObject and it's type is JTokenType.Null or;
  * it's a string and it's empty or only contains whitespace characters (\r, \n, \t, or ' ').

#### Parameters
  * value

#### Examples
  * isNullOrWhiteSpace(null) : true
  * isNullOrWhiteSpace('') : true
  * isNullOrWhiteSpace(' ') : true
  * isNullOrWhiteSpace(bob) : true if bob is null or whitespace
  * isNullOrWhiteSpace(1) : false
  * isNullOrWhiteSpace('text') : false
---

### isSet()

#### Purpose

Determines whether a parameter is set:

#### Parameters
  * parameter name

#### Examples
  * isSet('a') : true/false depending on whether a is an available variable

---
### itemAtIndex()

#### Purpose

Determines the item at the given index.  The first index is 0.

#### Parameters
  * parameter name

#### Examples
  * itemAtIndex(split('a b c', ' '), 1) : 'b'

---
### jObject()

#### Purpose

Creates a JObject from key/value pairs.

#### Parameters
  * key1 (string)
  * value1
  * key2 (string)
  * value2
  * ...
  * keyN
  * valueN

#### Examples
  * jObject('a', 1, 'b', null) : JObject{ "a": 1, "b": null}


---
### join()

#### Purpose

Joins a list of strings into a single string.

#### Parameters
  * parameter name

#### Examples
  * join(split('a b c', ' '), ', ') : 'a, b, c'

---
### lastIndexOf()

#### Purpose

Determines the last position of a string within another string.  Returns -1 if not present.

#### Parameters
  * longString
  * shortString

#### Examples
  * lastIndexOf('#abcabc#', 'abc') : 4
  * lastIndexOf('#abcabc#', 'abcd') : -1

---
### length()

#### Purpose

Determines length of a string or IList.

#### Parameters
  * string or IList

#### Examples
  * length('a piece of string') : 17
  * length(split('a piece of string', ' ')) : 4

---
### nullCoalesce()

#### Purpose

Returns the first parameter that is not null, otherwise: null.

#### Parameters
  * any number of objects

#### Examples
  * nullCoalesce() : null
  * nullCoalesce(1, null) : 1
  * nullCoalesce(null, 1, 2, 3) : 1
  * nullCoalesce(null, null, null) : null
  * nullCoalesce(null, null, 'xxx', 3) : 'xxx'

---
### orderBy()

#### Purpose

Orders an IEnumerable by one or more lambda expressions.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString1 - the first orderBy lambda expression
  * nCalcString2 (optional) - the next orderBy lambda expression
  * nCalcString... (optional) - the next orderBy lambda expression

#### Examples
  * orderBy(list(34, 33, 2, 1), 'n', 'n') : list(1, 2, 33, 34)
  * orderBy(list(34, 33, 2, 1), 'n', '-n') : list(34, 33, 2, 1)
  * orderBy(list(34, 33, 2, 1), 'n % 32', 'n % 2') : list(34, 33, 1, 2)
  * orderBy(list(34, 33, 2, 1), 'n % 2', 'n % 32') : list(33, 1, 34, 2)

---
### skip()

#### Purpose
Skips a number of items in a list.
If the number of items to skip is greater than the number of items in the list, an empty list is returned.

#### Parameters
  * the list to skip from
  * the number of items to skip

#### Examples
  * skip(list(1, 2, 3), 1): list(2, 3)

---
### split()

#### Purpose

Splits a string on a given character into a list of strings.

#### Parameters
  * longString
  * character

#### Examples
  * split('a bc d', ' ') : list('a', 'bc', 'd')

---
### startsWith()

#### Purpose

Determines whether a string starts with another string.

#### Parameters
  * longString
  * shortString

#### Examples
  * startsWith('abcdefg', 'ab') : true
  * startsWith('abcdefg', 'cd') : false

---
### endsWith()

#### Purpose
Determines whether a string ends with another string.

#### Parameters
  * longString
  * shortString

#### Examples
  * endsWith('abcdefg', 'fg') : true
  * endsWith('abcdefg', 'fgh') : false

---
### getProperty()

#### Purpose

Gets an object's property.

#### Parameters
  * sourceObject
  * propertyName

#### Examples
  * getProperty(toDateTime('2019-01-01', 'yyyy-MM-dd'), 'Year') : 2019 (int)

---

### jPath()

#### Purpose

Selects a single value from a JObject using a [JPath](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) expression

#### Parameters
  * input JObject
  * JPath string expression

#### Examples
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
  * jPath(sourceJObject, 'name') : 'bob'
  * jPath(sourceJObject, 'size') : an exception is thrown
  * jPath(sourceJObject, 'size', True) : null is returned
  * jPath(sourceJObject, 'numbers[0]') : 1
  * jPath(sourceJObject, 'arrayList[?(@key==\\'key1\\')]') : "value1"
---

### list()

#### Purpose

Emits a List<object?> and collapses down lists of lists to a single list.

#### Parameters
  * the parameters

#### Examples
  * list('', 1, '0')
  * list(null, 1, '0')
  * list(list(null, 1, '0'), 1, '0')
---

### listOf()

#### Purpose

Emits a List&lt;T&gt;.

#### Parameters
  * the type
  * the parameters

#### Examples
  * listOf('object?', '', 1, '0')
  * listOf('object?', null, 1, '0')
  * listOf('int?', 1, null, 3)
  * listOf('string', '1', '2', 3) : throws an exception
---

### max()

#### Purpose

Emits the maximum value, ignoring nulls.

#### Parameters
  * the list
  * optionally, a pair of parameters providing a lambda expression to be evaluated.

#### Examples
  * max(listOf('int?', 1, null, 3)) : 3
  * max(listOf('int', 1, 2, 3), 'x', 'x + 1') : 4
  * max(listOf('string', '1', '2', '3')) : '3'
  * max(listOf('string', '1', '2', '3'), 'x', 'x + x') : '33'
---

### min()

#### Purpose

Emits the minimum value, ignoring nulls.

#### Parameters
  * the list
  * optionally, a pair of parameters providing a lambda expression to be evaluated.

#### Examples
  * min(listOf('int?', 1, null, 3)) : 1
  * min(listOf('int', 1, 2, 3), 'x', 'x + 1') : 2
  * min(listOf('string', '1', '2', '3')) : '1'
  * min(listOf('string', '1', '2', '3'), 'x', 'x + x') : '11'

---

### padLeft()

#### Purpose

Pad the left of a string with a character to a desired string length.

#### Parameters
  * stringToPad
  * desiredStringLength (must be >=1)
  * paddingCharacter

#### Examples
  * padLeft('', 1, '0') : '0'
  * padLeft('12', 5, '0') : '00012'
  * padLeft('12345', 5, '0') : '12345'
  * padLeft('12345', 3, '0') : '12345'

---

### parse()

#### Purpose

Returns the conversion of a string to a numeric type.  Supported types are:
  * bool
  * sbyte
  * byte
  * short
  * ushort
  * int
  * uint
  * long
  * ulong
  * double
  * float
  * decimal
  * JArray (jArray also supported for backaward compatibility)
  * JObject (jObject also supported for backaward compatibility)
  * Guid

#### Parameters
  * type (see above)
  * text
  * valueIfParseFails (optional)

#### Examples
  * parse('int', '1') : 1
  * parse('bool', 'x', null) : null
  * parse('jObject', '{ "a" : 1 }', null) : null
  * parse('jArray', '[ { "a" : 1 } ]', null) : null
---

### parseInt()

#### Purpose

Returns an integer version of a string.

#### Parameters
  * integerAsString

#### Examples
  * parseInt('1') : 1

---
### regexGroup()

#### Purpose

Selects a regex group capture

#### Parameters
  * input
  * regex
  * zero-based capture index (default: 0)

#### Examples
  * regexGroup('abcdef', '^ab(.+?)f$') : 'cde'
  * regexGroup('abcdef', '^ab(.)+f$') : 'c'
  * regexGroup('abcdef', '^ab(.)+f$', 1) : 'd'
  * regexGroup('abcdef', '^ab(.)+f$', 2) : 'e'
  * regexGroup('abcdef', '^ab(.)+f$', 10) : null

---
### regexIsMatch()

#### Purpose

Determine whether a string matches a regex

#### Parameters
  * input
  * regex

#### Examples
  * regexIsMatch('abcdef', '^ab.+') : true
  * regexIsMatch('Zbcdef', '^ab.+') : false

---
### replace()

#### Purpose

Replace a string with another string

#### Parameters
  * haystackString
  * needleString
  * betterNeedleString

#### Examples
  * replace('abcdefg', 'cde', 'CDE') : 'abCDEfg'
  * replace('abcdefg', 'cde', '') : 'abfg'

---
### retrieve()

#### Purpose

Retrieves a value from storage

#### Parameters
  * key

#### Examples
  * retrieve('thing')

---
### select()

#### Purpose

Converts an IEnumerable using a lambda.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the value to evaluate to for each item in the list

#### Examples
  * select(list(1, 2, 3, 4, 5), 'n', 'n + 1') : list(2, 3, 4, 5, 6)

---
### selectDistinct()

#### Purpose

Converts an IEnumerable using a lambda and removes duplicates.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the value to evaluate to for each item in the list

#### Examples
  * selectDistinct(list(1, 2, 3, 3, 3), 'n', 'n + 1') : list(2, 3, 4)

---
### setProperties()

#### Purpose

Sets properties on an existing object.

#### Parameters
  * object - the original object
  * property1 - the first new property name
  * value1 - the first new property value
  * propertyN (optional) - the nth new property name
  * valueN (optional) - the nth new property value
#### Examples
  * setProperties(jObject('a', 1, 'b', null), 'c', 'X') : jObject('a', 1, 'b', null, 'c', 'X')
  * setProperties(jObject('a', 1, 'b', null), 'c', 'X', 'd', 'Y') : jObject('a', 1, 'b', null, 'c', 'X', 'd', 'Y')
---
### sort()

#### Purpose

Sorts an IComparable ascending or descending.

#### Parameters
  * list - the original list
  * direction (optional) - 'asc' is the default, 'desc' is the other option
#### Examples
  * sort(list(2, 1, 3)) : list(1, 2, 3)
  * sort(list(2, 1, 3), 'asc') : list(1, 2, 3)
  * sort(list(2, 1, 3), 'desc') : list(3, 2, 1)
  * sort(list('b', 'a', 'c'))) : list('a', 'b', 'c')

---
### store()

#### Purpose

Stores a value for use later in the pipeline

### Returns

true

#### Parameters
  * key
  * value

#### Examples
  * store('thing', 1) : true

---
### substring()

#### Purpose

Retrieves part of a string.  If more characters are requested than available at the end of the string, just the available characters are returned.

#### Parameters
  * inputString
  * startIndex
  * length (optional)

#### Examples
  * substring('haystack', 3) : 'stack'
  * substring('haystack', 0, 3) : 'hay'
  * substring('haystack', 3, 100) : 'stack'
  * substring('haystack', 0, 100) : 'haystack'
  * substring('haystack', 0, 0) : ''

---
### sum()

#### Purpose

Sums numeric items.  Optionally, perform a lambda on each one first.

#### Parameters
  * list - the original list
  * predicate (optional) - a string to represent the value to be evaluated
  * nCalcString (optional) - the string to evaluate

#### Examples
  * sum(list(1, 2, 3)) : 6
  * sum(list(1, 2, 3), 'n', 'n * n') : 14

---
### switch()

#### Purpose
Return one of a number of values, depending on the input function.

#### Parameters
  * switched value
  * a set of pairs: case_n, output_n
  * if present, a final value can be used as a default.  If the default WOULD have been returned, but no default is present, an exception is thrown.

#### Examples
  * switch('yes', 'yes', 1, 'no', 2) : 1
  * switch('blah', 'yes', 1, 'no', 2) : throws exception
  * switch('blah', 'yes', 1, 'no', 2, 3) : 3

---
### take()

#### Purpose
Takes a number of items from a list.
If a number is provided that is longer than the list, the full list is returned.

#### Parameters
  * the list to take from
  * the number of items to take

#### Examples
  * take(list(1, 2, 3), 2): list(1, 2)
  * take(list(1, 2, 3), 10): list(1, 2, 3)

---
### throw()

#### Purpose

Throws an NCalcExtensionsException.   Useful in an if().

#### Parameters
  * message (optional)

#### Examples
  * throw()
  * throw('This is a message')
  * if(problem, throw('There is a problem'), 5)

---
### timeSpan()

#### Purpose

Determines the amount of time between two DateTimes.
The following units are supported:
  * Years
  * Weeks
  * Days
  * Hours
  * Minutes
  * Seconds
  * Milliseconds
  * Any other string is handled with TimeSpan.ToString(timeUnit). See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings

#### Parameters
  * startDateTime
  * endDateTime
  * timeUnit

#### Examples
  * timeSpan('2019-01-01 00:01:00', '2019-01-01 00:02:00', 'seconds') : 3600

---
### toDateTime()

#### Purpose

Converts a string to a UTC DateTime.  May take an optional inputTimeZone.

Note that when using numbers as the first input parameter, provide it as a decimal (see examples, below)
to avoid hitting an NCalc bug relating to longs being interpreted as floats.


#### Parameters
  * inputString
  * stringFormat
  * inputTimeZone (optional) See https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.findsystemtimezonebyid?view=netstandard-2.0

#### Examples
  * toDateTime('2019-01-01', 'yyyy-MM-dd') : A date time representing 2019-01-01
  * toDateTime('2020-02-29 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') : A date time representing 2020-02-29 17:00:00 UTC
  * toDateTime('2020-03-13 12:00', 'yyyy-MM-dd HH:mm', 'Eastern Standard Time') : A date time representing 2020-03-13 16:00:00 UTC
  * toDateTime(161827200.0, 's', 'UTC') : A date time representing 1975-02-17 00:00:00 UTC
  * toDateTime(156816000000.0, 'ms', 'UTC') : A date time representing 1974-12-21 00:00:00 UTC
  * toDateTime(156816000000000.0, 'us', 'UTC') : A date time representing 1974-12-21 00:00:00 UTC

---
### toLower()

#### Purpose

Converts a string to lower case.

#### Parameters
  * string

#### Examples
  * toLower('PaNToMIMe') : 'pantomime'

---
### toString()

#### Purpose

Converts any object to a string

#### Parameters
  * object
  * format (optional)

#### Examples
  * toString(1) : '1'
  * toString(1000, 'N2') : '1,000.00'
  * toString(DateTimeOffset, 'yyyy-MM-dd') : '2023-02-17'

---
### toUpper()

#### Purpose

Converts a string to upper case.

#### Parameters
  * string

#### Examples
  * toUpper('PaNToMIMe') : 'PANTOMIME'

---
### try()

#### Purpose

If a function throws an exception, return an alternate value.

#### Parameters
  * function to attempt
  * result to return if an exception is thrown (null is returned if this parameter is omitted and an exception is thrown)

#### Examples
  * try(1, 'Failed') : 1
  * try(throw('Woo')) : null
  * try(throw('Woo'), 'Failed') : 'Failed'
  * try(throw('Woo'), exception_message) : 'Woo'
  * try(throw('Woo'), exception_type) : typeof(PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException)
  * try(throw('Woo'), exception_typeFullName) : 'PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException'
  * try(throw('Woo'), exception_typeName) : 'NCalcExtensionsException'
  * try(throw('Woo'), exception) : The Exception object thrown by the throw function.

---
### typeOf()

#### Purpose

Determines the C# type of the object.

#### Parameters
  * parameter

#### Examples
  * typeOf('text') : 'String'
  * typeOf(1) : 'Int32'
  * typeOf(1.1) : 'Double'
  * typeOf(null) : null

---
### where()

#### Purpose

Filters an IEnumerable to bring back only those items that match a condition.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the string to evaluate

#### Examples
  * where(list(1, 2, 3, 4, 5), 'n', 'n < 3') : list(1, 2)
  * where(list(1, 2, 3, 4, 5), 'n', 'n < 3 || n > 4') : list(1, 2, 5)
