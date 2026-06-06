# try()

| Field | Value |
| --- | --- |
| Purpose | If a function throws an exception, return an alternate value. |
| Parameters | * function to attempt * result to return if an exception is thrown (null is returned if this parameter is omitted and an exception is thrown) |
| Examples | 8 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | try(1, 'Failed') | int | 1 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-01.ncalc) |
| 2 | try(throw('Woo')) | object | null | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-02.ncalc) |
| 3 | try(throw('Woo'), 'Failed') | string | 'Failed' | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-03.ncalc) |
| 4 | try(throw('Woo'), exception_message) | string | 'Woo' | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-04.ncalc) |
| 5 | try(throw('Woo'), exception_type) | System.Type | typeof(PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException) | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-05.ncalc) |
| 6 | try(throw('Woo'), exception_typeFullName) | string | 'PanoramicData.NCalcExtensions.Exceptions.NCalcExtensionsException' | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-06.ncalc) |
| 7 | try(throw('Woo'), exception_typeName) | string | 'NCalcExtensionsException' | [example-07.ncalc](example-07.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-07.ncalc) |
| 8 | try(throw('Woo'), exception) |  | The Exception object thrown by the throw function. | [example-08.ncalc](example-08.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Ftry%2Fexample-08.ncalc) |

