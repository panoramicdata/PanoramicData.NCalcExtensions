# count()

| Field | Value |
| --- | --- |
| Purpose | Counts the number of items. Optionally, only count those that match a lambda. |
| Parameters | * list - the original list * predicate (optional) - a string to represent the value to be evaluated * nCalcString (optional) - the string to evaluate |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | count('a piece of string') | int | 17 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcount%2Fexample-01.ncalc) |
| 2 | count(list(1, 2, 3, 4, 5)) | int | 5 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcount%2Fexample-02.ncalc) |
| 3 | count(list(1, 2, 3, 4, 5), 'n', 'n > 3') | int | 2 | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcount%2Fexample-03.ncalc) |

