using AutoMapper;
using CourseWork.WebApi.Data.Repository.IRepository;
using CourseWork.WebApi.Models;
using CourseWork.WebApi.Models.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseWork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDTO> _categoryCreateDtoValidator;
        private readonly IValidator<CategoryUpdateDTO> _categoryUpdateDtoValidator;

        public CategoryController(
            IUnitOfWork uof,
            IMapper mapper,
            IValidator<CategoryUpdateDTO> categoryUpdateDtoValidator,
            IValidator<CategoryCreateDTO> categoryCreateDtoValidator)
        {
            _uof = uof;
            _mapper = mapper;
            _response = new();
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
            _categoryCreateDtoValidator = categoryCreateDtoValidator;
        }

        [Authorize(Policy = "ApiScope")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try 
            {
                IEnumerable<Category> categoryList = await _uof.Categories.GetAllAsync();

                _response.Result = _mapper
                    .Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categoryList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }

            return _response;

        }

        [Authorize(Policy = "ApiScope")]
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Id must be greater than 0" };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var category = await _uof.Categories
                    .GetAsync(c => c.Id == id);

                if(category is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<CategoryDTO>(category);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.Message };
            }

            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO createDTO)
        {
            try
            {

                if (createDTO is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Category is null" };
                    _response.IsSuccess = false;
                }

                var validationResult = _categoryCreateDtoValidator.Validate(createDTO);
                if (!validationResult.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = validationResult.Errors
                        .Select(m => m.ErrorMessage)
                        .ToList();
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }

                if (await _uof.Categories.GetAsync(c => c.Name.ToLower() == createDTO.Name.ToLower()) is not null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "Category is already exists" };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
               
                Category category = _mapper.Map<Category>(createDTO);
                category.CreatedDate = DateTime.Now;
                await _uof.Categories.CreateAsync(category);
                await _uof.Commit();
                _response.Result = _mapper.Map<CategoryDTO>(category);
                _response.StatusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetCategory", new {id = category.Id}, _response);
                
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
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

                Category category = await _uof.Categories
                    .GetAsync(c => c.Id == id);

                if (category is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { $"Category with Id:{id} not exists" };
                    _response.IsSuccess = false;

                    return BadRequest(_response);
                }

                await _uof.Categories.RemoveAsync(category);
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
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateCategory([FromBody] CategoryUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO is null)
                {
                    _response.ErrorMessages = new List<string> { "Category is null" };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var validationResult = _categoryUpdateDtoValidator.Validate(updateDTO);
                if (!validationResult.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList();
                    return BadRequest(_response);
                }

                Category category = _mapper.Map<Category>(updateDTO);
                await _uof.Categories.UpdateAsync(category);
                await _uof.Commit();

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch(Exception ex) 
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }

            return _response;
        }
    }
}
