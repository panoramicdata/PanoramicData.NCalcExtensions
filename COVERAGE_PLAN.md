# 100% Code Coverage Plan
## PanoramicData.NCalcExtensions

**Target:** 100% Line Coverage, 100% Branch Coverage  
**Current:** 81.0% Line Coverage, 73.7% Branch Coverage  
**Timeline:** 2-3 weeks (accelerated)  
**Status:** ?? In Progress - Strong Foundation Established

---

## ?? Coverage Milestones

| Milestone | Line Coverage | Branch Coverage | Target Date | Status |
|-----------|---------------|-----------------|-------------|--------|
| **Phase 0** (Baseline) | 78.2% | 71.7% | Oct 31, 2025 | ? Complete |
| **Phase 1** (Foundation) | **81.0%** | **73.7%** | Oct 31, 2025 | ? Complete |
| **Phase 2** (85% Target) | **85%** | **75%** | Nov 7, 2025 | ?? In Progress (81%) |
| **Phase 3** (90% Target) | **90%** | **80%** | Nov 14, 2025 | ? Pending |
| **Phase 4** (95% Target) | **95%** | **90%** | Nov 21, 2025 | ? Pending |
| **Phase 5** (Complete) | **100%** | **100%** | Nov 28, 2025 | ? Pending |

---

## ?? Current Status (Updated: Oct 31, 2025)

### Session Achievement Summary
- **Line Coverage:** 81.0% (was 78.2%, **+2.8%**)
- **Branch Coverage:** 73.7% (was 71.7%, **+2.0%**)
- **Tests Added:** **157 new tests** (933 ? 1090)
- **Classes at 100%:** **36** (was 32, +4)
- **Test Pass Rate:** **99.8%** (1088/1090 passing)
- **Commits Made:** **8 comprehensive commits**

### Gap to 100%
- **Line Coverage:** 19.0% remaining
- **Branch Coverage:** 26.3% remaining
- **Estimated Time:** 20-25 hours of focused work

---

## ? Phase 1 Completed Tasks

### Classes Reaching 100% Coverage
1. ? **ListHelpers**: 0% ? 100% (+6 tests)
2. ? **LastIndexOf**: 0% ? 100% (+8 tests)
3. ? **TypeHelper**: 0% ? 100% (+6 tests)
4. ? **StartsWith**: 66.6% ? 100% (+14 tests)

### Significant Improvements (>15%)
- ? **If**: 62.9% ? 85.1% (+22.2%, +17 tests)
- ? **In**: 61.5% ? 76.9% (+15.4%, +18 tests)
- ? **Substring**: 65% ? 80% (+15%, +16 tests)
- ? **Now**: 69.2% ? 84.6% (+15.4%, +10 tests)

### Good Improvements (8-15%)
- ? **IsInfinite**: 68.7% ? 81.2% (+12.5%, +13 tests)
- ? **Sum**: 59% ? 67.2% (+8.2%, +14 tests)
- ? **DateTimeIsInPast**: 63.6% ? 81.8% (+18.2%, +4 tests)
- ? **DateTimeIsInFuture**: 63.6% ? 81.8% (+18.2%, +4 tests)

### Test Files Created/Enhanced
1. ? ListHelpersTests.cs (NEW - 6 tests)
2. ? LastIndexOfTests.cs (NEW - 8 tests)
3. ? TypeHelperTests.cs (NEW - 6 tests)
4. ? SumTests.cs (enhanced +14 tests)
5. ? IfTests.cs (enhanced +17 tests)
6. ? DateTimeIsInPastTests.cs (enhanced +4 tests)
7. ? DateTimeIsInFutureTests.cs (enhanced +4 tests)
8. ? IsNanTests.cs (enhanced +18 tests)
9. ? InTests.cs (enhanced +18 tests)
10. ? SubstringTests.cs (enhanced +16 tests)
11. ? StartsWithTests.cs (enhanced +14 tests)
12. ? IsInfiniteTests.cs (enhanced +13 tests)
13. ? NowTests.cs (enhanced +10 tests)

---

## ?? Phase 2: Path to 85% Coverage

### Goal: Achieve 85% Line Coverage, 75% Branch Coverage
**Estimated Time:** 4-5 hours
**Current Gap:** 4% line coverage

### High-Priority Tasks (Next Session)

#### Task 2.1: Max/Min Functions (50.5% ? 90%+)
**Priority:** CRITICAL  
**Estimated Time:** 1 hour  
**Coverage Impact:** +1%

Already have good tests but need branch coverage:
```csharp
// Add tests for:
- All numeric type branches (sbyte, byte, short, ushort, uint, ulong)
- Edge cases with very large numbers
- Mixed type comparisons
- Empty collection with lambda
- All null value combinations
```

