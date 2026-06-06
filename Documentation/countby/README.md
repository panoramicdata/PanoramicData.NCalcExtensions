# countBy()

| Field | Value |
| --- | --- |
| Purpose | Counts the number of items, grouped by a calculation. |
| Parameters | * list - the original list * predicate - a string to represent the value to be evaluated * nCalcString - the string to evaluate. Must emit a string containing one or more characters: A-Z, a-z, 0-9 or _. |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))') |  | { 'false': 1, 'true': 6 } | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcountby%2Fexample-01.ncalc) |
| 2 | countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)') |  | { '1': 1, '2': 2, '3', 3, '4', '1' } | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcountby%2Fexample-02.ncalc) |

