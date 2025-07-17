using Infonetica.WorkflowEngine.Models;

namespace Infonetica.WorkflowEngine.DTOs;

public class CreateWorkflowDefinitionDto
{
    public required List<State> States { get; set; }
    public required List<WorkflowAction> Actions { get; set; }
}
