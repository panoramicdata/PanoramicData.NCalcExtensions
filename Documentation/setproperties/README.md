# setProperties()

| Field | Value |
| --- | --- |
| Purpose | Sets properties on an existing object. |
| Parameters | * object - the original object * property1 - the first new property name * value1 - the first new property value * propertyN (optional) - the nth new property name * valueN (optional) - the nth new property value |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | setProperties(jObject('a', 1, 'b', null), 'c', 'X') | Newtonsoft.Json.Linq.JObject | jObject('a', 1, 'b', null, 'c', 'X') | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsetproperties%2Fexample-01.ncalc) |
| 2 | setProperties(jObject('a', 1, 'b', null), 'c', 'X', 'd', 'Y') | Newtonsoft.Json.Linq.JObject | jObject('a', 1, 'b', null, 'c', 'X', 'd', 'Y') | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsetproperties%2Fexample-02.ncalc) |

