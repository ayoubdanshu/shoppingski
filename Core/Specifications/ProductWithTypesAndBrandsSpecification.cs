using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams ProductParams)
          : base(x => 
               (string.IsNullOrEmpty(ProductParams.Search) || x.Name.ToLower().Contains
               (ProductParams.Search)) &&
               (!ProductParams.BrandId.HasValue || x.ProductBrandId ==ProductParams.BrandId)&&
               (!ProductParams.TypeId.HasValue || x.ProductTypeId == ProductParams.TypeId)
        )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(ProductParams.PageSize * (ProductParams.PageIndex -1)
            ,ProductParams.PageSize);
            
            if (!string.IsNullOrEmpty(ProductParams.sort))
            {
                switch (ProductParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);   
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;    
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id)
        : base(x => x.id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}