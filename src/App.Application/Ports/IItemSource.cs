using Contoso.App.Domain;

namespace Contoso.App.Application.Ports;

/// <summary>Inbound port: reads items from the source system. Implemented in Infrastructure.</summary>
public interface IItemSource
{
    /// <summary>Reads every item currently available from the source.</summary>
    IReadOnlyCollection<Item> Read();
}
