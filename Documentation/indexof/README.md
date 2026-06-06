# indexOf()

| Field | Value |
| --- | --- |
| Purpose | Determines the first position of a string within another string. |
| Parameters | * longString * shortString |
| Notes | The first character is position 0. Returns -1 if not present. |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | indexOf('#abcabc#', 'abc') | int | 1 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Findexof%2Fexample-01.ncalc) |
| 2 | indexOf('#abcabc#', 'abcd') | int | -1 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Findexof%2Fexample-02.ncalc) |

