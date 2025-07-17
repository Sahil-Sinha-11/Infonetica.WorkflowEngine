namespace Infonetica.WorkflowEngine.Models;


public class WorkflowAction
{
    public required string Id { get; init; }
    public string Name { get; set; }
    public required List<string> FromStates { get; init; }
    public required string ToState { get; init; }
}