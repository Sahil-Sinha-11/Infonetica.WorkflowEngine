using System.Collections.Concurrent;
using Infonetica.WorkflowEngine.DTOs;
using Infonetica.WorkflowEngine.Models;

namespace Infonetica.WorkflowEngine.Services;

public class WorkflowService
{
    private readonly ConcurrentDictionary<Guid, WorkflowDefinition> _definitions = new();
    private readonly ConcurrentDictionary<Guid, WorkflowInstance> _instances = new();

    public (WorkflowDefinition? definition, string? error) CreateDefinition(CreateWorkflowDefinitionDto dto)
    {
        if (dto.States.Count(s => s.IsInitial) != 1)
        {
            return (null, "A workflow definition must have exactly one initial state.");
        }

        if (dto.States.Select(s => s.Id).Distinct().Count() != dto.States.Count)
        {
            return (null, "All state IDs within a definition must be unique.");
        }

        var definition = new WorkflowDefinition { States = dto.States, Actions = dto.Actions };
        _definitions[definition.Id] = definition;
        return (definition, null);
    }

    public WorkflowDefinition? GetDefinition(Guid id) => _definitions.GetValueOrDefault(id);

    public (WorkflowInstance? instance, string? error) StartInstance(Guid definitionId)
    {
        if (!_definitions.TryGetValue(definitionId, out var definition))
        {
            return (null, "Workflow definition not found.");
        }

        var initialState = definition.States.Single(s => s.IsInitial);

        var instance = new WorkflowInstance
        {
            DefinitionId = definitionId,
            CurrentStateId = initialState.Id
        };

        _instances[instance.Id] = instance;
        return (instance, null);
    }

    public WorkflowInstance? GetInstance(Guid id) => _instances.GetValueOrDefault(id);

    public (WorkflowInstance? instance, string? error) ExecuteAction(Guid instanceId, string actionId)
    {
        if (!_instances.TryGetValue(instanceId, out var instance))
        {
            return (null, "Workflow instance not found.");
        }

        if (!_definitions.TryGetValue(instance.DefinitionId, out var definition))
        {
            return (null, "Workflow definition not found for this instance.");
        }

        var currentState = definition.States.Find(s => s.Id == instance.CurrentStateId);
        if (currentState?.IsFinal == true)
        {
            return (null, "Cannot execute action: the instance is in a final state.");
        }

        var action = definition.Actions.Find(a => a.Id == actionId);
        if (action == null)
        {
            return (null, "Action not found in this workflow definition.");
        }

        if (!action.FromStates.Contains(instance.CurrentStateId))
        {
            return (null, $"Action '{actionId}' cannot be executed from the current state '{instance.CurrentStateId}'.");
        }

        instance.CurrentStateId = action.ToState;
        instance.History.Add(new HistoryEntry { ActionId = actionId });

        return (instance, null);
    }
}
