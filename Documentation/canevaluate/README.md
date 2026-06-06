# canEvaluate()

| Field | Value |
| --- | --- |
| Purpose | Determines whether ALL of the parameters can be evaluated. This can be used, for example, to test whether a parameter is set. |
| Parameters | * parameter1, parameter2, ... |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | canEvaluate(nonExistent) | bool | false | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcanevaluate%2Fexample-01.ncalc) |
| 2 | canEvaluate(1) | bool | true | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fcanevaluate%2Fexample-02.ncalc) |

