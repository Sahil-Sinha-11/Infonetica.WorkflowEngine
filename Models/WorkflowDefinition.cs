namespace Infonetica.WorkflowEngine.Models;

public class WorkflowDefinition
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required List<State> States { get; init; }
    public required List<WorkflowAction> Actions { get; init; }
}
