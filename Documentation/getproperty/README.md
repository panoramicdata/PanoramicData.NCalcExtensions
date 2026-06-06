# getProperty()

| Field | Value |
| --- | --- |
| Purpose | Gets an object's property. |
| Parameters | * sourceObject * propertyName |
| Notes | Supports JObject, JsonDocument, JsonElement, Dictionary<string, object?>, and regular .NET objects. |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | getProperty(toDateTime('2019-01-01', 'yyyy-MM-dd'), 'Year') |  | 2019 (int) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fgetproperty%2Fexample-01.ncalc) |
| 2 | getProperty(jsonDocument('name', 'John', 'age', 30), 'name') | string | 'John' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fgetproperty%2Fexample-02.ncalc) |

