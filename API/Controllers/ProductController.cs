using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class ProductController : BaseApiController
    {

        private readonly IGenericRepository<Product> _ProductRepo;
        private readonly IGenericRepository<ProductBrand> _ProductBrandRepo;
        private readonly IGenericRepository<ProductType> _ProductTypeRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> ProductsRepo,
         IGenericRepository<ProductBrand> ProductBrandRepo, IGenericRepository<ProductType>
         ProductTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _ProductTypeRepo = ProductTypeRepo;
            _ProductBrandRepo = ProductBrandRepo;
            _ProductRepo = ProductsRepo;


        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> getProducts(
           [FromQuery]ProductSpecParams ProductParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(ProductParams);

            var CountSpec = new ProductWithFiltersForCountSpecification(ProductParams);

            var totalItems = await _ProductRepo.CountAsync(CountSpec);

            var Products = await _ProductRepo.ListAsync(spec);

             var Data = _mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            return Ok(new Pagination<ProductToReturnDto>(ProductParams.PageIndex, 
            ProductParams.PageSize, totalItems, Data  ));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> getProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _ProductRepo.GetEntityWithSpec(spec);
            
            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _ProductBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _ProductTypeRepo.ListAllAsync());
        }
    }
}