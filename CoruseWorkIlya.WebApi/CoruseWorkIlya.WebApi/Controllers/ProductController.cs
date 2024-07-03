using AutoMapper;
using CourseWork.WebApi.Data.Repository.IRepository;
using CourseWork.WebApi.Models;
using CourseWork.WebApi.Models.DTOs;
using CourseWork.Utility;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseWork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductUpdateDTO> _productUpdateDtoValidator;
        private readonly IValidator<ProductCreateDTO> _productCreateDtoValidator;
        public ProductController(
            IUnitOfWork uof,
            IMapper mapper,
            IValidator<ProductUpdateDTO> productUpdateValidator,
            IValidator<ProductCreateDTO> productCreateValidator)
        {
            _uof = uof;
            _mapper = mapper;
            _response = new();
            _productUpdateDtoValidator = productUpdateValidator;
            _productCreateDtoValidator = productCreateValidator;
        }

        [Authorize(Policy = "ApiScope")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            try
            {
                IEnumerable<Product> productList = await _uof.Products.GetAllAsync(
                    includeProperties: $"{WebConstants.CategoryName}");

                _response.Result = _mapper
                    .Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(productList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }

            return _response;

        }

        [Authorize(Policy = "ApiScope")]
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Id must be greater than 0" };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var product = await _uof.Products
                    .GetAsync(c => c.Id == id,
                        includeProperties: $"{WebConstants.CategoryName}");

                if (product is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.Message };
            }

            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO createDTO)
        {
            try
            {

                if (createDTO is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Product is null" };
                    _response.IsSuccess = false;
                }

                var validationResult = _productCreateDtoValidator.Validate(createDTO);
                if (!validationResult.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = validationResult.Errors
                        .Select(m => m.ErrorMessage)
                        .ToList();
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }

                if (await _uof.Products.GetAsync(p => p.Name.ToLower() == createDTO.Name.ToLower()) is not null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Product is already exists" };

                    return BadRequest(_response);
                }

                if(await _uof.Categories.GetAsync(c => c.Id == createDTO.CategoryId) is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Category is not exists" };
                    _response.IsSuccess = false;
                }

                Product product = _mapper.Map<Product>(createDTO);
                product.CreatedDate = DateTime.Now;
                await _uof.Products.CreateAsync(product);
                await _uof.Commit();
                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetProduct", new { id = product.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateProduct([FromBody] ProductUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO is null)
                {
                    _response.ErrorMessages = new List<string> { "Product is null" };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var validationResult = _productUpdateDtoValidator.Validate(updateDTO);
                if (!validationResult.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList();
                    return BadRequest(_response);
                }
                if (await _uof.Products.GetAsync(p => p.Name.ToLower() == updateDTO.Name.ToLower()) is not null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Product is already exists" };

                    return BadRequest(_response);
                }

                if (await _uof.Categories.GetAsync(c => c.Id == updateDTO.CategoryId) is null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Category is not exists" };
                }


                Product product = _mapper.Map<Product>(updateDTO);
                await _uof.Products.UpdateAsync(product);
                await _uof.Commit();

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }

            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Id must be greater than 0" };
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }

                Product product = await _uof.Products
                    .GetAsync(c => c.Id == id);

                if (product is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Product with Id:{id} not exists" };
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }

                await _uof.Products.RemoveAsync(product);
                await _uof.Commit();

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }

            return _response;
        }
    }
}
