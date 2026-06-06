# jArray()

| Field | Value |
| --- | --- |
| Purpose | Creates a Newtonsoft JArray from input values. |
| Parameters | * items[] |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | jArray(jObject('a', 1, 'b', null), jObject('a', 2, 'b', #2024-06-21#)) | Newtonsoft.Json.Linq.JArray | JArray [ { "a": 1, "b": null}, { "a": 2, "b": "2024-06-21T00:00:00"} ] | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjarray%2Fexample-01.ncalc) |
| 2 | jArray(1, null, 'woo') | Newtonsoft.Json.Linq.JArray | JArray [ 1, null, "woo" ] | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjarray%2Fexample-02.ncalc) |

