# sort()

| Field | Value |
| --- | --- |
| Purpose | Sorts an IComparable ascending or descending. |
| Parameters | * list - the original list * direction (optional) - 'asc' is the default, 'desc' is the other option |
| Examples | 4 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | sort(list(2, 1, 3)) | System.Collections.Generic.List<object?> | list(1, 2, 3) | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsort%2Fexample-01.ncalc) |
| 2 | sort(list(2, 1, 3), 'asc') | System.Collections.Generic.List<object?> | list(1, 2, 3) | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsort%2Fexample-02.ncalc) |
| 3 | sort(list(2, 1, 3), 'desc') | System.Collections.Generic.List<object?> | list(3, 2, 1) | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsort%2Fexample-03.ncalc) |
| 4 | sort(list('b', 'a', 'c'))) | System.Collections.Generic.List<object?> | list('a', 'b', 'c') | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fsort%2Fexample-04.ncalc) |

