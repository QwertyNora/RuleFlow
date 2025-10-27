namespace RuleFlow.Core.Core;

// Immutable input model used as validation context for the pipeline

public sealed record UserRegistration
{
    public string? Email { get; init; }

    public string? Password { get; init; }

    // Optional convenience constructor to set properties at creation time.
    public UserRegistration(string? email, string? password)
    {
        Email = email;
        Password = password;
    }

    // Parameterless constructor to support object initializers and serializers.
    public UserRegistration() { }
}