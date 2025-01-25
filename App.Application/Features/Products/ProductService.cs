﻿using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Application.ServiceBus;
using App.Domain.Entities;
using App.Domain.Events;
using AutoMapper;
using System.Net;

namespace App.Application.Features.Products;

public class ProductService(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cacheService,
    IServiceBus serviceBus  
    ) : IProductService
{
    private const string ProductListCacheKey = "ProductListCacheKey";

    public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
    {
        var products = await productRepository.GetTopPriceProductsAsync(count);

        var productsAsDto = mapper.Map<List<ProductDto>>(products);

        return new ServiceResult<List<ProductDto>>
        {
            Data = productsAsDto
        };
    }

    public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
    {
        var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

        if (productListAsCached is not null)
            return ServiceResult<List<ProductDto>>.Success(productListAsCached);

        var product = await productRepository.GetAllAsync();
        var productAsDto = mapper.Map<List<ProductDto>>(product);

        await cacheService.SetAsync(ProductListCacheKey, productAsDto, TimeSpan.FromMinutes(5));

        return ServiceResult<List<ProductDto>>.Success(productAsDto);
    }

    public async Task<ServiceResult<ProductDto>> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            return ServiceResult<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);

        var productAsDto = mapper.Map<ProductDto>(product);

        return new ServiceResult<ProductDto> { Data = productAsDto };
    }

    public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
    {
        //async manuel service business check
        var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);
        if (anyProduct)
            return ServiceResult<CreateProductResponse>.Fail("Aynı ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);

        var product = mapper.Map<Product>(request);

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        await serviceBus.PublishAsync(new ProductAddedEvent(product.Id,product.Name,product.Price));

        return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
    {
        var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id == id);

        if (isProductNameExist)
            return ServiceResult.Fail("Aynı ürün ismi veritabanında bulunmaktadır.", HttpStatusCode.BadRequest);

        var product = mapper.Map<Product>(request);
        product.Id = id;

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
            return ServiceResult.Fail("Güncellenecek ürün bulunamadı", HttpStatusCode.NotFound);

        product.Stock = request.stock;

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        productRepository.Delete(product!);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
    {
        var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);

        var productsAsDto = mapper.Map<List<ProductDto>>(products);

        return ServiceResult<List<ProductDto>>.Success(productsAsDto);
    }
}