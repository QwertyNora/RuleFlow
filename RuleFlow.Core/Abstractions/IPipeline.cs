using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RuleFlow.Core.Abstractions;

// Contract for a validation pipeline that evaluates a sequence of rules
// agains a given context and produces an aggregated PipelineResult.
// TContext => The type of the input that the pipeline validates (e.g., UserRegistration).
public interface IPipeline<in TContext>
{
    IReadOnlyList<IRule<TContext>> Rules { get; }

    Task<PipelineResult> RunAsync(TContext context, CancellationToken ct = default);
}