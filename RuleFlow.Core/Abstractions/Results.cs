using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleFlow.Core.Abstractions;

// Represents a single validation error from a rule.
public sealed record RuleError(string Code, string Message);

// Result returned by a single rule. Either IsSuccess = true (no errors),
// or false with a list of RuleError entries.
public sealed record RuleResult(bool IsSuccess, IReadOnlyList<RuleError> Errors)
{
    // Factory member for a successful result (note: property, not method).
    public static RuleResult Success => new(true, Array.Empty<RuleError>());

    // Factory method for a failed result.
    public static RuleResult Fail(params RuleError[] errors) => new(false, errors);
}

// Aggregated result of all rules in a pipeline.
public sealed record PipelineResult(bool IsSuccess, IReadOnlyList<RuleError> Errors)
{
    public static PipelineResult From(IEnumerable<RuleResult> ruleResults)
    {
        var errors = ruleResults
            .Where(r => !r.IsSuccess)
            .SelectMany(r => r.Errors)
            .ToArray();

        return new PipelineResult(errors.Length == 0, errors);
    }
}
