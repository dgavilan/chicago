using Rated.Core.Contracts;
using Rated.Core.Models.Project;
using Rated.Core.Shared;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using Rated.Models.Project;
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

        [Route("api/ProjectApi/Project/ReviewerAccepted")]
        [HttpPut]
        public HttpResponseMessage ReviewerAccepted(ProjectCoreModel projectCore)
        //[Route("api/ProjectApi/Project/{projectId}/ReviewerAccepted")]
        //[HttpPut]
        //public HttpResponseMessage ReviewerAccepted(Guid projectId)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                projectCore.UserId = userSession.GetUserSession().UserId;
                projectCore.MoveToNextStatus();

                //projectDetail.UserId = userSession.GetUserSession().UserId;

                // 1. insert record in ProjectReviewer table
                // 2. insert record in user table if user id doesn't exist
                //_projectRepo.ReviewerAccepted(projectId, userSession.GetUserSession().UserId);
                _projectRepo.ReviewerAccepted(projectCore);

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

        //[Route("api/ProjectApi/Project/{projectId}/StartTheProject")]
        [Route("api/ProjectApi/Project/StartTheProject")]
        [HttpPut]
        //public HttpResponseMessage StartTheProject(Guid projectId)
        public HttpResponseMessage StartTheProject(ProjectCoreModel projectCore)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                //projectCore.UserId = userSession.GetUserSession().UserId;
                //projectCore.MoveToNextStatus();
                _projectRepo.MoveToNextProjectStatus(projectCore);

                // TODO: 1. Keep project status the same (pending)
                //       2. Move detail status to next
                

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
                projectDetail.DetailStatus = Enums.ProjectDetailStatus.New;
                projectDetail.MoveToNextStatus();

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

                // Save project
                project.ProjectId = newProjectId;
                project.UserId = user.UserId;
                project.ProjectStatus = Enums.ProjectStatus.New;
                project.MoveToNextStatus();
                _projectRepo.AddProject(project);

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

        [Route("api/ProjectApi/Project/{projectId}/ProjectDetail/{projectDetailId}/Complete")]
        [HttpPut]
        public HttpResponseMessage MarkProjectDetailAsComplete(Guid projectId, Guid projectDetailId)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                //_projectRepo.UpdateProjectDetailStatus(
                //    userSession.GetUserSession().UserId, 
                //    projectDetailId,
                //    Core.Shared.Enums.ProjectDetailStatus.Done);

                //_projectRepo.ProjectCompletedByOwner(userSession.GetUserSession().UserId, projectId);

                var projectDetail = _projectRepo.GetProjectDetailById(projectDetailId);
                projectDetail.MoveToNextStatus();
                _projectRepo.UpdateProjectDetail(projectDetail);

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

        [Route("api/ProjectApi/ProjectDetail/{projectDetailId}")]
        [HttpGet]
        public HttpResponseMessage GetProjectDetailById(Guid projectDetailId)
        {
            try
            {
                var projectDetailCore = _projectRepo.GetProjectDetailById(projectDetailId);
                var projectHelper = new ProjectHelper();
                var detailView = projectHelper.BuildProjectDetailView(projectDetailCore);

                return Request.CreateResponse(HttpStatusCode.OK, detailView);
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

        [Route("api/ProjectApi/ProjectDetail/{projectDetailId}/ReviewerAcceptsProjectDetail")]
        [HttpPut]
        public HttpResponseMessage ReviewerAcceptsProjectDetail(Guid projectDetailId)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                var projectDetail = _projectRepo.GetProjectDetailById(projectDetailId);
                projectDetail.MoveToNextStatus();
                _projectRepo.UpdateProjectDetail(projectDetail);

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

        [Route("api/ProjectApi/ProjectDetail/SubmitReview")]
        [HttpPut]
        public HttpResponseMessage SubmitReview(ProjectDetailCoreModel projectDetail)
        {
            try
            {
                var userSession = new UserSession();
                var timeStamp = DateTime.UtcNow;

                var projectDetailToUpdate = _projectRepo.GetProjectDetailById(projectDetail.ProjectDetailId);
                projectDetailToUpdate.DetailRating = projectDetail.DetailRating;
                projectDetailToUpdate.MoveToNextStatus();

                _projectRepo.UpdateProjectDetail(projectDetailToUpdate);
                _projectRepo.SetProjectToDone(projectDetail.ProjectId);

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

    }
}
