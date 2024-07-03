using AutoMapper;
using CourseWork.Utility;
using CourseWork.WebApp.Models;
using CourseWork.WebApp.Models.DTOs;
using CourseWork.WebApp.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourseWork.WebApp.Controllers
{
    [Authorize(Policy = WebConstants.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper; 

        public CategoryController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryDTO> list = new List<CategoryDTO>();

            var response = await _categoryService.GetAllAsync<APIResponse>();

            if(response is not null && response.IsSuccess)
                list = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>(Convert.ToString(response.Result))!;

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _categoryService.CreateAsync<APIResponse>(model);

                if(response is not null && response.IsSuccess )
                {
                    TempData[WebConstants.SuccessAlert] = "Category created successfully";
                    return RedirectToAction("Index");
                }
            }

            return View(model); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _categoryService.GetAsync<APIResponse>(id);

            if(response is not null && response.IsSuccess)
            {
                CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<CategoryUpdateDTO>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(CategoryUpdateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _categoryService.UpdateAsync<APIResponse>(model);

                if (response is not null && response.IsSuccess)
                {
                    TempData[WebConstants.SuccessAlert] = "Category edited successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.GetAsync<APIResponse>(id);

            if (response is not null && response.IsSuccess)
            {
                CategoryDTO model = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryDTO model)
        {
            
            var response = await _categoryService.DeleteAsync<APIResponse>(model.Id);

            if(response is not null && response.IsSuccess)
            {
                TempData[WebConstants.SuccessAlert] = "Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
