# isNull()

| Field | Value |
| --- | --- |
| Purpose | Determines whether a value is null. |
| Parameters | * value |
| Notes | Returns true if the value is: * null; or * it's a JObject and it's type is JTokenType.Null; or * it's a JsonElement and it's ValueKind is JsonValueKind.Null. |
| Examples | 4 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | isNull(1) | bool | false | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnull%2Fexample-01.ncalc) |
| 2 | isNull('text') | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnull%2Fexample-02.ncalc) |
| 3 | isNull(bob) |  | true if bob is null | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnull%2Fexample-03.ncalc) |
| 4 | isNull(null) | bool | true | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnull%2Fexample-04.ncalc) |

