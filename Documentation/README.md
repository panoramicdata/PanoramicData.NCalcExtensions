<h1>PanoramicData.NCalcExtensions</h1>

This index points to the generated documentation folders under `Documentation/<function>/`.

| Function | Purpose | Examples |
| --- | --- | ---: |
| [all()](all/README.md) | Returns true if all values match the lambda expression, otherwise false. | 6 |
| [any()](any/README.md) | Returns true if any values match the lambda expression, otherwise false. | 4 |
| [canEvaluate()](canevaluate/README.md) | Determines whether ALL of the parameters can be evaluated. This can be used, for example, to test whether a parameter is set. | 2 |
| [capitalize()](capitalize/README.md) | Capitalizes a string. | 1 |
| [cast()](cast/README.md) | Cast an object to another (e.g. float to decimal). | 1 |
| [changeTimeZone()](changetimezone/README.md) | Change a DateTime's time zone. | 2 |
| [concat()](concat/README.md) | Concatenates lists and objects. | 5 |
| [contains()](contains/README.md) | Determines whether one string contains another. | 2 |
| [convert()](convert/README.md) | Converts the output of parameter 1 into the result of parameter 2. | 2 |
| [count()](count/README.md) | Counts the number of items. Optionally, only count those that match a lambda. | 3 |
| [countBy()](countby/README.md) | Counts the number of items, grouped by a calculation. | 2 |
| [dateAdd()](dateadd/README.md) | Add a specified period to a DateTime. The following units are supported: * Years * Months * Days * Hours * Minutes * Seconds * Milliseconds | 2 |
| [dateTime()](datetime/README.md) | Return the DateTime in the specified format as a string, with an optional offset. | 2 |
| [dateTimeAsEpoch()](datetimeasepoch/README.md) | Parses the input DateTime and outputs as seconds since the Epoch (1970-01-01T00:00Z). | 1 |
| [dateTimeAsEpochMs()](datetimeasepochms/README.md) | Parses the input DateTime and outputs as milliseconds since the Epoch (1970-01-01T00:00Z). | 1 |
| [dateTimeIsInFuture()](datetimeisinfuture/README.md) | Returns whether a date and time is in the future, with optional timezone correction. | 3 |
| [dateTimeIsInPast()](datetimeisinpast/README.md) | Returns whether a date and time is in the past, with optional timezone correction. | 3 |
| [dictionary()](dictionary/README.md) | Emits a Dictionary<string, object?>. | 2 |
| [distinct()](distinct/README.md) | Returns only distinct items from the input. | 1 |
| [endsWith()](endswith/README.md) | Determines whether a string ends with another string. | 2 |
| [extend()](extend/README.md) | Extends an existing object into a JObject with both the original and additional properties. | 2 |
| [first()](first/README.md) | Returns the first item in a list that matches a lambda or throws a FormatException if no items match. | 2 |
| [firstOrDefault()](firstordefault/README.md) | Returns the first item in a list that matches a lambda or null if no items match. | 2 |
| [format()](format/README.md) | Formats strings and numbers as output strings with the specified format. | 13 |
| [getProperties()](getproperties/README.md) | Gets a list of an object's properties. | 3 |
| [getProperty()](getproperty/README.md) | Gets an object's property. | 2 |
| [humanize()](humanize/README.md) | Humanizes the value text. | 1 |
| [if()](if/README.md) | Return one of two values, depending on the input function. | 2 |
| [in()](in/README.md) | Determines whether a value (the first parameter) is in a set of other values (the remaining parameters). | 2 |
| [indexOf()](indexof/README.md) | Determines the first position of a string within another string. | 2 |
| [isGuid()](isguid/README.md) | Determines whether a value is a GUID, or is a string that can be converted to a GUID. | 3 |
| [isInfinite()](isinfinite/README.md) | Determines whether a value is infinite. | 2 |
| [isNaN()](isnan/README.md) | Determines whether a value is not a number. | 3 |
| [isNull()](isnull/README.md) | Determines whether a value is null. | 4 |
| [isNullOrEmpty()](isnullorempty/README.md) | Determines whether a value is null or empty. | 6 |
| [isNullOrWhiteSpace()](isnullorwhitespace/README.md) | Determines whether a value is null, empty or whitespace. | 6 |
| [isSet()](isset/README.md) | Determines whether a parameter is set. | 1 |
| [itemAtIndex()](itematindex/README.md) | Determines the item at the given index. The first index is 0. | 1 |
| [jArray()](jarray/README.md) | Creates a Newtonsoft JArray from input values. | 2 |
| [jObject()](jobject/README.md) | Creates a JObject from key/value pairs. | 1 |
| [jsonArray()](jsonarray/README.md) | Creates a System.Text.Json JsonDocument array from input values. | 3 |
| [jsonDocument()](jsondocument/README.md) | Creates a System.Text.Json JsonDocument from key/value pairs. | 1 |
| [join()](join/README.md) | Joins a list of strings into a single string. | 1 |
| [jPath()](jpath/README.md) | Selects a single value from a JObject using a [JPath](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) expression | 6 |
| [lastIndexOf()](lastindexof/README.md) | Determines the last position of a string within another string. Returns -1 if not present. | 2 |
| [lastOrDefault()](lastordefault/README.md) | Returns the last item in a list that matches a lambda or null if no items match. Note that items are processed in reverse order. | 2 |
| [length()](length/README.md) | Determines length of a string or IList. | 2 |
| [list()](list/README.md) | Emits a List\<object?\> and collapses down lists of lists to a single list. | 3 |
| [listOf()](listof/README.md) | Emits a List\<T\>. | 4 |
| [max()](max/README.md) | Emits the maximum value, ignoring nulls. | 4 |
| [maxValue()](maxvalue/README.md) | Emits the maximum possible value for a given numeric or date/time type. | 5 |
| [min()](min/README.md) | Emits the minimum value, ignoring nulls. | 4 |
| [minValue()](minvalue/README.md) | Emits the minimum possible value for a given numeric or date/time type. | 5 |
| [now()](now/README.md) | Returns the current date and time, with optional timezone correction. | 3 |
| [nullCoalesce()](nullcoalesce/README.md) | Returns the first parameter that is not null, otherwise: null. | 5 |
| [orderBy()](orderby/README.md) | Orders an IEnumerable by one or more lambda expressions. | 4 |
| [padLeft()](padleft/README.md) | Pad the left of a string with a character to a desired string length. | 4 |
| [parse()](parse/README.md) | Returns the conversion of a string to a numeric type. Supported types are: * bool or System.Boolean * sbyte or System.SByte * byte or System.Byte * short or System.Int16 * ushort or System.UInt16 * int or System.Int32 * uint or System.UInt32 * long or System.Int64 * ulong or System.UInt64 * double or System.Double * float or System.Single * decimal or System.Decimal * JArray or Newtonsoft.Json.Linq.JArray (jArray also supported for backaward compatibility) * JObject or Newtonsoft.Json.Linq.JObject (jObject also supported for backaward compatibility) * JsonDocument or System.Text.Json.JsonDocument (jsonDocument also supported) * JsonArray (for System.Text.Json arrays, jsonArray also supported) * Guid | 7 |
| [parseInt()](parseint/README.md) | Returns an integer version of a string. | 1 |
| [regexGroup()](regexgroup/README.md) | Selects a regex group capture | 5 |
| [regexIsMatch()](regexismatch/README.md) | Determine whether a string matches a regex | 2 |
| [replace()](replace/README.md) | Replace a string with another string | 3 |
| [retrieve()](retrieve/README.md) | Retrieves a value from storage | 1 |
| [reverse()](reverse/README.md) | Reverses an IEnumerable. | 1 |
| [sanitize()](sanitize/README.md) | Sanitize a string, replacing any characters outside of the allowed set. | 6 |
| [select()](select/README.md) | Converts an IEnumerable using a lambda. | 2 |
| [selectDistinct()](selectdistinct/README.md) | Converts an IEnumerable using a lambda and removes duplicates. | 1 |
| [setProperties()](setproperties/README.md) | Sets properties on an existing object. | 2 |
| [sha256()](sha256/README.md) | Performs a SHA 256 hash of a string. | 1 |
| [skip()](skip/README.md) | Skips a number of items in a list. | 1 |
| [sort()](sort/README.md) | Sorts an IComparable ascending or descending. | 4 |
| [split()](split/README.md) | Splits a string on a given character into a list of strings. | 2 |
| [startsWith()](startswith/README.md) | Determines whether a string starts with another string. | 2 |
| [store()](store/README.md) | Stores a value for use later in the pipeline | 1 |
| [substring()](substring/README.md) | Retrieves part of a string. If more characters are requested than available at the end of the string, just the available characters are returned. | 5 |
| [sum()](sum/README.md) | Sums numeric items. Optionally, perform a lambda on each one first. | 2 |
| [switch()](switch/README.md) | Return one of a number of values, depending on the input function. | 3 |
| [take()](take/README.md) | Takes a number of items from a list. | 2 |
| [throw()](throw/README.md) | Throws an NCalcExtensionsException. Useful in an if(). | 3 |
| [timeSpan()](timespan/README.md) | Determines the amount of time between two DateTimes. The following units are supported: * Years * Weeks * Days * Hours * Minutes * Seconds * Milliseconds * Any other string is handled with TimeSpan.ToString(timeUnit). See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings | 1 |
| [toDateTime()](todatetime/README.md) | Converts a string to a UTC DateTime. May take an optional inputTimeZone. | 6 |
| [toLower()](tolower/README.md) | Converts a string to lower case. | 1 |
| [toString()](tostring/README.md) | Converts any object to a string | 3 |
| [toUpper()](toupper/README.md) | Converts a string to upper case. | 1 |
| [trim()](trim/README.md) | Removes leading and trailing whitespace. | 1 |
| [try()](try/README.md) | If a function throws an exception, return an alternate value. | 8 |
| [tryParse()](tryparse/README.md) | Returns a boolean result of an attempted cast. | 2 |
| [typeOf()](typeof/README.md) | Determines the C# type of the object. | 4 |
| [where()](where/README.md) | Filters an IEnumerable to bring back only those items that match a condition. | 2 |

