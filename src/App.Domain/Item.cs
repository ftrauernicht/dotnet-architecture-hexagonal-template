namespace Contoso.App.Domain;

/// <summary>
/// A sample domain entity. Replace with your real domain model. It carries a stable
/// <see cref="ExternalId"/> from the source system, used for idempotency.
/// </summary>
public sealed record Item(string ExternalId, string Name);
