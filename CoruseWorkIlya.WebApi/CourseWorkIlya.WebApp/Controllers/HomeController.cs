using CourseWork.WebApp.Models;
using CourseWork.WebApp.Models.DTOs;
using CourseWork.WebApp.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;


namespace CourseWork.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<CategoryDTO> list = new List<CategoryDTO>();
            var response = await _productService.GetAllAsync<APIResponse>();

            if (response is not null && response.IsSuccess)
                list = JsonConvert.DeserializeObject<IEnumerable<CategoryDTO>>
                    (Convert.ToString(response.Result))!;

            return View(list);
        }

        [HttpGet]
        [Authorize(Policy = "")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id);

            if (response is not null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetProductList()
        {
            var response = await _productService.GetAllAsync<APIResponse>();
            IEnumerable<ProductDTO> products = new List<ProductDTO>();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(response.Result));
            }

            return Json(new { data = products });
        }
        #endregion API CALL
    }
}
