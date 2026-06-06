# sum()

| Field | Value |
| --- | --- |
| Purpose | Sums numeric items. Optionally, perform a lambda on each one first. |
| Parameters | * list - the original list * predicate (optional) - a string to represent the value to be evaluated * nCalcString (optional) - the string to evaluate |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | sum(list(1, 2, 3)) | int | 6 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsum%2Fexample-01.ncalc) |
| 2 | sum(list(1, 2, 3), 'n', 'n * n') | int | 14 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsum%2Fexample-02.ncalc) |

