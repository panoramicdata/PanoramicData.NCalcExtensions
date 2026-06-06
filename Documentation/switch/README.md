# switch()

| Field | Value |
| --- | --- |
| Purpose | Return one of a number of values, depending on the input function. |
| Parameters | * switched value * a set of pairs: case_n, output_n * if present, a final value can be used as a default. If the default WOULD have been returned, but no default is present, an exception is thrown. |
| Examples | 3 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | switch('yes', 'yes', 1, 'no', 2) | int | 1 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fswitch%2Fexample-01.ncalc) |
| 2 | switch('blah', 'yes', 1, 'no', 2) |  | throws exception | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fswitch%2Fexample-02.ncalc) |
| 3 | switch('blah', 'yes', 1, 'no', 2, 3) | int | 3 | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fswitch%2Fexample-03.ncalc) |