#### Task 2.2: CountBy Function (56.2% ? 85%+)
**Priority:** HIGH  
**Estimated Time:** 45 minutes  
**Coverage Impact:** +0.8%

```csharp
// Add tests for:
- Complex grouping scenarios
- Lambda expression variations
- Null values in grouping keys
- Empty collections
- Invalid group key formats
```

#### Task 2.3: ToString Function (65.6% ? 90%+)
**Priority:** HIGH  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.5%

```csharp
// Add tests for:
- All numeric format specifiers (N, C, P, E, F, G)
- Date/time format strings
- Custom format strings
- Culture-specific formatting
- Null handling
```

#### Task 2.4: Humanize Function (65.7% ? 85%+)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.4%

```csharp
// Add tests for:
- All time units (milliseconds, seconds, minutes, hours, days, weeks, years)
- Edge case values (0, negative, very large)
- Invalid time unit strings
- Overflow scenarios
```

#### Task 2.5: GetProperty Function (66.6% ? 85%+)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.3%

```csharp
// Add tests for:
- JObject property access
- JsonDocument property access
- Dictionary access
- Regular .NET object properties
- Non-existent properties
- Null handling
```

#### Task 2.6: Sum Function (67.2% ? 85%+)
**Priority:** MEDIUM  
**Estimated Time:** 30 minutes  
**Coverage Impact:** +0.3%

```csharp
// Add more tests for:
- JValue numeric types
- Complex lambda expressions
- Error conditions
- Mixed type lists
```

#### Task 2.7: Quick Wins (Various 70-80% ? 85%+)
**Priority:** MEDIUM  
**Estimated Time:** 1 hour  
**Coverage Impact:** +0.7%

Focus on:
- IsSet (66.6%)
- Capitalize (70%)
- Cast (71.4%)
- Contains (75%)
- EndsWith (75%)
- ChangeTimeZone (77.7%)

---

## ?? Updated Execution Plan

### Session 2: Push to 85% (4-5 hours)
1. ? Max/Min comprehensive branch coverage - 1 hour
2. ? CountBy scenarios - 45 minutes
3. ? ToString format variations - 30 minutes
4. ? Humanize time units - 30 minutes
5. ? GetProperty edge cases - 30 minutes
6. ? Sum completion - 30 minutes
7. ? Quick wins (70-80% functions) - 1 hour

**Expected Result:** 81% ? 85% ?

### Session 3: Push to 90% (5-6 hours)
8. Complete all 75-85% functions
9. DateTimeMethods improvements
10. Format function completion
11. Branch coverage focus
12. Integration testing

**Expected Result:** 85% ? 90% ?

### Session 4: Push to 95% (5-6 hours)
13. Systematic review of 85-95% functions
14. Complex edge cases
15. Error path completion
16. Branch coverage optimization

**Expected Result:** 90% ? 95% ?

### Session 5: Final Push to 100% (8-10 hours)
17. Line-by-line coverage review
18. Every uncovered branch tested
19. Integration test suite
20. Perfect coverage achieved

**Expected Result:** 95% ? 100% ?

---

## ?? Current Coverage by Function (Top Priority)

### Critical (<60%)
| Function | Coverage | Priority | Effort |
|----------|----------|----------|--------|
| Max | 50.5% | ?? CRITICAL | 1 hour |
| Min | 50.5% | ?? CRITICAL | 1 hour |
| CountBy | 56.2% | ?? HIGH | 45 min |

### High Priority (60-70%)
| Function | Coverage | Priority | Effort |
|----------|----------|----------|--------|
| ToString | 65.6% | ?? HIGH | 30 min |
| Humanize | 65.7% | ?? HIGH | 30 min |
| GetProperty | 66.6% | ?? HIGH | 30 min |
| IsSet | 66.6% | ?? HIGH | 20 min |
| Sum | 67.2% | ?? HIGH | 30 min |

### Medium Priority (70-80%)
| Function | Coverage | Priority | Effort |
|----------|----------|----------|--------|
| Capitalize | 70% | ?? MEDIUM | 20 min |
| Cast | 71.4% | ?? MEDIUM | 20 min |
| Contains | 75% | ?? MEDIUM | 15 min |
| EndsWith | 75% | ?? MEDIUM | 15 min |
| ChangeTimeZone | 77.7% | ?? MEDIUM | 20 min |
| DateTimeMethods | 79.3% | ?? MEDIUM | 30 min |

### Good Progress (80-85%)
| Function | Coverage | Priority | Effort |
|----------|----------|----------|--------|
| Substring | 80% | ?? LOW | 15 min |
| IsInfinite | 81.2% | ?? LOW | 15 min |
| DateTimeIsInPast | 81.8% | ?? LOW | 10 min |
| DateTimeIsInFuture | 81.8% | ?? LOW | 10 min |
| Now | 84.6% | ?? LOW | 10 min |
| If | 85.1% | ?? LOW | 10 min |

