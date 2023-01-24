using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.ExtendedAttributes.Queries.Export;
using BlueLotus360.Com.Application.Features.ExtendedAttributes.Queries.GetAll;
using BlueLotus360.Com.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlueLotus360.Com.Client.Infrastructure.Extensions;
using BlueLotus360.Com.Domain.Contracts;
using BlueLotus360.Com.Shared.Wrapper;

namespace BlueLotus360.Com.Infrastructure.Managers.ExtendedAttribute
{
    public class ExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute>
        : IExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly HttpClient _httpClient;

        public ExtendedAttributeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<IResult<TId>> DeleteAsync(TId id)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<IResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>> GetAllAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<IResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> GetAllByEntityIdAsync(TEntityId entityId)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<IResult<TId>> SaveAsync(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> request)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}