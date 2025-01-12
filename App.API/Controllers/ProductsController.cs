using App.API.Filters;
using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

public class ProductsController(IProductService productService) : CustomBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());

    [HttpGet("{pageNumber:int}/{pageSize:int}")]
    public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) => CreateActionResult(await productService.GetPagedAllListAsync(pageNumber, pageSize));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));

    [HttpPatch("UpdateStock")]
    public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));

    [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
}
