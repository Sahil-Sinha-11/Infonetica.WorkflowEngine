using Infonetica.WorkflowEngine.DTOs;
using Infonetica.WorkflowEngine.Services;

namespace Infonetica.WorkflowEngine.Endpoints;

public static class WorkflowEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/workflows");

        group.MapPost("/definitions", (WorkflowService service, CreateWorkflowDefinitionDto dto) =>
        {
            var (definition, error) = service.CreateDefinition(dto);
            return error != null 
                ? Results.BadRequest(new { message = error }) 
                : Results.Created($"/api/workflows/definitions/{definition!.Id}", definition);
        });

        group.MapGet("/definitions/{id:guid}", (WorkflowService service, Guid id) =>
        {
            var definition = service.GetDefinition(id);
            return definition == null ? Results.NotFound() : Results.Ok(definition);
        });

        group.MapPost("/instances/{definitionId:guid}/start", (WorkflowService service, Guid definitionId) =>
        {
            var (instance, error) = service.StartInstance(definitionId);
            return error != null ? Results.BadRequest(new { message = error }) : Results.Ok(instance);
        });
        
        group.MapGet("/instances/{id:guid}", (WorkflowService service, Guid id) =>
        {
            var instance = service.GetInstance(id);
            return instance == null ? Results.NotFound() : Results.Ok(instance);
        });

        group.MapPost("/instances/{id:guid}/execute/{actionId}", (WorkflowService service, Guid id, string actionId) =>
        {
            var (instance, error) = service.ExecuteAction(id, actionId);
            return error != null ? Results.BadRequest(new { message = error }) : Results.Ok(instance);
        });
    }
}
