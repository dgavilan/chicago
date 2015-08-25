using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rated.Core.Models.User;
using Rated.Infrastructure.Database.EF.User;
using Rated.Core.Contracts;
using System.Data.Entity;
using Rated.Core.Shared;
using System.Data.Entity.Validation;
using Rated.Infrastructure.Database.EF.Project;
using Rated.Core.Models.Project;

namespace Rated.Infrastructure.Database.Repository
{
    public class CompanyRepo : ICompanyRepo
    {
        ProjectContext _projectContext;

        public CompanyRepo()
        {
            _projectContext = new ProjectContext();
        }

        public void AddNewCompany(CompanyCoreModel companyCore)
        {
            _projectContext.Companies.Add(MapToDb(companyCore));
            _projectContext.SaveChanges();
        }

        public List<CompanyCoreModel> GetCompanyForUser(Guid userId)
        {
            var companiesDb = (from c in _projectContext.Companies
                               where c.UserId == userId
                               select c).ToList();

            var companiesCore = new List<CompanyCoreModel>();

            foreach (var companyDb in companiesDb)
            {
                companiesCore.Add(MapToCore(companyDb));
            }

            return companiesCore;
        }

        private CompanyCoreModel MapToCore(Company companyDb)
        {
            return new CompanyCoreModel() {
                Address1 = companyDb.Address1,
                Address2 = companyDb.Address2,
                City = companyDb.City,
                Description = companyDb.CompanyDescription,
                CompanyId = companyDb.CompanyId,
                Name = companyDb.CompanyName,
                CreatedDate = companyDb.CreatedDate,
                ModifiedDate = companyDb.ModifiedDate,
                State = companyDb.State,
                Zip = companyDb.Zip,
                UserId = companyDb.UserId,
            };
        }

        private Company MapToDb(CompanyCoreModel companyCore)
        {
            return new Company()
            {
                Address1 = companyCore.Address1,
                Address2 = companyCore.Address2,
                City = companyCore.City,
                CompanyDescription = companyCore.Description,
                CompanyId = companyCore.CompanyId,
                CompanyName = companyCore.Name,
                CreatedDate = companyCore.CreatedDate,
                ModifiedDate = companyCore.ModifiedDate,
                State = companyCore.State,
                Zip = companyCore.Zip,
                UserId = companyCore.UserId,
            };
        }
    }
}
