using System;
using System.Threading;
using System.Threading.Tasks;
using RuleFlow.Core.Abstractions;

namespace RuleFlow.Core.Rules;

public sealed class MinLengthRule<TContext> : IRule<TContext>
{
    private readonly string _fieldName;
    private readonly int _minLength;
    private readonly bool _trimWhitespace;
    private readonly Func<TContext, string?> _selector;

    public MinLengthRule(string fieldName, int minLength, Func<TContext, string?> selector, bool trimWhitespace = true)
    {
        _fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        if (minLength < 0) throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length must be >= 0.");
        _minLength = minLength;
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        _trimWhitespace = trimWhitespace;
    }

    public Task<RuleResult> EvaluateAsync(TContext context, CancellationToken ct = default)
    {
        if (ct.IsCancellationRequested)
            ct.ThrowIfCancellationRequested();

        var value = _selector(context);
        var measured = value is null
            ? 0
            : (_trimWhitespace ? value.Trim().Length : value.Length);

        if (measured < _minLength)
        {
            var error = new RuleError("MINLEN", $"{_fieldName} must be at least {_minLength} characters long.");
            return Task.FromResult(RuleResult.Fail(error));
        }

        return Task.FromResult(RuleResult.Success);
    }
}
