using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PiPlanningApp.Models;

namespace PiPlanningApp.Repositories;

internal interface IInformationRepository
{
    ApplicationInformation ApplicationInformation { get; set; }

    void ReadInformation();

    void SaveChanges();
}