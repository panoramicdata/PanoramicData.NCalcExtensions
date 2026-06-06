# selectDistinct()

| Field | Value |
| --- | --- |
| Purpose | Converts an IEnumerable using a lambda and removes duplicates. |
| Parameters | * list - the original list * predicate - a string to represent the value to be evaluated * nCalcString - the value to evaluate to for each item in the list |
| Examples | 1 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | selectDistinct(list(1, 2, 3, 3, 3), 'n', 'n + 1') | System.Collections.Generic.List<object?> | list(2, 3, 4) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fselectdistinct%2Fexample-01.ncalc) |

