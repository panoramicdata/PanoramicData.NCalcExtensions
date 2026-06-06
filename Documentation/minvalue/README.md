# minValue()

| Field | Value |
| --- | --- |
| Purpose | Emits the minimum possible value for a given numeric or date/time type. |
| Parameters | * a string representing the type, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double' 'decimal', 'DateTime', or 'DateTimeOffset'. |
| Examples | 5 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | minValue('byte') |  | (byte)0 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fminvalue%2Fexample-01.ncalc) |
| 2 | minValue('ushort') |  | (ushort)0 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fminvalue%2Fexample-02.ncalc) |
| 3 | minValue('int') | int | -2147483648 | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fminvalue%2Fexample-03.ncalc) |
| 4 | minValue('DateTime') | System.DateTime | DateTime.MinValue (0001-01-01T00:00:00.0000000) | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fminvalue%2Fexample-04.ncalc) |
| 5 | minValue('DateTimeOffset') | System.DateTimeOffset | DateTimeOffset.MinValue (0001-01-01T00:00:00.0000000+00:00) | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fminvalue%2Fexample-05.ncalc) |