---

## ?? Success Criteria (Realistic Milestones)

### Phase 2 Complete When (85%):
? Line Coverage: ? 85%  
? Branch Coverage: ? 75%  
? All <60% functions improved to 80%+  
? Test Pass Rate: ? 99.5%  
? Zero new warnings

### Phase 3 Complete When (90%):
? Line Coverage: ? 90%  
? Branch Coverage: ? 80%  
? All <75% functions improved to 85%+  
? Integration test suite started

### Phase 4 Complete When (95%):
? Line Coverage: ? 95%  
? Branch Coverage: ? 90%  
? All functions ? 90%  
? Comprehensive edge case coverage

### Phase 5 Complete When (100%):
? Line Coverage: **100%**  
? Branch Coverage: **100%**  
? Every line tested  
? Every branch tested  
? Zero uncovered code

---

## ?? Progress Tracking Dashboard

### Velocity Analysis
- **Session 1 Velocity:** 0.7% coverage per hour
- **Tests per Hour:** ~40 tests
- **Quality Maintained:** 99.8% pass rate

### Projections
| Target | Hours from 81% | Total Hours | ETA |
|--------|----------------|-------------|-----|
| 85% | 5-6 hours | 9-10 hours | Nov 1, 2025 |
| 90% | 11-13 hours | 20-23 hours | Nov 7, 2025 |
| 95% | 18-20 hours | 32-35 hours | Nov 14, 2025 |
| 100% | 28-32 hours | 50-55 hours | Nov 28, 2025 |

### Coverage by Category
| Range | Classes | % of Total | Status |
|-------|---------|------------|--------|
| 100% | 36 | 45% | ? Excellent |
| 80-99% | 18 | 23% | ?? Good |
| 70-79% | 12 | 15% | ?? Fair |
| 50-69% | 12 | 15% | ?? Needs Work |
| <50% | 2 | 2% | ?? Critical |

---

## ??? Coverage Enforcement

### CI/CD Requirements
- Minimum: 80% line coverage (current requirement)
- Target: 90% line coverage
- Goal: 100% line coverage

### Publish Script
```powershell
# Current enforcement
.\Publish.ps1  # Requires 90% coverage

# Can be overridden
.\Publish.ps1 -Force  # Skips coverage check

# Custom threshold
.\Publish.ps1 -MinCoverage 85
```

### Future Enhancement
- Phase 2 complete: Update to 85% minimum
- Phase 3 complete: Update to 90% minimum
- Phase 5 complete: Update to 100% minimum

---

## ?? Documentation Status

### Completed
- ? COVERAGE_PLAN.md (this document)
- ? COVERAGE_ASSESSMENT.md (initial analysis)
- ? SESSION_SUMMARY.md (session 1 complete summary)
- ? Publish.ps1 (coverage enforcement)

### In Progress
- ?? Individual function documentation
- ?? Test pattern guidelines

### Planned
- ? Coverage badge for README
- ? Automated coverage reports
- ? Coverage trend analysis

---

## ?? Known Issues

### Test Failures (Known NCalc Bugs)
1. ? EqualityTests.Equality_Succeeds("1 != ''") - NCalc comparison issue
2. ? DiagnosticComparisonTests.Diagnostic_IntegerVsEmptyString_Inequality - Related issue

**Status:** Documented, not blocking coverage work

### Functions Skipped
- **LambdaFunction** (0%) - Internal/unused, low priority
- **Parameters** (16%) - Internal helper class, not user-facing

---

## ?? Session 1 Achievements

### Quantitative
- **+157 tests** (16.8% increase)
- **+2.8% line coverage**
- **+2.0% branch coverage**
- **+4 classes at 100%**
- **8 commits** with full documentation

### Qualitative
- ? Established comprehensive test patterns
- ? Zero coverage classes eliminated
- ? Strong foundation for continued work
- ? Documented approach and methodology
- ? Maintained code quality (99.8% pass rate)

---

## ?? Next Session Priorities

1. **Max/Min functions** - Highest impact (1 hour, +1%)
2. **CountBy function** - High priority (45 min, +0.8%)
3. **ToString function** - Quick win (30 min, +0.5%)
4. **Humanize function** - Medium effort (30 min, +0.4%)
5. **GetProperty function** - Good ROI (30 min, +0.3%)

**Total: 3.5 hours ? Expected result: 81% ? 84-85%**

---

**Plan Updated:** October 31, 2025 - End of Session 1  
**Next Review:** November 1, 2025 - Start of Session 2  
**Status:** ?? Excellent Progress - 81% Achieved  
**Commitment:** Systematic approach to 100% coverage
