# regexGroup()

| Field | Value |
| --- | --- |
| Purpose | Selects a regex group capture |
| Parameters | * input * regex * zero-based capture index (default: 0) |
| Examples | 5 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | regexGroup('abcdef', '^ab(.+?)f$') | string | 'cde' | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexgroup%2Fexample-01.ncalc) |
| 2 | regexGroup('abcdef', '^ab(.)+f$') | string | 'c' | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexgroup%2Fexample-02.ncalc) |
| 3 | regexGroup('abcdef', '^ab(.)+f$', 1) | string | 'd' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexgroup%2Fexample-03.ncalc) |
| 4 | regexGroup('abcdef', '^ab(.)+f$', 2) | string | 'e' | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexgroup%2Fexample-04.ncalc) |
| 5 | regexGroup('abcdef', '^ab(.)+f$', 10) | object | null | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fregexgroup%2Fexample-05.ncalc) |

