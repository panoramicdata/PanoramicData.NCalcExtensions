# extend()

| Field | Value |
| --- | --- |
| Purpose | Extends an existing object into a JObject with both the original and additional properties. |
| Parameters | * originalObject * listOfAdditionalProperties |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | extend(jObject('a', 1, 'b', null), list('c', 5)) | Newtonsoft.Json.Linq.JObject | JObject with a=1, b=null and c=5 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fextend%2Fexample-01.ncalc) |
| 2 | extend(jObject('a', 1, 'b', null), list('c', null)) | Newtonsoft.Json.Linq.JObject | JObject with a=1, b=null and c=null | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fextend%2Fexample-02.ncalc) |

