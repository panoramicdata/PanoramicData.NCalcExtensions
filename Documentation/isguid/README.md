# isGuid()

| Field | Value |
| --- | --- |
| Purpose | Determines whether a value is a GUID, or is a string that can be converted to a GUID. |
| Parameters | * value |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | isGuid('9384EF0Z-38AD-4E8E-A24E-0ACD3273A401') | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisguid%2Fexample-01.ncalc) |
| 2 | isGuid('{9384EF0Z-38AD-4E8E-A24E-0ACD3273A401}') | bool | true | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisguid%2Fexample-02.ncalc) |
| 3 | isGuid('abc') | bool | false | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fisguid%2Fexample-03.ncalc) |

