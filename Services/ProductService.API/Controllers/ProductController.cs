using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProductService.API.Model;
using ProductService.API.Model.Dto;
using ProductService.API.RabbitMq;
using ProductService.API.RabbitMq.Events;
using ProductService.API.Services;

namespace ProductService.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private ResponseDto _response;
        private readonly IPublishService _publishService;

        public ProductController(IProductService productService, IPublishService publishService)
        {
            _productService = productService;
            _response = new ResponseDto();
            _publishService = publishService;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var datas = _productService.GetAll().ToList();
                _response.Result = datas;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);           
        }


        [HttpGet("{id}")]
        public async Task<ResponseDto> Get([FromRoute] int id)
        {
            try
            {
                var data = await _productService.GetByIdAysnc(id);
                _response.Result = data;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] ProductDto product)
        {
            Product productNew = new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
            };

            try
            {
                bool isSucces = await _productService.AddAysnc(productNew);
                if (isSucces) 
                { 
                    await _productService.SaveAysnc();
                    _response.Result = productNew;

                    ProductCreatedEvent eventData = new() { ProductId = productNew.Id };

                    _publishService.Publish(eventData, nameof(ProductCreatedEvent));

                }


                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] ProductDto product)
        {

            try
            {
                Product data = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                };

                _productService.Update(data);
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpDelete("{id}")]
        public async Task<ResponseDto> Delete([FromRoute] int id)
        {

            try
            {
                var data = await _productService.GetByIdAysnc(id);
                _productService.Delete(data);

                _response.Result = data;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
