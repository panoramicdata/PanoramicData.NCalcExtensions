# parse()

| Field | Value |
| --- | --- |
| Purpose | Returns the conversion of a string to a numeric type. Supported types are: * bool or System.Boolean * sbyte or System.SByte * byte or System.Byte * short or System.Int16 * ushort or System.UInt16 * int or System.Int32 * uint or System.UInt32 * long or System.Int64 * ulong or System.UInt64 * double or System.Double * float or System.Single * decimal or System.Decimal * JArray or Newtonsoft.Json.Linq.JArray (jArray also supported for backaward compatibility) * JObject or Newtonsoft.Json.Linq.JObject (jObject also supported for backaward compatibility) * JsonDocument or System.Text.Json.JsonDocument (jsonDocument also supported) * JsonArray (for System.Text.Json arrays, jsonArray also supported) * Guid |
| Parameters | * type (see above) * text * valueIfParseFails (optional) |
| Examples | 7 |

## Examples

| # | Example | Return type | Expected | .ncalc | NCalc101 |
| ---: | --- | --- | --- | --- | --- |
| 1 | parse('int', '1') | int | 1 | [example-01.ncalc](example-01.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-01.ncalc) |
| 2 | parse('System.Int32', '1') | int | 1 | [example-02.ncalc](example-02.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-02.ncalc) |
| 3 | parse('bool', 'x', null) | object | null | [example-03.ncalc](example-03.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-03.ncalc) |
| 4 | parse('jObject', '{ "a" |  | 1 }', null) : JObject { "a": 1 } | [example-04.ncalc](example-04.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-04.ncalc) |
| 5 | parse('jArray', '[ { "a" |  | 1 } ]', null) : JArray [{ "a": 1 }] | [example-05.ncalc](example-05.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-05.ncalc) |
| 6 | parse('JsonDocument', '{ "a" |  | 1 }', null) : JsonDocument { "a": 1 } | [example-06.ncalc](example-06.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-06.ncalc) |
| 7 | parse('JsonArray', '[ 1, 2, 3 ]', null) | System.Text.Json.JsonDocument | JsonDocument array [1, 2, 3] | [example-07.ncalc](example-07.ncalc) | [Open example](https://ncalc101.magicsuite.net/?url=https%3A%2F%2Fraw.githubusercontent.com%2Fpanoramicdata%2FPanoramicData.NCalcExtensions%2Fmain%2FDocumentation%2Fparse%2Fexample-07.ncalc) |

