using BlueLotus360.Com.Infrastructure.Models.Audit;
using BlueLotus360.Com.Application.Specifications.Base;

namespace BlueLotus360.Com.Infrastructure.Specifications
{
    public class AuditFilterSpecification : HeroSpecification<Audit>
    {
        public AuditFilterSpecification(string userId, string searchString, bool searchInOldValues, bool searchInNewValues)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.TableName.Contains(searchString) || searchInOldValues && p.OldValues.Contains(searchString) || searchInNewValues && p.NewValues.Contains(searchString)) && p.UserId == userId;
            }
            else
            {
                Criteria = p => p.UserId == userId;
            }
        }
    }
}