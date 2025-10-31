# 100% Code Coverage Plan
## PanoramicData.NCalcExtensions

**Target:** 100% Line Coverage, 100% Branch Coverage  
**Current:** 78.2% Line Coverage, 71.7% Branch Coverage  
**Timeline:** 3-4 weeks  
**Status:** ?? In Progress

---

## ?? Coverage Milestones

| Milestone | Line Coverage | Branch Coverage | Target Date | Status |
|-----------|---------------|-----------------|-------------|--------|
| **Phase 0** (Current) | 78.2% | 71.7% | Oct 31, 2025 | ? Complete |
| **Phase 1** (Critical) | 85%+ | 75%+ | Nov 7, 2025 | ?? In Progress |
| **Phase 2** (Improved) | 92%+ | 85%+ | Nov 14, 2025 | ? Pending |
| **Phase 3** (Complete) | 100% | 100% | Nov 21, 2025 | ? Pending |

---

## ?? Phase 1: Fix Critical Gaps (Week 1)

### Goal: Achieve 85% Line Coverage, 75% Branch Coverage

### Day 1-2: Zero Coverage Classes (HIGH PRIORITY)

#### Task 1.1: ListHelpers Tests
**File:** `PanoramicData.NCalcExtensions.Test/ListHelpersTests.cs` (NEW)  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
public class ListHelpersTests : NCalcTest
{
    [Fact]
    public void Collapse_EmptyList_ReturnsEmpty()
    {
        var list = new List<object?>();
        var result = list.Collapse();
        result.Should().BeEmpty();
    }

    [Fact]
    public void Collapse_FlatList_ReturnsSame()
    {
      var list = new List<object?> { 1, "test", 3.14 };
        var result = list.Collapse();
        result.Should().BeEquivalentTo(new List<object?> { 1, "test", 3.14 });
    }

    [Fact]
  public void Collapse_SingleNestedList_Flattens()
{
        var inner = new List<object?> { 1, 2, 3 };
        var list = new List<object?> { inner };
var result = list.Collapse();
        result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3 });
    }

    [Fact]
  public void Collapse_MultipleNestedLists_Flattens()
    {
     var list1 = new List<object?> { 1, 2 };
        var list2 = new List<object?> { 3, 4 };
        var list = new List<object?> { list1, list2 };
     var result = list.Collapse();
        result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3, 4 });
    }

    [Fact]
    public void Collapse_DeeplyNested_CollapsesAll()
    {
        var inner3 = new List<object?> { 1, 2 };
 var inner2 = new List<object?> { inner3 };
        var inner1 = new List<object?> { inner2 };
        var list = new List<object?> { inner1 };
var result = list.Collapse();
   result.Should().BeEquivalentTo(new List<object?> { 1, 2 });
    }

    [Fact]
    public void Collapse_MixedTypes_PreservesNonLists()
    {
        var inner = new List<object?> { 1, 2 };
        var list = new List<object?> { inner, "string", 3 };
        var result = list.Collapse();
     // Should not collapse since not all items are lists
        result.Should().BeEquivalentTo(new List<object?> { inner, "string", 3 });
    }
}
```

#### Task 1.2: LambdaFunction Tests
**File:** `PanoramicData.NCalcExtensions.Test/LambdaFunctionTests.cs` (NEW)  
**Estimated Time:** 45 minutes  
**Coverage Impact:** +0.5%

**Status:** ? NOT IMPLEMENTED - LambdaFunction appears to be unused/internal

#### Task 1.3: LastIndexOf Tests
**File:** Extend `PanoramicData.NCalcExtensions.Test/LastIndexOfTests.cs`  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
public class LastIndexOfTests : NCalcTest
{
    [Theory]
    [InlineData("lastIndexOf('abcdefabc', 'abc')", 6)]
    [InlineData("lastIndexOf('abcdefabc', 'def')", 3)]
    [InlineData("lastIndexOf('hello', 'l')", 3)]
    public void LastIndexOf_Found_ReturnsCorrectIndex(string expression, int expected)
        => Test(expression).Should().Be(expected);

    [Theory]
    [InlineData("lastIndexOf('abcdef', 'xyz')", -1)]
    [InlineData("lastIndexOf('hello', 'world')", -1)]
    public void LastIndexOf_NotFound_ReturnsMinusOne(string expression, int expected)
        => Test(expression).Should().Be(expected);

    [Theory]
    [InlineData("lastIndexOf('aaa', 'a')", 2)]
  [InlineData("lastIndexOf('test test test', 'test')", 10)]
    public void LastIndexOf_MultipleOccurrences_ReturnsLast(string expression, int expected)
        => Test(expression).Should().Be(expected);

    [Theory]
    [InlineData("lastIndexOf('', 'a')", -1)]
    [InlineData("lastIndexOf('test', '')", 4)] // .NET behavior
    public void LastIndexOf_EmptyStrings_HandlesCorrectly(string expression, int expected)
        => Test(expression).Should().Be(expected);

    [Fact]
    public void LastIndexOf_NullParameters_ThrowsException()
  {
        new ExtendedExpression("lastIndexOf(null, 'test')")
     .Invoking(e => e.Evaluate())
 .Should()
  .Throw<FormatException>();
    }
}
```

