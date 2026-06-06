# select()

| Field | Value |
| --- | --- |
| Purpose | Converts an IEnumerable using a lambda. |
| Parameters | * list - the original list * predicate - a string to represent the value to be evaluated * nCalcString - the value to evaluate to for each item in the list * output list type - outputs a list of the specified type (optional) |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | select(list(1, 2, 3, 4, 5), 'n', 'n + 1') | System.Collections.Generic.List<object?> | list(2, 3, 4, 5, 6) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fselect%2Fexample-01.ncalc) |
| 2 | select(list(jObject('a', 1, 'b', '2'), jObject('a', 3, 'b', '4')), 'n', 'n', 'JObject') |  | list of JObjects | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fselect%2Fexample-02.ncalc) |

