# isNullOrWhiteSpace()

| Field | Value |
| --- | --- |
| Purpose | Determines whether a value is null, empty or whitespace. |
| Parameters | * value |
| Notes | Returns true is the value is: * null; or * it's a JObject and it's type is JTokenType.Null; or * it's a JsonElement and it's ValueKind is JsonValueKind.Null; or * it's a string and it's empty or only contains whitespace characters (\r, \n, \t, or ' '); or * it's a JsonElement string and it's empty or only contains whitespace. |
| Examples | 6 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | isNullOrWhiteSpace(null) | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-01.ncalc) |
| 2 | isNullOrWhiteSpace('') | bool | true | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-02.ncalc) |
| 3 | isNullOrWhiteSpace(' ') | bool | true | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-03.ncalc) |
| 4 | isNullOrWhiteSpace(bob) |  | true if bob is null or whitespace | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-04.ncalc) |
| 5 | isNullOrWhiteSpace(1) | bool | false | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-05.ncalc) |
| 6 | isNullOrWhiteSpace('text') | bool | false | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisnullorwhitespace%2Fexample-06.ncalc) |

