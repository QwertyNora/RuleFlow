using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RuleFlow.Core.Abstractions;

namespace RuleFlow.Core.Core;

public sealed class Pipeline<TContext> : IPipeline<TContext>
{
    private readonly IReadOnlyList<IRule<TContext>> _rules;

    public IReadOnlyList<IRule<TContext>> Rules => _rules;

    public Pipeline(IEnumerable<IRule<TContext>> rules)
    {
        if (rules is null) throw new ArgumentNullException(nameof(rules));

        var list = new List<IRule<TContext>>(rules);
        _rules = new ReadOnlyCollection<IRule<TContext>>(list);
    }

    public async Task<PipelineResult> RunAsync(TContext context, CancellationToken ct = default)
    {

        var results = new List<RuleResult>(_rules.Count);

        foreach (var rule in _rules)
        {
            if (ct.IsCancellationRequested)
            {
                ct.ThrowIfCancellationRequested();
            }

            var result = await rule.EvaluateAsync(context, ct).ConfigureAwait(false);
            results.Add(result);
        }

        return PipelineResult.From(results);
    }
}
