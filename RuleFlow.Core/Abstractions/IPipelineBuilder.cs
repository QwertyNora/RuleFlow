using System.Collections.Generic;

namespace RuleFlow.Core.Abstractions;

// Builder contract for composing a validation pipeline from discrete rules.
// This enables a fluent API to to add rules and then build a concrete IPipeline.
// TContext => The type of the input the pipeline will validate (e.g., UserRegistration).

public interface IPipelineBuilder<TContext>
{
    // Add a single rule to the builder
    IPipelineBuilder<TContext> AddRule(IRule<TContext> rule);

    // Add multiple rules to the builder in one call
    IPipelineBuilder<TContext> AddRules(IEnumerable<IRule<TContext>> rules);

    IPipeline<TContext> Build();
}