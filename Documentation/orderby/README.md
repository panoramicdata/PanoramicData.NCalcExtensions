# orderBy()

| Field | Value |
| --- | --- |
| Purpose | Orders an IEnumerable by one or more lambda expressions. |
| Parameters | * list - the original list * predicate - a string to represent the value to be evaluated * nCalcString1 - the first orderBy lambda expression * nCalcString2 (optional) - the next orderBy lambda expression * nCalcString... (optional) - the next orderBy lambda expression |
| Examples | 4 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | orderBy(list(34, 33, 2, 1), 'n', 'n') | System.Collections.Generic.List<object?> | list(1, 2, 33, 34) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Forderby%2Fexample-01.ncalc) |
| 2 | orderBy(list(34, 33, 2, 1), 'n', '-n') | System.Collections.Generic.List<object?> | list(34, 33, 2, 1) | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Forderby%2Fexample-02.ncalc) |
| 3 | orderBy(list(34, 33, 2, 1), 'n % 32', 'n % 2') | System.Collections.Generic.List<object?> | list(34, 33, 1, 2) | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Forderby%2Fexample-03.ncalc) |
| 4 | orderBy(list(34, 33, 2, 1), 'n % 2', 'n % 32') | System.Collections.Generic.List<object?> | list(33, 1, 34, 2) | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Forderby%2Fexample-04.ncalc) |

