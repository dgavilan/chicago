﻿using Rated.Core.Models.Project;
using Rated.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Contracts
{
    public interface IProjectRepo
    {
        void AddProject(ProjectCoreModel project);

        ProjectCoreModel GetProject(Guid userId, Guid projectId);

        void AddProjectDetail(ProjectDetailCoreModel projectDetail);
    }
}
