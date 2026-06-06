# maxValue()

| Field | Value |
| --- | --- |
| Purpose | Emits the maximum possible value for a given numeric or date/time type. |
| Parameters | * a string representing the type, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double', 'decimal', 'DateTime', or 'DateTimeOffset'. |
| Examples | 5 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | maxValue('byte') |  | (byte)255 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmaxvalue%2Fexample-01.ncalc) |
| 2 | maxValue('ushort') |  | (ushort)65535 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmaxvalue%2Fexample-02.ncalc) |
| 3 | maxValue('int') | int | 2147483647 | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmaxvalue%2Fexample-03.ncalc) |
| 4 | maxValue('DateTime') | System.DateTime | DateTime.MaxValue (9999-12-31T23:59:59.9999999) | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmaxvalue%2Fexample-04.ncalc) |
| 5 | maxValue('DateTimeOffset') | System.DateTimeOffset | DateTimeOffset.MaxValue (9999-12-31T23:59:59.9999999+00:00) | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fmaxvalue%2Fexample-05.ncalc) |

