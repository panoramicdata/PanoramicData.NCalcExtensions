# any()

| Field | Value |
| --- | --- |
| Purpose | Returns true if any values match the lambda expression, otherwise false. |
| Parameters | * list - the original list * predicate - (optional) a string to represent the value to be evaluated * nCalcString - (optional, but must be provided if predicate is) the string to evaluate |
| Examples | 4 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | any(list()) | bool | false | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fany%2Fexample-01.ncalc) |
| 2 | any(list(1, 2, 3)) | bool | true | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fany%2Fexample-02.ncalc) |
| 3 | any(list(1, 2, 3, 4, 5), 'n', 'n < 3') | bool | true | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fany%2Fexample-03.ncalc) |
| 4 | any(list(1, 2, 3, 4, 5), 'n', 'n > 11') | bool | false | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fany%2Fexample-04.ncalc) |

