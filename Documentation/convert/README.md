# convert()

| Field | Value |
| --- | --- |
| Purpose | Converts the output of parameter 1 into the result of parameter 2. |
| Parameters | * the value to calculate * destination TimeZone name |
| Notes | Can be used to return an empty string instead of the result of parameter 1, which can be useful when the return value is not useful. The result of parameter 1 is available as the variable "value". |
| Examples | 2 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | convert(anyFunction(), 'XYZ'): 'XYZ' |  |  | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fconvert%2Fexample-01.ncalc) |
| 2 | convert(1 + 1, value + 1): 3 |  |  | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fconvert%2Fexample-02.ncalc) |

