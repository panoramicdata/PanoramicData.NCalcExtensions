# split()

| Field | Value |
| --- | --- |
| Purpose | Splits a string on a given character into a list of strings. |
| Parameters | * longString * string to split on |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | split('a bc d', ' ') | System.Collections.Generic.List<object?> | list('a', 'bc', 'd') | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsplit%2Fexample-01.ncalc) |
| 2 | split('aXXbcXXd', 'XX') | System.Collections.Generic.List<object?> | list('a', 'bc', 'd') | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsplit%2Fexample-02.ncalc) |

