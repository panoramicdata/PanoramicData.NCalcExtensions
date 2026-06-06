# jsonArray()

| Field | Value |
| --- | --- |
| Purpose | Creates a System.Text.Json JsonDocument array from input values. |
| Parameters | * items[] |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | jsonArray(1, 'test', null, true) | System.Text.Json.JsonDocument | JsonDocument array containing [1, "test", null, true] | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjsonarray%2Fexample-01.ncalc) |
| 2 | jsonArray() |  | Empty JsonDocument array | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjsonarray%2Fexample-02.ncalc) |
| 3 | jsonArray(jsonDocument('a', 1), jsonDocument('b', 2)) | System.Text.Json.JsonDocument | JsonDocument array containing two objects | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fjsonarray%2Fexample-03.ncalc) |

