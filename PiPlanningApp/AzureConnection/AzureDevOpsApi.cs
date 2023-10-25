namespace PiPlanningApp.AzureConnection;

using System;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

public class AzureDevOpsApi
{
    private readonly string _baseUrl;
    private readonly string projectName;
    private readonly string _personalAccessToken;

    public AzureDevOpsApi(string organizationName, string projectName, string personalAccessToken)
    {
        this._baseUrl = $"https://dev.azure.com/{organizationName}/";
        this.projectName = projectName;
        this._personalAccessToken = personalAccessToken;
    }

    public async Task<WorkItem> CreateWorkItemAsync(string featureId, string iterationPath, string title, string effort)
    {
        var credentials = new VssBasicCredential("", this._personalAccessToken);
        var connection = new VssConnection(new Uri(this._baseUrl), credentials);
        var client = connection.GetClient<WorkItemTrackingHttpClient>();

        // Create a new user story work item
        var document = new JsonPatchDocument
        {
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Title",
                Value = title // replace with the title of your user story
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.AreaPath",
                Value = "PIPlanningAssistantTool" // replace with the area path of your user story
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Common.Priority",
                Value = 2 // replace with the priority of your user story
            },
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.IterationPath",
                Value = iterationPath // replace with the path to the iteration you want to assign the user story to
            },
            new JsonPatchOperation()
            {
            Operation = Operation.Add,
            Path = "/relations/-",
            Value = new
                {
                    rel = "System.LinkTypes.Hierarchy-Reverse",
                    url = $"{this._baseUrl}/{projectName}/_apis/wit/workItems/{featureId}"
                },
            }
        };
        if (!string.IsNullOrEmpty(effort))
        {
            document.Add(new()
            {
                Operation = Operation.Add,
                Path = "/fields/Microsoft.VSTS.Scheduling.StoryPoints",
                Value = effort // replace with the effort of your user story
            });
        }

        return await client.CreateWorkItemAsync(document, projectName, "User Story");
    }
}
