using BlueLotus360.Com.Application.Features.Documents.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.Documents.Queries.GetAll;
using BlueLotus360.Com.Application.Requests.Documents;
using BlueLotus360.Com.Shared.Wrapper;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.Documents.Queries.GetById;

namespace BlueLotus360.Com.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}