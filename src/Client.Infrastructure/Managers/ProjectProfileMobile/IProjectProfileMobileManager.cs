using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.ProjectProfileMobile
{
    public interface IProjectProfileMobileManager : IManager
    {
        bool IsExceptionthrown();
        Task<IList<ProjectProfileList>> GetProjectProfileList(ProjectProfileRequest request);

        Task<ProjectProfileList> InsertProjectList(ProjectProfileList request);
        Task<ProjectProfileList> UpdateProjectList(ProjectProfileList request);

    }
}