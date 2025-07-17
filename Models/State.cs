namespace Infonetica.WorkflowEngine.Models;

public class State
{
    public required string Id { get; init; }// init used : only can be defined at the start 
    public string? Name { get; set; }
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public string? Description{ get; set;}
}
