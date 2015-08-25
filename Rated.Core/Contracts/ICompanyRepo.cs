using Rated.Core.Models.Project;
using Rated.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rated.Core.Contracts
{
    public interface ICompanyRepo
    {
        List<CompanyCoreModel> GetCompanyForUser(Guid userId);
        void AddNewCompany(CompanyCoreModel company);
    }
}
