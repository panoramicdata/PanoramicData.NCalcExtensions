# all()

| Field | Value |
| --- | --- |
| Purpose | Returns true if all values match the lambda expression, otherwise false. |
| Parameters | * list - the original list * predicate - (optional) a string to represent the value to be evaluated * nCalcString - (optional, but must be provided if predicate is) the string to evaluate |
| Examples | 6 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | all() | bool | true | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-01.ncalc) |
| 2 | all(list(false, false, false)) | bool | false | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-02.ncalc) |
| 3 | all(list(true, false, true)) | bool | false | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-03.ncalc) |
| 4 | all(list(true, true, true)) | bool | true | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-04.ncalc) |
| 5 | all(list(1, 2, 3, 4, 5), 'n', 'n < 3') | bool | false | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-05.ncalc) |
| 6 | all(list(1, 2, 3, 4, 5), 'n', 'n > 0 && n < 10') | bool | true | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fall%2Fexample-06.ncalc) |

