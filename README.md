# PanoramicData.NCalcExtensions

[![Nuget](https://img.shields.io/nuget/v/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
[![Nuget](https://img.shields.io/nuget/dt/PanoramicData.NCalcExtensions)](https://www.nuget.org/packages/PanoramicData.NCalcExtensions/)
![License](https://img.shields.io/github/license/panoramicdata/PanoramicData.NCalcExtensions)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5b0ad600b19d42e2b735e4199b795fa2)](https://www.codacy.com/gh/panoramicdata/PanoramicData.NCalcExtensions/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=panoramicdata/PanoramicData.NCalcExtensions&amp;utm_campaign=Badge_Grade)

## Introduction

This nuget package provides extension functions for NCalc.

The NCalc documentation can be found [here (source code)](https://github.com/sklose/NCalc2) and [here (good explanation of built-in functions)](https://github.com/pitermarx/NCalc-Edge/wiki/Functions).

## Functions

| Function | Purpose | Notes |
| -------- | ------- | ----- |
| [all()](#all) | Returns true if all values match the lambda expression, otherwise false. |
| [any()](#any) | Returns true if any values match the lambda expression, otherwise false. |
| [canEvaluate()](#canevaluate) | Determines whether ALL of the parameters can be evaluated.  This can be used, for example, to test whether a parameter is set. |
| [capitalize()](#capitalize) | Capitalizes a string. |
| [cast()](#cast) | Cast an object to another (e.g. float to decimal). |
| [changeTimeZone()](#changetimezone) | Change a DateTime's time zone. |
| [concat()](#concat) | Concatenates lists and objects. |
| [contains()](#contains) | Determines whether one string contains another. |
| [convert()](#convert) | Converts the output of parameter 1 into the result of parameter 2. |
| [count()](#count) | Counts the number of items.  Optionally, only count those that match a lambda. |
| [countBy()](#count) | Counts the number of items by a calculated term. |
| [dateAdd()](#dateAdd) | Add a specified interval to a DateTime. |
| [dateTime()](#dateTime) | Return the DateTime in the specified format as a string, with an optional offset. |
| [dateTimeAsEpoch()](#datetimeasepoch) | Parses the input DateTime and outputs as seconds since the Epoch (1970-01-01T00:00Z). |
| [dateTimeAsEpochMs()](#datetimeasepochms) | Parses the input DateTime and outputs as milliseconds since the Epoch (1970-01-01T00:00Z). |
| [dictionary()](#dictionary) | Builds a Dictionary\<string, object?\> from the parameters provided. |
| [distinct()](#distinct) | Returns only distinct items from the input. |
| [endsWith()](#endswith) | Determines whether a string ends with another string. |
| [extend()](#extend) | Extends an existing object into a JObject with both the original and additional properties. |
| [first()](#first) | Returns the first item in a list that matches a lambda or throws a FormatException if no items match. |
| [firstOrDefault()](#firstOrDefault) | Returns the first item in a list that matches a lambda or null if no items match. |
| [format()](#format) | Formats strings and numbers as output strings with the specified format. |
| [getProperties()](#getproperties) | Returns a list of property names for an object. |
| [getProperty()](#getproperty) | Returns the value of a given property. |
| [humanize()](#humanize) | Converts a value to a more readable format. |
| [if()](#if) | Return one of two values, depending on the input function. |
| [in()](#in) | Determines whether a value is in a set of other values. |
| [indexOf()](#indexof) | Determines the first position of a string within another string. |
| [isGuid()](#isguid) | Determines whether a value is a GUID, or is a string that can be converted to a GUID. |
| [isInfinite()](#isinfinite) | Determines whether a value is infinite. |
| [isNaN()](#isnan) | Determines whether a value is not a number. |
| [isNull()](#isnull) | Determines whether a value is null. |
| [isNullOrEmpty()](#isnullorempty) | Determines whether a value is null or empty. |
| [isNullOrWhiteSpace()](#isnullorwhitespace) | Determines whether a value is null, empty or white space. |
| [isSet()](#isset) | Determines whether a parameter is set. |
| [itemAtIndex()](#itematindex) | Determines the item at the given index. |
| [jObject()](#jobject) | Creates a JObject from key/value pairs. |
| [join()](#join) | Joins a list of strings into a single string. |
| [jPath()](#jpath) | Selects a single value from a JObject using a [JPath](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) expression |
| [last()](#last) | Determines the last value in an IEnumerable. Throws an Exception if no item matches. |
| [lastIndexOf()](#lastindexof) | Determines the last position of a string within another string. |
| [lastOrDefault()](#lastOrDefault) | Determines the last value in an IEnumerable. Returns null if no item matches. |
| [length()](#length) | Determines length of a string or IList. |
| [list()](#list) | Emits a List\<object?\> and collapses down lists of lists to a single list. |
| [listOf()](#listof) | Emits a List\<T\>. |
| [max()](#max) | Emits the maximum value, ignoring nulls. |
| [maxValue()](#maxValue) | Emits the maximum possible value for a given numeric type. |
| [min()](#min) | Emits the minimum value, ignoring nulls. |
| [minValue()](#minValue) | Emits the minimum possible value for a given numeric type. |
| [nullCoalesce()](#nullcoalesce) | Returns the first parameter that is not null, otherwise: null. |
| [orderBy()](#orderby) | Orders an IEnumerable by one or more lambda expressions. |
| [padLeft()](#padleft) | Pad the left of a string with a character to a desired string length. |
| [parse()](#parse) | Returns the conversion of a string to a new type. |
| [parseInt()](#parseint) | Returns an integer version of a string. | Deprecated - use parse() or tryParse() instead) |
| [regexGroup()](#regexgroup) | Selects a regex group capture. |
| [regexIsMatch()](#regexismatch) | Determine whether a string matches a regex. |
| [replace()](#replace) | Replace a string with another string. |
| [retrieve()](#retrieve) | Retrieves a value from storage. |
| [reverse()](#reverse) | Reverses an IEnumerable and emits a List<object?>. |
| [sanitize()](#sanitize) | Sanitizes a string, replacing any characters outside of the allowed set. |
| [select()](#select) | Converts an IEnumerable using a lambda. |
| [selectDistinct()](#selectdistinct) | Converts an IEnumerable using a lambda and removes duplicates. |
| [setProperties()](#setproperties) | Sets properties on an existing object. |
| [skip()](#skip) | Skips a number of items in a list. |
| [sort()](#sort) | Sorts an IComparable ascending or descending. |
| [split()](#split) | Splits a string on a given character into a list of strings. |
| [startsWith()](#startswith) | Determines whether a string starts with another string. |
| [store()](#store) | Stores a value for use later in the pipeline. |
| [substring()](#substring) | Retrieves part of a string. |
| [sum()](#sum) | Sums numeric items. |
| [switch()](#switch) | Return one of a number of values, depending on the input function. |
| [take()](#take) | Takes a number of items from a list. |
| [throw()](#throw) | Throws an NCalcExtensionsException. |
| [timeSpan()](#timespan) | Determines the amount of time between two DateTimes. |
| [toDateTime()](#todatetime) | Converts a string to a UTC DateTime.  May take an optional inputTimeZone. |
| [toLower()](#tolower) | Converts a string to lower case. |
| [toString()](#tostring) | Converts any object to a string. |
| [toUpper()](#toupper) | Converts a string to upper case. |
| [try()](#try) | If a function throws an exception, return an alternate value. |
| [tryParse()](#tryparse) | Returns a boolean result of an attempted cast |
| [typeOf()](#typeof) | Determines the C# type of the object. |
| [where()](#where) | Filters an IEnumerable to bring back only those items that match a condition. |

## Usage

````C#
using PanoramicData.NCalcExtensions;

...
var calculation = "lastIndexOf('abcdefg', 'def')";

// Instead of extending the NCalc functions we now just create an ExtendedExpression
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

### all()

#### Purpose
Returns true if all values match the lambda expression, otherwise false.

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
  * count('a piece of string') : 17
  * count(list(1, 2, 3, 4, 5)) : 5
  * count(list(1, 2, 3, 4, 5), 'n', 'n > 3') : 2


---

### countBy()

#### Purpose
Counts the number of items, grouped by a calculation.

#### Parameters
  * list - the original list
  * predicate - a string to represent the value to be evaluated
  * nCalcString - the string to evaluate.  Must emit a string containing one or more characters: A-Z, a-z, 0-9 or _.

#### Examples
  * countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))') : { 'false': 1, 'true': 6 }
  * countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)') : { '1': 1, '2': 2, '3', 3, '4', '1 }

---

### dateAdd()

#### Purpose
Add a specified period to a DateTime.
The following units are supported:
   * Years
   * Months
   * Days
   * Hours
   * Minutes
   * Seconds
   * Milliseconds

#### Parameters
  * intialDateTime - A DateTime to which to add the period specified
  * quantity - The integer number of the units to be added
  * units - A string representing the units used to specify the period to be added

#### Examples
  * dateAdd(toDateTime('2019-03-05 05:09', 'yyyy-MM-dd HH:mm'), -90, 'days') : 90 days before (2018-12-05 05:09:00)
  * dateAdd(toDateTime('2019-03-05 01:03:05', 'yyyy-MM-dd HH:mm:ss'), 2, 'hours') : 2 hours later (2019-03-05 03:03:05)

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

### dateTimeAsEpoch()

#### Purpose
Parses the input DateTime and outputs as seconds since the Epoch (1970-01-01T00:00Z).

#### Parameters
  * input date string
  * format

#### Examples
  * dateTimeAsEpoch('20190702T000000', 'yyyyMMddTHHmmssK') : 1562025600

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

### dictionary()

#### Purpose
Emits a Dictionary<string, object?>.

#### Parameters
* interlaced keys and values. You must provide an even number of parameters, and keys must evaluate to strings.

#### Examples
* dictionary('KEY1', 'Hello', 'KEY2', 'Goodbye') : a dictionary containing 2 values with keys KEY1 and KEY2, and string values
* dictionary('TRUE', true, 'FALSE', false) : a dictionary containing 2 values with keys TRUE and FALSE, and boolean values

---

### distinct()

#### Purpose
Returns only distinct items from the input.

#### Parameters
* list - the original list

#### Examples
* distinct(list(1, 2, 3, 3, 3)) : list(1, 2, 3)

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
### extend()

#### Purpose
Extends an existing object into a JObject with both the original and additional properties.

#### Parameters
* originalObject
* listOfAdditionalProperties

#### Examples
* extend(jObject('a', 1, 'b', null), list('c', 5)) : JObject with a=1, b=null and c=5

---
### first()

#### Purpose
Returns the first item in a list that matches a lambda or throws a FormatException if no items match.

#### Parameters
* list
* predicate
* lambda expression as a string

#### Examples
* first(list(1, 5, 2, 3), 'n', 'n % 2 == 0') : 2
* first(list(1, 5, 7, 3), 'n', 'n % 2 == 0') : FormatException thrown

---
### firstOrDefault()

#### Purpose
Returns the first item in a list that matches a lambda or null if no items match.

#### Parameters
* list
* predicate
* lambda expression as a string

#### Examples
* firstOrDefault(list(1, 5, 2, 3), 'n', 'n % 2 == 0') : 2
* firstOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0') : null

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

### getProperties()

#### Purpose
Gets a list of an object's properties.

#### Parameters
* sourceObject

#### Examples
* getProperties(parse('jObject', '{ "A": 1, "B": 2 }')) : [ 'A', 'B' ]
* getProperties(toDateTime('2019-01-01', 'yyyy-MM-dd')) : [ 'Date', 'Day', 'DayOfWeek', 'DayOfYear', 'Hour', 'Kind', 'Millisecond', 'Microsecond', 'Nanosecond', 'Minute', 'Month', 'Now', 'Second', 'Ticks', 'TimeOfDay', 'Today', 'Year', 'UtcNow' ]

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
Determines whether a value is null.

#### Notes
Returns true if the value is:
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
Determines whether a value is null or empty.

#### Notes
True if:
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
Determines whether a value is null, empty or whitespace.

#### Notes
Returns true is the value is:
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
Determines whether a parameter is set.

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
"details": {
   "this.thing": "woo",
   "that.thing": "yay",
},
"numbers": [ 1, 2 ],
"arrayList": [ 
	{ "key": "key1", "value": "value1" },
	{ "key": "key2", "value": "value2" } 
]
}
```
* jPath(sourceJObject, 'name') : 'bob'
* jPath(sourceJObject, 'details.[\'this.thing\']') : 'woo'
* jPath(sourceJObject, 'size') : an exception is thrown
* jPath(sourceJObject, 'size', True) : null is returned
* jPath(sourceJObject, 'numbers[0]') : 1
* jPath(sourceJObject, 'arrayList[?(@key==\\'key1\\')]') : "value1"
---


#### Purpose
Returns the last item in a list that matches a lambda or throws a FormatException if no items match.
Note that items are processed in reverse order.

#### Parameters
* list
* predicate
* lambda expression as a string

#### Examples
* first(list(1, 5, 2, 3, 4, 1), 'n', 'n % 2 == 0') : 4
* first(list(1, 5, 7, 3), 'n', 'n % 2 == 0') : FormatException thrown

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

### lastOrDefault()

#### Purpose
Returns the last item in a list that matches a lambda or null if no items match.
Note that items are processed in reverse order.

#### Parameters
* list
* predicate
* lambda expression as a string

#### Examples
* lastOrDefault(list(1, 5, 2, 3, 4, 1), 'n', 'n % 2 == 0') : 4
* lastOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0') : null

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

### list()

#### Purpose
Emits a List\<object?\> and collapses down lists of lists to a single list.

#### Parameters
  * the parameters

#### Examples
  * list('', 1, '0')
  * list(null, 1, '0')
  * list(list(null, 1, '0'), 1, '0')
---

### listOf()

#### Purpose
Emits a List\<T\>.

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

### maxValue()

#### Purpose
   Emits the maximum possible value for a given numeric type.

#### Parameters
   * a string representing the type, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double' or 'decimal'.

#### Examples
   * maxValue('byte') : (byte)255
   * maxValue('ushort') : (ushort)65535
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

### minValue()

#### Purpose
   Emits the minimum possible value for a given numeric type.

#### Parameters
   * a string representing the type, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double' or 'decimal'.

#### Examples
   * minValue('byte') : (byte)0
   * minValue('ushort') : (ushort)0

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

### reverse()

#### Purpose
   Reverses an IEnumerable.

#### Parameters
   * list

#### Examples
   * reverse(list(1, 2, 3, 4, 5, 5) : 5, 5, 4, 3, 2, 1

---

### sanitize()

#### Purpose
   Sanitize a string, replacing any characters outside of the allowed set.

#### Parameters
   * input - the string to be sanitized
   * allowedCharacters - all of the characters that are allowed
   * replacementCharacters (optional) - the characters to insert in place of any that are not allowed (defaults : empty string)

#### Examples
   * sanitize('ab cd', 'abcdefghi') : 'abcd'
   * sanitize('ab cd./', 'abcdefghi.') : 'abcd.'
   * sanitize('ab cd./', 'CBa') : 'a'
   * sanitize('ab cd', 'abCD', '?') : 'ab???'
   * sanitize('ab cd', 'ab', '?') : 'ab???'
   * sanitize('ab cd', 'ab', '-') : 'ab---'

---

### select()

#### Purpose
   Converts an IEnumerable using a lambda.

#### Parameters
   * list - the original list
   * predicate - a string to represent the value to be evaluated
   * nCalcString - the value to evaluate to for each item in the list
   * output list type - outputs a list of the specified type (optional)

#### Examples
   * select(list(1, 2, 3, 4, 5), 'n', 'n + 1') : list(2, 3, 4, 5, 6)
   * select(list(jObject('a', 1, 'b', '2'), jObject('a', 3, 'b', '4')), 'n', 'n', 'JObject') : list of JObjects

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

### skip()

#### Purpose
   Skips a number of items in a list.

#### Notes
   If the number of items to skip is greater than the number of items in the list, an empty list is returned.

#### Parameters
   * the list to skip from
   * the number of items to skip

#### Examples
   * skip(list(1, 2, 3), 1): list(2, 3)

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

### split()

#### Purpose
   Splits a string on a given character into a list of strings.

#### Parameters
   * longString
   * string to split on

#### Examples
   * split('a bc d', ' ') : list('a', 'bc', 'd')
   * split('aXXbcXXd', 'XX') : list('a', 'bc', 'd')

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

### store()

#### Purpose
   Stores a value for use later in the pipeline

#### Returns

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

#### Notes
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

#### Notes
   When using numbers as the first input parameter, provide it as a decimal (see examples, below)
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

### tryParse()

#### Purpose
   Returns a boolean result of an attempted cast.

#### Parameters
   * type
   * value
   * key - for use with the retrieve() function

#### Examples
   * tryParse('int', '1', 'outputVariable') : true
   * tryParse('int', 'string', 'outputVariable') : false

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
