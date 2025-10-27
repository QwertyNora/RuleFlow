using System;
using System.Threading;
using System.Threading.Tasks;
using RuleFlow.Core.Abstractions;

namespace RuleFlow.Core.Rules;

public sealed class NotNullRule<TContext> : IRule<TContext>
{
    private readonly string _fieldName;
    private readonly Func<TContext, object?> _selector;

    public NotNullRule(string fieldName, Func<TContext, object?> selector)
    {
        _fieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        _selector = selector ?? throw new ArgumentNullException(nameof(selector));
    }

    public Task<RuleResult> EvaluateAsync(TContext context, CancellationToken ct = default)
    {
        // Cooperative cancellation (fast-fail if a caller asked us to stop).
        if (ct.IsCancellationRequested)
            ct.ThrowIfCancellationRequested();

        var value = _selector(context);

        if (value is null)
        {
            var error = new RuleError("NULL", $"{_fieldName} must not be null.");
            return Task.FromResult(RuleResult.Fail(error));
        }

        return Task.FromResult(RuleResult.Success);
    }
}

