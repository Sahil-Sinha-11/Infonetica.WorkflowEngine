namespace Infonetica.WorkflowEngine.Models;

public class WorkflowInstance
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid DefinitionId { get; init; }
    public required string CurrentStateId { get; set; }
    public List<HistoryEntry> History { get; } = new List<HistoryEntry>();
}
