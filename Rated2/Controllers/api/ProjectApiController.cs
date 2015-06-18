using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace Rated.Web.Controllers.api
{
    public class ProjectApiController : ApiController
    {
        IProjectRepo _projectRepo;

        public ProjectApiController()
        {
            _projectRepo = new ProjectRepo();
        }

        [Route("api/ProjectApi/AddDetail")]
        [HttpPost]
        public HttpResponseMessage AddDetail(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                var userSession = new UserSession();
                projectDetail.UserId = userSession.GetUserSession().UserId;
                _projectRepo.AddProjectDetail(projectDetail);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = ex.Message
                });
            }
        }

        [Route("api/ProjectApi/AddProject")]
        [HttpPost]
        public string AddProject(ProjectCoreModel project)
        {
            try
            {
                var userSession = new UserSession();
                var user = userSession.GetUserSession();
                var newProjectId = Guid.NewGuid();

                project.ProjectId = newProjectId;
                project.UserId = user.UserId;

                _projectRepo.AddProject(project);

                // TODO: Get record from database
                var newProduct = _projectRepo.GetProject(user.UserId, project.ProjectId);

                var json = new JavaScriptSerializer().Serialize(newProduct);
                return json;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = ex.Message
                });
            }
        }
    }
}
