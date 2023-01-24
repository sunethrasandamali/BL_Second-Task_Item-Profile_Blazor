using System.Collections.Generic;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.DocumentTypes.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.DocumentTypes.Queries.GetAll;
using BlueLotus360.Com.Shared.Wrapper;

namespace BlueLotus360.Com.Infrastructure.Managers.Misc.DocumentType
{
    public interface IDocumentTypeManager : IManager
    {
        Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}