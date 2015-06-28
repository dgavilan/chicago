using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
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

        [Route("api/ProjectApi/Project/{projectId}/StartTheProject")]
        [HttpPut]
        public HttpResponseMessage StartTheProject(Guid projectId)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                //projectDetail.UserId = userSession.GetUserSession().UserId;

                // 1. insert record in ProjectReviewer table
                // 2. insert record in user table if user id doesn't exist
                _projectRepo.StartTheProject(userSession.GetUserSession().UserId, projectId);

                return Request.CreateResponse(HttpStatusCode.OK);
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

        [Route("api/ProjectApi/Project/Reviewer")]
        [HttpPost]
        public HttpResponseMessage AddReviewer(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                projectDetail.UserId = userSession.GetUserSession().UserId;

                // 1. insert record in ProjectReviewer table
                // 2. insert record in user table if user id doesn't exist
                _projectRepo.AddProjectReviewer(projectDetail);

                return Request.CreateResponse(HttpStatusCode.OK);
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

        [Route("api/ProjectApi/AddDetail")]
        [HttpPost]
        public HttpResponseMessage AddDetail(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                projectDetail.ProjectDetailId = Guid.NewGuid();
                projectDetail.UserId = userSession.GetUserSession().UserId;
                projectDetail.CreatedBy = userSession.GetUserSession().UserId;
                projectDetail.CreatedDate = timeStamp;
                projectDetail.ModifiedBy = userSession.GetUserSession().UserId;
                projectDetail.ModifiedDate = timeStamp;

                _projectRepo.AddProjectDetail(projectDetail);

                var javaScriptSerializer = new JavaScriptSerializer();

                return Request.CreateResponse(HttpStatusCode.OK, javaScriptSerializer.Serialize(projectDetail));
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

        [Route("api/ProjectApi/UpdateDetail")]
        [HttpPut]
        public HttpResponseMessage UpdateDetail(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                var userSession = new UserSession();
                projectDetail.UserId = userSession.GetUserSession().UserId;
                _projectRepo.UpdateProjectDetail(projectDetail);

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

        [Route("api/ProjectApi/Project/{projectId}/Detail/{detailId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteDetail(Guid projectId, Guid detailId)
        {
            try
            {
                var userSession = new UserSession();
                _projectRepo.DeleteProjectDetail(userSession.GetUserSession().UserId, projectId, detailId);

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
