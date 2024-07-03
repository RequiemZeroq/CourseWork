using AutoMapper;
using CourseWork.WebApp.Models;
using CourseWork.WebApp.Models.DTOs;
using CourseWork.WebApp.Models.VMs;
using CourseWork.WebApp.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using CourseWork.Utility;
using Microsoft.AspNetCore.Authorization;


namespace CourseWork.WebApp.Controllers
{
    [Authorize(Policy = WebConstants.AdminRole)]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public ProductController(
            IProductService productService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductDTO> list = new List<ProductDTO>();

            var response = await _productService.GetAllAsync<APIResponse>();

            if(response is not null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>
                    (Convert.ToString(response.Result))!;
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ProductCreateVM vm = new ProductCreateVM();
            var response = await _categoryService.GetAllAsync<APIResponse>();

            if(response is not null && response.IsSuccess)
            {
                var categoryList = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>
                    (Convert.ToString(response.Result))!;

                vm.CategoryListItems = categoryList.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateAsync<APIResponse>(model.ProductCreateModel);

                if (response is not null && response.IsSuccess)
                {
                    TempData[WebConstants.SuccessAlert] = "Product created successfully";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var response = await _categoryService.GetAllAsync<APIResponse>();

                if (response is not null && response.IsSuccess)
                {
                    var categoryList = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>
                        (Convert.ToString(response.Result))!;

                    model.CategoryListItems = categoryList.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    });
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductUpdateVM vm = new ProductUpdateVM();
            var categoriesResponse = await _categoryService.GetAllAsync<APIResponse>();

            if (categoriesResponse is not null && categoriesResponse.IsSuccess)
            {
                var categoryList = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>
                    (Convert.ToString(categoriesResponse.Result))!;

                vm.CategoryListItems = categoryList.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
            }

            var productResponse = await _productService.GetAsync<APIResponse>(id);
            if(productResponse is not null && categoriesResponse.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>
                    (Convert.ToString(productResponse.Result))!;

                vm.ProductUpdateModel = _mapper.Map<ProductUpdateDTO>(product);  
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateAsync<APIResponse>(model.ProductUpdateModel);

                if (response is not null && response.IsSuccess)
                {
                    TempData[WebConstants.SuccessAlert] = "Product edited successfully";
                    return RedirectToAction(nameof(Index));
                }
                    
            }
            else
            {
                var response = await _categoryService.GetAllAsync<APIResponse>();

                if (response is not null && response.IsSuccess)
                {
                    var categoryList = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>
                        (Convert.ToString(response.Result))!;

                    model.CategoryListItems = categoryList.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    });
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id);

            if (response is not null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDTO model)
        {

            var response = await _productService.DeleteAsync<APIResponse>(model.Id);

            if (response is not null && response.IsSuccess)
            {
                TempData[WebConstants.SuccessAlert] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
