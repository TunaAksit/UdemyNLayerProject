﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerProject.API.DTOs;
using NLayerProject.API.Filters;
using NLayerProject.Core.Models;
using NLayerProject.Core.Services;

namespace NLayerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;

        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           
            var products = await _productService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));


        }
        
        
        [ServiceFilter(typeof(ProductNotFoundFilter))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //idleri kontrol etmek için filter kullanıcaz
            var product = await _productService.GetByIdAsync(id);
            return Ok(_mapper.Map<ProductDto>(product));
        }

        
        [ServiceFilter(typeof(ProductNotFoundFilter))]
        [HttpGet("{id}/category")]
        public async Task<IActionResult>GetWithCategoryById(int id)
        {
            var product = await _productService.GetWithCategoryByIdAsync(id);
            return Ok(_mapper.Map<ProductWithCategoryDto>(product));

        }


        // [ValidationFilter] i merkezilerştirmek için starupa koyduk
        [HttpPost]
        public async Task <IActionResult> Save (ProductDto productDto)
        {

            var newproduct = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            return Created(string.Empty, _mapper.Map<ProductDto>(newproduct));

        }
        [HttpPut]
        public IActionResult Update (ProductDto productDto)
        {

            //uygun olanı UpdatteProductDto oluşturup bunun Idsine requid olmaktır
            //if (string.IsNullOrEmpty(productDto.Id.ToString()) || productDto.Id<=0)
            //{
            //    throw new Exception("Id alanı gereklidir");
            //}
            
            var product = _productService.Update(_mapper.Map<Product>(productDto));
            return NoContent();

        }

        [ServiceFilter(typeof(ProductNotFoundFilter))]
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var product = _productService.GetByIdAsync(id).Result;
            _productService.Remove(product);
            return NoContent();
        }

    }
}
