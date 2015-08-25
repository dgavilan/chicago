using Rated.Core.Contracts;
using Rated.Infrastructure.Database.Repository;
using Rated.Web.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rated.Web.Controllers.api
{
    public class CompanyApiController : ApiController
    {
        ICompanyRepo _companyRepo;

        public CompanyApiController()
        {
            _companyRepo = new CompanyRepo();
        }

        [Route("api/CompanyApi/GetCompanyForUser")]
        [HttpGet]
        public HttpResponseMessage GetCompanyForUser()
        {
            try
            {
                var userSession = new UserSession();
                var companies = _companyRepo.GetCompanyForUser(userSession.GetUserSession().UserId);

                return Request.CreateResponse(HttpStatusCode.OK, companies);
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