#### Task 1.4: TypeHelper Tests
**File:** `PanoramicData.NCalcExtensions.Test/TypeHelperTests.cs` (NEW)  
**Estimated Time:** 1 hour  
**Coverage Impact:** +1%

```csharp
public class TypeHelperTests
{
    [Theory]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(int?), "int?")]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(List<int>), "List<int>")]
    [InlineData(typeof(Dictionary<string, object>), "Dictionary<string, object>")]
    public void AsHumanString_VariousTypes_ReturnsReadable(Type type, string expected)
    {
   var result = TypeHelper.AsHumanString(type);
     result.Should().Be(expected);
    }

    [Fact]
    public void AsHumanString_GenericType_ReturnsReadable()
 {
        var result = TypeHelper.AsHumanString<List<string>>();
        result.Should().Be("List<string>");
    }

  [Fact]
    public void AsHumanString_NullableValueType_IncludesNullableSymbol()
    {
        var result = TypeHelper.AsHumanString<DateTime?>();
        result.Should().Be("DateTime?");
    }
}
```

**Phase 1 Total:** +2.5% coverage (78.2% ? 80.7%)

---

### Day 3-4: Low Coverage Areas (50-65%)

#### Task 1.5: Max/Min Edge Cases
**File:** `PanoramicData.NCalcExtensions.Test/MaxMinTests.cs`  
**Estimated Time:** 1 hour  
**Coverage Impact:** +1%

```csharp
// Add tests for:
- Max with empty list
- Max with single item
- Max with all nulls
- Max with mixed types
- Min with empty list
- Min with single item
- Min with all nulls
- Min with mixed types
```

#### Task 1.6: Sum Edge Cases
**File:** `PanoramicData.NCalcExtensions.Test/SumTests.cs`  
**Estimated Time:** 1 hour  
**Coverage Impact:** +1%

```csharp
// Add tests for:
- Sum with empty list
- Sum with all zeros
- Sum with negative numbers
- Sum with very large numbers
- Sum with JValue types
- Sum with lambda on empty list
- Sum with lambda returning null
```

#### Task 1.7: If Conditional Branches
**File:** `PanoramicData.NCalcExtensions.Test/IfTests.cs`
**Estimated Time:** 45 minutes  
**Coverage Impact:** +0.8%

```csharp
// Add tests for:
- If with non-boolean first parameter
- If with null parameters
- If with exception in true branch
- If with exception in false branch
- If with wrong parameter count
```

**Phase 1 Total:** +5% coverage (80.7% ? 85.7%)

---

## ?? Phase 2: Comprehensive Coverage (Week 2)

### Goal: Achieve 92% Line Coverage, 85% Branch Coverage

### Day 1-2: Medium Priority Classes

#### Task 2.1: CountBy Scenarios
**Estimated Time:** 1.5 hours  
**Coverage Impact:** +1%

#### Task 2.2: Parameters Handling
**Estimated Time:** 2 hours  
**Coverage Impact:** +2%

#### Task 2.3: DateTime Functions
**Estimated Time:** 1 hour  
**Coverage Impact:** +1%

### Day 3-5: Branch Coverage

#### Task 2.4: Error Path Testing
**Estimated Time:** 3 hours  
**Coverage Impact:** +3%

- Test all exception paths
- Test all null handling
- Test all type validation
- Test all parameter validation

**Phase 2 Total:** +7% coverage (85.7% ? 92.7%)

---

## ?? Phase 3: Perfect Coverage (Week 3-4)

### Goal: Achieve 100% Line Coverage, 100% Branch Coverage

### Day 1-5: Edge Cases & Integration

#### Task 3.1: Substring Boundary Conditions
**Estimated Time:** 1 hour  
**Coverage Impact:** +0.5%

#### Task 3.2: ListOf Type Variations
**Estimated Time:** 1 hour  
**Coverage Impact:** +0.5%

#### Task 3.3: ToString Format Variations
**Estimated Time:** 1 hour  
**Coverage Impact:** +0.5%

#### Task 3.4: Humanize Time Units
**Estimated Time:** 1 hour  
**Coverage Impact:** +0.5%

#### Task 3.5: Complex Integration Tests
**Estimated Time:** 4 hours  
**Coverage Impact:** +2%

#### Task 3.6: Final Coverage Sweep
**Estimated Time:** 3 hours  
**Coverage Impact:** +3%

- Review coverage report
- Identify remaining uncovered lines
- Add targeted tests
- Achieve 100% coverage

**Phase 3 Total:** +7.3% coverage (92.7% ? 100%)

---

## ??? Coverage Enforcement

### Publish Script Integration

The `Publish.ps1` script will be updated to:

