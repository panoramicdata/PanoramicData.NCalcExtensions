# take()

| Field | Value |
| --- | --- |
| Purpose | Takes a number of items from a list. |
| Parameters | * the list to take from * the number of items to take |
| Notes | If a number is provided that is longer than the list, the full list is returned. |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | take(list(1, 2, 3), 2): list(1, 2) |  |  | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftake%2Fexample-01.ncalc) |
| 2 | take(list(1, 2, 3), 10): list(1, 2, 3) |  |  | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftake%2Fexample-02.ncalc) |

