# Infonetica.WorkflowEngine

A minimal, extensible workflow engine built with ASP.NET Core 9.0. This project allows you to define, manage, and execute custom workflows via a simple REST API.

## Features

- **Define workflows** with custom states and actions.
- **Start workflow instances** and track their progress.
- **Execute actions** to transition between states.
- **RESTful API** with OpenAPI/Swagger documentation.
- **In-memory storage** for workflow definitions and instances (suitable for prototyping and testing).

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Setup

1. **Install dependencies** (if not already present):

   ```sh
   dotnet restore
   ```

2. **Run the application with HTTPS enabled:**

   ```sh
   dotnet run --launch-profile https
   ```

   > **Note:**
   > This project includes both HTTP and HTTPS launch profiles (see `Properties/launchSettings.json`).
   > Running with the `https` profile is recommended, as some browsers and tools require HTTPS for API and Swagger UI access, and may block or warn about HTTP endpoints.

3. **Access the API documentation:**

   - Navigate to [https://localhost:7171/swagger](https://localhost:7171/swagger) (or the port shown in your terminal).

## API Overview

All workflow-related endpoints are grouped under `/api/workflows`.

### Endpoints

- **POST `/api/workflows/definitions`**  
  Create a new workflow definition.

- **GET `/api/workflows/definitions/{id}`**  
  Retrieve a workflow definition by its ID.

- **POST `/api/workflows/instances/{definitionId}/start`**  
  Start a new workflow instance from a definition.

- **GET `/api/workflows/instances/{id}`**  
  Retrieve a workflow instance by its ID.

- **POST `/api/workflows/instances/{id}/execute/{actionId}`**  
  Execute an action on a workflow instance.

### Example: Create a Workflow Definition

```json
POST /api/workflows/definitions
Content-Type: application/json

{
  "states": [
    { "id": "start", "name": "Start", "isInitial": true, "isFinal": false },
    { "id": "end", "name": "End", "isInitial": false, "isFinal": true }
  ],
  "actions": [
    { "id": "advance", "name": "Advance", "fromStates": ["start"], "toState": "end" }
  ]
}
```

## Data Models

### WorkflowDefinition

```csharp
public class WorkflowDefinition
{
    public Guid Id { get; init; }
    public List<State> States { get; init; }
    public List<WorkflowAction> Actions { get; init; }
}
```

### State

```csharp
public class State
{
    public string Id { get; init; }
    public string? Name { get; set; }
    public bool IsInitial { get; set; }
    public bool IsFinal { get; set; }
    public string? Description { get; set; }
}
```

### WorkflowAction

```csharp
public class WorkflowAction
{
    public string Id { get; init; }
    public string Name { get; set; }
    public List<string> FromStates { get; init; }
    public string ToState { get; init; }
}
```

### WorkflowInstance

```csharp
public class WorkflowInstance
{
    public Guid Id { get; init; }
    public Guid DefinitionId { get; init; }
    public string CurrentStateId { get; set; }
    public List<HistoryEntry> History { get; }
}
```

### HistoryEntry

```csharp
public class HistoryEntry
{
    public string ActionId { get; init; }
    public DateTime Timestamp { get; init; }
}
```

## Configuration

- **Logging**: Controlled via `appsettings.json` and `appsettings.Development.json`.
- **AllowedHosts**: Set to `*` by default.

## Dependencies

- `Microsoft.AspNetCore.OpenApi`
- `Swashbuckle.AspNetCore`

## Development Notes

- All data is stored in-memory; restarting the app will clear all definitions and instances.
- The project is structured for easy extension (e.g., add persistent storage, authentication, etc.).
