# firstOrDefault()

| Field | Value |
| --- | --- |
| Purpose | Returns the first item in a list that matches a lambda or null if no items match. |
| Parameters | * list * predicate * lambda expression as a string |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | firstOrDefault(list(1, 5, 2, 3), 'n', 'n % 2 == 0') | int | 2 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ffirstordefault%2Fexample-01.ncalc) |
| 2 | firstOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0') | object | null | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ffirstordefault%2Fexample-02.ncalc) |