1. **Run Tests with Coverage** before publishing
2. **Check Coverage Threshold** (90% minimum)
3. **Block Publish** if coverage is below threshold
4. **Allow Override** with `--force` parameter

```powershell
# Example usage:
.\Publish.ps1        # Requires 90% coverage
.\Publish.ps1 -Force             # Skips coverage check
.\Publish.ps1 -MinCoverage 95    # Custom threshold
```

### CI/CD Pipeline

Add to GitHub Actions / Azure DevOps:

```yaml
- name: Test with Coverage
  run: dotnet test --collect:"XPlat Code Coverage"

- name: Check Coverage
  run: |
    dotnet tool install --global dotnet-reportgenerator-globaltool
    reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage"
    # Parse coverage and fail if < 90%
```

---

## ?? Daily Checklist

### Before Starting Work
- [ ] Pull latest changes
- [ ] Run all existing tests
- [ ] Check current coverage baseline

### During Development
- [ ] Write test first (TDD)
- [ ] Implement feature/fix
- [ ] Run tests locally
- [ ] Check coverage impact

### Before Committing
- [ ] All tests pass (931/933 minimum)
- [ ] Coverage increased or maintained
- [ ] No new warnings
- [ ] Code reviewed

### Before Publishing
- [ ] Coverage ? 90% (or use --force)
- [ ] All tests pass
- [ ] Documentation updated
- [ ] CHANGELOG updated

---

## ?? Progress Tracking

### Week 1 Progress
- [ ] ListHelpers: 0% ? 100%
- [ ] LastIndexOf: 0% ? 100%
- [ ] TypeHelper: 0% ? 100%
- [ ] Max: 50.5% ? 90%+
- [ ] Min: 50.5% ? 90%+
- [ ] Sum: 59% ? 85%+
- [ ] If: 62.9% ? 85%+
- **Target:** 85% line coverage

### Week 2 Progress
- [ ] CountBy: 56.2% ? 90%+
- [ ] Parameters: 16% ? 90%+
- [ ] DateTime functions: 63.6% ? 90%+
- [ ] Branch coverage: 71.7% ? 85%+
- **Target:** 92% line coverage

### Week 3-4 Progress
- [ ] All remaining classes: 90%+
- [ ] Integration tests added
- [ ] Edge cases covered
- [ ] **Target:** 100% coverage

---

## ?? Success Criteria

### Minimum Requirements (Publish Allowed)
? Line Coverage: ? 90%  
? Branch Coverage: ? 85%  
? Tests Passing: ? 99%  
? Zero Warnings  
? All critical paths tested

### Ideal Requirements (Gold Standard)
?? Line Coverage: 100%  
?? Branch Coverage: 100%  
?? Tests Passing: 100%  
?? Zero Warnings  
?? Zero Technical Debt

---

## ?? Blocking Issues

### Publish Will Be Blocked If:
1. Line coverage < 90% (without --force)
2. Tests failing > 2 (excluding known NCalc bugs)
3. New warnings introduced
4. Coverage decreased from previous version

### Force Override Required For:
1. Hotfix deployments
2. Coverage temporarily decreased due to refactoring
3. Documented and approved coverage exceptions
4. Emergency bug fixes

---

## ?? Coverage Monitoring

### Automated Reports
- **Daily:** Coverage trend email
- **Weekly:** Detailed coverage analysis
- **Monthly:** Coverage quality review

### Coverage Badges
Add to README.md:
```markdown
![Coverage](https://img.shields.io/badge/coverage-78.2%25-yellow)
```

Target badges:
```markdown
![Coverage](https://img.shields.io/badge/coverage-90%25-brightgreen)
![Coverage](https://img.shields.io/badge/coverage-100%25-brightgreen)
```

---

## ?? Tools & Commands

### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory:./TestResults
```

### Generate Coverage Report
```bash
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:"Html;TextSummary"
```

### View Coverage
```bash
start CoverageReport/index.html
```

### Check Coverage Threshold
```powershell
# In Publish.ps1
$coverage = Get-CoveragePercentage
if ($coverage -lt 90 -and -not $Force) {
    Write-Error "Coverage is $coverage%, minimum is 90%. Use -Force to override."
    exit 1
}
```

---

## ?? Support & Resources

### Documentation
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator Documentation](https://github.com/danielpalme/ReportGenerator)
- [xUnit Documentation](https://xunit.net/)

### Team Contacts
- **Coverage Champion:** TBD
- **Test Lead:** TBD
- **CI/CD Owner:** TBD

---

## ? Acceptance Criteria

This plan is considered complete when:

1. ? Line coverage reaches 100%
2. ? Branch coverage reaches 100%
3. ? Publish script enforces 90% minimum
4. ? CI/CD pipeline includes coverage checks
5. ? Coverage badges added to README
6. ? All team members trained on coverage requirements
7. ? Coverage monitoring automated
8. ? Documentation updated

---

**Plan Created:** October 31, 2025  
**Plan Owner:** Development Team  
**Next Review:** November 7, 2025  
**Status:** ?? Active
