using Contoso.App.Domain;

namespace Contoso.App.Application.Ports;

/// <summary>Outbound port: writes an item to the target system. Implemented in Infrastructure.</summary>
public interface IItemTarget
{
    /// <summary>Writes a single item to the target.</summary>
    void Write(Item item);
}
