# jsonDocument()

| Field | Value |
| --- | --- |
| Purpose | Creates a System.Text.Json JsonDocument from key/value pairs. |
| Parameters | * key1 (string) * value1 * key2 (string) * value2 * ... * keyN * valueN |
| Examples | 1 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | jsonDocument('a', 1, 'b', null) | System.Text.Json.JsonDocument | JsonDocument with properties a=1 and b=null | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjsondocument%2Fexample-01.ncalc) |

