# jObject()

| Field | Value |
| --- | --- |
| Purpose | Creates a JObject from key/value pairs. |
| Parameters | * key1 (string) * value1 * key2 (string) * value2 * ... * keyN * valueN |
| Examples | 1 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | jObject('a', 1, 'b', null) | Newtonsoft.Json.Linq.JObject | JObject{ "a": 1, "b": null} | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjobject%2Fexample-01.ncalc) |

