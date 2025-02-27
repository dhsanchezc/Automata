using Automata.Domain.Common;

namespace Automata.Domain.Aggregates.Assets;

public class Asset : IAggregateRoot
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Asset()
    {
        CreatedAt = DateTime.UtcNow;
    }
}