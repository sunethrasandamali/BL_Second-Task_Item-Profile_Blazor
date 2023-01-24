using BlueLotus360.Com.Application.Features.Documents.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.Documents.Queries.GetAll;
using BlueLotus360.Com.Application.Requests.Documents;
using BlueLotus360.Com.Client.Infrastructure.Extensions;
using BlueLotus360.Com.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.Documents.Queries.GetById;

namespace BlueLotus360.Com.Infrastructure.Managers.Misc.Document
{
    public class DocumentManager : IDocumentManager
    {
        private readonly HttpClient _httpClient;

        public DocumentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            return null;
        }

        public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
        {
            return null;
        }

        public async Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request)
        {
            return null;
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentCommand request)
        {
            return null;
        }
    }
}