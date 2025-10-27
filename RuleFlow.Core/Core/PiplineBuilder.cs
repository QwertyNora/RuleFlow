using System;
using System.Collections.Generic;
using RuleFlow.Core.Abstractions;

namespace RuleFlow.Core.Core;

public sealed class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
{
    private readonly List<IRule<TContext>> _rules = new();

    // Add a single rule to the builder (fluent API):
    public IPipelineBuilder<TContext> AddRule(IRule<TContext> rule)
    {
        if (rule is null) throw new ArgumentNullException(nameof(rule));
        _rules.Add(rule);
        return this;
    }

    // Add multiple rules to the builder (fluent API):
    public IPipelineBuilder<TContext> AddRules(IEnumerable<IRule<TContext>> rules)
    {
        if (rules is null) throw new ArgumentNullException(nameof(rules));
        foreach (var r in rules)
        {
            if (r is null) throw new ArgumentException("Rules collection contains a null element.", nameof(rules));
            _rules.Add(r);
        }
        return this;
    }

    public IPipeline<TContext> Build()
    {
        // Defensive copy to ensure the produced pipeline becomes immutable from the outside.
        var snapshot = new List<IRule<TContext>>(_rules);
        return new Pipeline<TContext>(snapshot);
    }
}

