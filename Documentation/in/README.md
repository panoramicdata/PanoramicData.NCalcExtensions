# in()

| Field | Value |
| --- | --- |
| Purpose | Determines whether a value (the first parameter) is in a set of other values (the remaining parameters). |
| Parameters | * item * list |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it') | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fin%2Fexample-01.ncalc) |
| 2 | in('needle', 'haystack', 'with', 'only', 'hay') | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fin%2Fexample-02.ncalc) |

