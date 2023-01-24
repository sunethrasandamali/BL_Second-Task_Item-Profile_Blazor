using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.UploadManager
{
    public class UploadManager : IUploadManager
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public UploadManager(HttpClient httpClient,IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }

        public async Task<IList<Base64Document>> getBase64Documents(DocumentRetrivaltDTO document)
        {
            IList<Base64Document> docs = new List<Base64Document>();
            try
            {

                //var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetBase64DocumentsEndPoint, document);
                //await response.Content.LoadIntoBufferAsync();
                //string content = response.Content.ReadAsStringAsync().Result;
                //docs = JsonConvert.DeserializeObject<IList<Base64Document>>(content);



            }
            catch (Exception exp)
            {

            }
            return docs;
        }

        public async Task UploadFile(FileUpload uploadReq)
        {
            try
            {
                //var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FileUploadEndPoint, uploadReq);
            }
            catch (Exception exp)
            {

            }
        }
    }
}
