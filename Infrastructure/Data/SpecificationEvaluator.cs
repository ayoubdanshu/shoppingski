using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery,
        ISpecification<TEntity> spec )
        {
            var query = InputQuery;

            if(spec.Criteria != null)
            {
               query = query.Where(spec.Criteria);
            }
 
            if(spec.OrderBy != null)
            {
               query = query.OrderBy(spec.OrderBy);
            }
            
            if(spec.OrderByDescending != null)
            {
               query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IsPAgingEnabled)
            {
                query = query.Skip(spec.skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query,(current, Include) => current.Include(Include));
            
            return query;
        } 
    }
}