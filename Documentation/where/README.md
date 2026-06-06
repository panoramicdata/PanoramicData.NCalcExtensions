# where()

| Field | Value |
| --- | --- |
| Purpose | Filters an IEnumerable to bring back only those items that match a condition. |
| Parameters | * list - the original list * predicate - a string to represent the value to be evaluated * nCalcString - the string to evaluate |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | where(list(1, 2, 3, 4, 5), 'n', 'n < 3') | System.Collections.Generic.List<object?> | list(1, 2) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fwhere%2Fexample-01.ncalc) |
| 2 | where(list(1, 2, 3, 4, 5), 'n', 'n < 3 \|\| n > 4') | System.Collections.Generic.List<object?> | list(1, 2, 5) | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fwhere%2Fexample-02.ncalc) |

