using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.UploadManager
{
    public interface IUploadManager:IManager
    {
        Task UploadFile(FileUpload uploadReq);
        Task<IList<Base64Document>> getBase64Documents(DocumentRetrivaltDTO document);
    }
}
