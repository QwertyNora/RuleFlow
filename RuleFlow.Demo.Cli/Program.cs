using System;
using System.Threading;
using System.Threading.Tasks;
using RuleFlow.Core.Abstractions;
using RuleFlow.Core.Core;
using RuleFlow.Core.Rules;

var context = new UserRegistration(
    email: "example@example.com",
    password: "supersafepassword"
);

IPipeline<UserRegistration> pipeline = new PipelineBuilder<UserRegistration>()
    .AddRule(new NotNullRule<UserRegistration>("Email", x => x.Email))
    .AddRule(new NotNullRule<UserRegistration>("Password", x => x.Password))
    .AddRule(new MinLengthRule<UserRegistration>("Password", 8, x => x.Password))
    .Build();

PipelineResult result = await pipeline.RunAsync(context, CancellationToken.None);

PrintReport(result);

// --- Helper ---

static void PrintReport(PipelineResult result)
{
    Console.WriteLine("=== RuleFlow: Validation Report ===");
    Console.WriteLine($"IsSuccess: {result.IsSuccess}");
    if (!result.IsSuccess)
    {
        Console.WriteLine("Errors:");
        foreach (var err in result.Errors)
        {
            Console.WriteLine($" - [{err.Code}] {err.Message}");
        }
    }
    Console.WriteLine("===================================");
}
