# RuleFlow – A Simple and Extensible Validation Pipeline

## 🧭 Overview

**RuleFlow** is a small, reusable C# library designed to validate objects through a **sequence of small, focused rules**.  
Each rule checks one specific thing (e.g., “Email must not be null”), and the pipeline runs them in order, collecting all validation results into a single summary.

The project demonstrates **Object-Oriented Programming (OOP)**, the **SOLID principles**, and several key **Design Patterns** — in a clean, testable way.

---

## 🎯 Goal Example

```csharp
var input = new UserRegistration("nora@example.com", "supersecret");

// Build a validation pipeline
var pipeline = new PipelineBuilder<UserRegistration>()
    .AddRule(new NotNullRule<UserRegistration>("Email", x => x.Email))
    .AddRule(new RegexRule<UserRegistration>("Email", @"^\S+@\S+\.\S+$", x => x.Email))
    .AddRule(new MinLengthRule<UserRegistration>("Password", 8, x => x.Password))
    .Build();

// Run the pipeline
var result = pipeline.Run(input); // => IsSuccess: true/false, Errors: [...]
```

## ⚙️ Project Structure

```bash
RuleFlow.sln
├─ RuleFlow.Core         # Core library (reusable logic)
│  ├─ Abstractions       # Interfaces + result types (contracts)
│  ├─ Core               # Pipeline engine + builder + context
│  ├─ Rules              # Concrete validation rules
│  └─ Factories          # (Optional) Rule creation via keys (Factory Method)
├─ RuleFlow.Demo.Cli     # Simple console demo (composition root)
└─ RuleFlow.Tests        # xUnit test project
```

## 🧩 Why this structure?

- **Abstractions** — defines what the system can do
- **Core** — implements the logic that connects everything
- **Rules** — independent, reusable components
- **Factories** — allows flexible creation of rules
- **Demo.Cli** — a tiny demo app to show how the library works
- **Tests** — unit tests that ensure correctness

## 🧩 How it all fits together:

```sql
[UserRegistration]  --->  [Pipeline.Run(ctx)]
                             |
                             v
              +--------------------------------+
              |  Chain of Rules (in order)     |
              |   1) NotNullRule(Email)        |
              |   2) RegexRule(Email)          |
              |   3) MinLengthRule(Password)   |
              +--------------------------------+
                |         |            |
                v         v            v
           RuleResult  RuleResult   RuleResult   (one per rule)
                 \        |         /
                  \       |        /
                   \      |       /
                   [PipelineResult]  (aggregated via LINQ)

```

1. You have an input object, e.g. UserRegistration.
2. The pipeline runs each rule in sequence.
3. Each rule inspects one property (via lambda selector).
4. Each rule returns a RuleResult (success/failure).
5. The pipeline combines all rule results into a PipelineResult.
6. You can then use this result in your app (UI message, stopping flow, etc.).

## SOLID + Patterns used:

| Concept                     | How it appears in RuleFlow                             |
| --------------------------- | ------------------------------------------------------ |
| **Chain of Responsibility** | Each rule is a handler in a sequential chain           |
| **Builder Pattern**         | `PipelineBuilder` composes and constructs the pipeline |
| **Factory Method**          | `IRuleFactory` can create rule instances from keys     |
| **LINQ**                    | Used for collecting and aggregating validation results |

## 🚦 Data Flow Summary

1. Input object (UserRegistration) is provided to the pipeline.
2. The builder constructs the chain of rules.
3. Pipeline.Run(context) executes each rule in sequence.
4. Each rule checks one property and returns a RuleResult.
5. LINQ gathers all failed rule results into a single PipelineResult.
6. The caller reads PipelineResult.IsSuccess and Errors.

## 🧪 How to Run

```bash
dotnet build
dotnet test
dotnet run --project RuleFlow.Demo.Cli
```
