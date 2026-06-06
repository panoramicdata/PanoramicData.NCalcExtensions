# substring()

| Field | Value |
| --- | --- |
| Purpose | Retrieves part of a string. If more characters are requested than available at the end of the string, just the available characters are returned. |
| Parameters | * inputString * startIndex * length (optional) |
| Examples | 5 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | substring('haystack', 3) | string | 'stack' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsubstring%2Fexample-01.ncalc) |
| 2 | substring('haystack', 0, 3) | string | 'hay' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsubstring%2Fexample-02.ncalc) |
| 3 | substring('haystack', 3, 100) | string | 'stack' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsubstring%2Fexample-03.ncalc) |
| 4 | substring('haystack', 0, 100) | string | 'haystack' | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsubstring%2Fexample-04.ncalc) |
| 5 | substring('haystack', 0, 0) | string | '' | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsubstring%2Fexample-05.ncalc) |

