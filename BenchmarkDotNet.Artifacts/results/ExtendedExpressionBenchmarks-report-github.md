```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.300-preview.0.26177.108
  [Host]   : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  ShortRun : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                                               | Mean       | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|----------------------------------------------------- |-----------:|-----------:|----------:|-------:|-------:|----------:|
| Construct_ExtendedExpression                         | 3,394.9 ns | 2,033.0 ns | 111.44 ns | 0.8163 |      - |    6856 B |
| Construct_And_Evaluate_ExtendedExpression            | 4,177.4 ns | 7,588.0 ns | 415.92 ns | 0.8659 | 0.0038 |    7264 B |
| Parse_Document_And_Evaluate_SimpleExtendedExpression | 3,611.7 ns | 5,663.5 ns | 310.43 ns | 0.8392 |      - |    7048 B |
| Evaluate_ExtendedExpression_Reused                   |   117.2 ns |   147.8 ns |   8.10 ns | 0.0488 |      - |     408 B |
| Evaluate_SimpleExtendedExpression_Reused             |   122.2 ns |   124.4 ns |   6.82 ns | 0.0488 |      - |     408 B |
