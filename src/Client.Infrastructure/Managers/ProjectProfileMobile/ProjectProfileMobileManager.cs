using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.ProjectProfileMobile
{
    public class ProjectProfileMobileManager : IProjectProfileMobileManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        public ProjectProfileMobileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IList<ProjectProfileList>> GetProjectProfileList(ProjectProfileRequest request)
        {
            List<ProjectProfileList> projectProfileList = new List<ProjectProfileList>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetProfileList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                projectProfileList = JsonConvert.DeserializeObject<List<ProjectProfileList>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                projectProfileList = new List<ProjectProfileList>();
            }
            finally
            {

            }

            return projectProfileList;
        }

        public async Task<ProjectProfileList> InsertProjectList(ProjectProfileList request)
        {
            ProjectProfileList response = new ProjectProfileList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.Insertprofile, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ProjectProfileList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ProjectProfileList();
            }
            finally
            {

            }
            return response;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        public async Task<ProjectProfileList> UpdateProjectList(ProjectProfileList request)
        {
            ProjectProfileList response = new ProjectProfileList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateProfile, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ProjectProfileList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ProjectProfileList();
            }
            finally
            {

            }
            return response;
        }
    }
}
