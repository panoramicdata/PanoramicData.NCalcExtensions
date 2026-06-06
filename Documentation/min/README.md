# min()

| Field | Value |
| --- | --- |
| Purpose | Emits the minimum value, ignoring nulls. |
| Parameters | * the list * optionally, a pair of parameters providing a lambda expression to be evaluated. |
| Examples | 4 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | min(listOf('int?', 1, null, 3)) | int | 1 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmin%2Fexample-01.ncalc) |
| 2 | min(listOf('int', 1, 2, 3), 'x', 'x + 1') | int | 2 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmin%2Fexample-02.ncalc) |
| 3 | min(listOf('string', '1', '2', '3')) | string | '1' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmin%2Fexample-03.ncalc) |
| 4 | min(listOf('string', '1', '2', '3'), 'x', 'x + x') | string | '11' | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmin%2Fexample-04.ncalc) |

