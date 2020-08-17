using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("API/[Controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _Repo;
        public ProductController(IProductRepository Repo)
        {
            _Repo = Repo;

        }

        [HttpGet]
        public async Task<ActionResult<List<Products>>> getProducts()
        {
            var Products = await _Repo.GetProductsAsync();

            return Ok(Products);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> getProduct(int id)
        {
            return await _Repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _Repo.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _Repo.GetProductTypesAsync());
        }
    }
}