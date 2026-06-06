# replace()

| Field | Value |
| --- | --- |
| Purpose | Replace a string with another string |
| Parameters | * haystackString * needleString * betterNeedleString * ... (optional) more needle/betterNeedle pairs |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | replace('abcdefg', 'cde', 'CDE') | string | 'abCDEfg' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Freplace%2Fexample-01.ncalc) |
| 2 | replace('abcdefg', 'cde', '') | string | 'abfg' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Freplace%2Fexample-02.ncalc) |
| 3 | replace('abcdefg', 'a', '1', 'bc', '23') | string | '123defg' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Freplace%2Fexample-03.ncalc) |

