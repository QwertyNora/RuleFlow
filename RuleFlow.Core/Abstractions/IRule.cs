using System.Threading;
using System.Threading.Tasks;

namespace RuleFlow.Core.Abstractions;

/// Contract for a single validation rule that can evaluate a given context.
/// TContext = The type of the input the rule validates (e.g., UserRegistration).

public interface IRule<in TContext>
{
    Task<RuleResult> EvaluateAsync(TContext context, CancellationToken ct = default);
}

