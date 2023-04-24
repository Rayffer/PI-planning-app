using System;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using PiPlanningApp.Models;
using PiPlanningApp.Types;

namespace PiPlanningApp.Repositories;

internal class JsonInformationRepository : IInformationRepository
{
    private readonly string filePath;
    public ApplicationInformation ApplicationInformation { get; set; }

    public JsonInformationRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public void ReadInformation()
    {
        if (!File.Exists(this.filePath))
        {
            File.WriteAllText(this.filePath, JsonConvert.SerializeObject(new ApplicationInformation()));
        }
        this.ApplicationInformation = JsonConvert.DeserializeObject<ApplicationInformation>(File.ReadAllText(this.filePath));

        if (!this.ApplicationInformation.Features.Any())
        {
            this.ApplicationInformation.Features.Add(new Feature
            {
                Id = Guid.NewGuid(),
                RowPosition = this.ApplicationInformation.Features.Count,
                Title = "Feature Title",
                FeatureType = FeatureTypes.Feature
            });

            this.SaveChanges();
        }

        if (!this.ApplicationInformation.Iterations.Any())
        {
            Enumerable.Range(0, 6).ToList().ForEach(_ => this.ApplicationInformation.Iterations.Add(new Iteration
            {
                Id = Guid.NewGuid(),
                ColumnPosition = this.ApplicationInformation.Iterations.Count,
                IterationNumber = this.ApplicationInformation.Iterations.Count + 1
            }));
            this.ApplicationInformation.Iterations.Last().IterationName = "IP";

            this.SaveChanges();
        }

        if (!this.ApplicationInformation.IterationFeatureSlots.Any())
        {
            foreach (var feature in this.ApplicationInformation.Features)
                foreach (var iteration in this.ApplicationInformation.Iterations)
                {
                    this.ApplicationInformation.IterationFeatureSlots.Add(new IterationFeatureSlot
                    {
                        ColumnPosition = iteration.ColumnPosition,
                        ParentIterationId = iteration.Id,
                        ParentFeatureId = feature.Id,
                        RowPosition = feature.RowPosition,
                        UserStories = new()
                    });
                }

            this.SaveChanges();
        }
    }

    public void SaveChanges()
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(this.ApplicationInformation));
    }
}