# getProperties()

| Field | Value |
| --- | --- |
| Purpose | Gets a list of an object's properties. |
| Parameters | * sourceObject |
| Notes | Supports JObject, JsonDocument, JsonElement, and regular .NET objects. |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | getProperties(parse('jObject', '{ "A": 1, "B": 2 }')) |  | [ 'A', 'B' ] | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fgetproperties%2Fexample-01.ncalc) |
| 2 | getProperties(jsonDocument('name', 'John', 'age', 30)) |  | [ 'name', 'age' ] | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fgetproperties%2Fexample-02.ncalc) |
| 3 | getProperties(toDateTime('2019-01-01', 'yyyy-MM-dd')) |  | [ 'Date', 'Day', 'DayOfWeek', 'DayOfYear', 'Hour', 'Kind', 'Millisecond', 'Microsecond', 'Nanosecond', 'Minute', 'Month', 'Now', 'Second', 'Ticks', 'TimeOfDay', 'Today', 'Year', 'UtcNow' ] | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fgetproperties%2Fexample-03.ncalc) |

