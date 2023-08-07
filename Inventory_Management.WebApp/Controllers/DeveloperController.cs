using Core.Application.Enums;
using Core.Application.Interfaces.Services;
using Core.Application.ViewModels.Category;
using Core.Application.ViewModels.DefaultProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Inventory_Management.WebApp.Controllers
{
    [Authorize(Roles = "Developer")]

    public class DeveloperController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly IDefaultProductService _productService;

        public DeveloperController(ICategoryService categoryService, IDefaultProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }


        public async Task<IActionResult> Product(bool hasError, string message)
        {
            ViewBag.HasError = hasError;
            ViewBag.Message = message;
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Products = await _productService.GetAllWithInclude();

            return View(new SaveDefaultProductViewModel() { ModalState = 0});
        }

        [HttpPost]
        public async Task<IActionResult> Product(SaveDefaultProductViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.HasError = false;
                viewModel.ModalState = 1;
                return View(viewModel);
            }

            viewModel.CreatedBy = "Develop";
            SaveDefaultProductViewModel result = await _productService.AddAsync(viewModel);
            if (result.Id != 0 && result != null)
            {
                result.Img = UploadFile(viewModel.File, result.Id);
                await _productService.UpdateAsync(result,result.Id);
            }

            bool hasError = true;
            string message = "Producto agregado correstamente.";

            return RedirectToRoute(new { Controller = "Developer", Action = "Product", hasError,message });

        }



        public async Task<IActionResult> DeleteProduct(int id)
        {

            await _productService.DeleteAsync(id);

            string basePath = $"/img/products/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if(Directory.Exists(path))
            {
                DirectoryInfo directory = new(path); 

                foreach(FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                foreach(DirectoryInfo folder in directory.GetDirectories())
                {
                   folder.Delete(); 
                }

                Directory.Delete(path);
            }

            bool hasError = true;
            string message = "Producto eliminado correctamente!";
            return RedirectToRoute(new { Controller = "Developer", Action = "Product",hasError,message });

        }


        public async Task<IActionResult> EditProduct(int id)
        {
            ViewBag.HasError = false;
            ViewBag.Categories = await _categoryService.GetAllAsync();

            DefaultProductViewModel viewModel = await _productService.GetById(id);
            SaveDefaultProductViewModel saveDefaultProductViewModel = new()
            {
                BarCode = viewModel.BarCode,
                Description = viewModel.Description,
                Id = viewModel.Id,
                Name = viewModel.Name,
                CategoryId = viewModel.CategoryId,
                Img = viewModel.Img,
                ModalState = 2
            };

            return View("Product", saveDefaultProductViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(SaveDefaultProductViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {

                ViewBag.HasError = false;
                ViewBag.Categories = await _categoryService.GetAllAsync();
                viewModel.ModalState = 2;
                return View("Product", viewModel);
            }


            viewModel.Img = UploadFile(viewModel.File, viewModel.Id, true, viewModel.Img);
            viewModel.CreatedBy = "Developer";
            await _productService.UpdateAsync(viewModel, viewModel.Id);

            bool hasError = true;
            string message = "Cambios guardados correctamente!";

            return RedirectToRoute(new { Controller = "Developer", Action = "Product",hasError,message });

        }




        public async Task<IActionResult> Category(bool hasError, string message)
        {
            ViewBag.HasError = hasError;
            ViewBag.Message = message;
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(new SaveCategoryViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Category(SaveCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HasError = false;
                viewModel.ModelState = 1;
                return View("Category",viewModel);
            }

            await _categoryService.AddAsync(viewModel);
            bool hasError = true;
            string message = "Categoria guardada correctamente!";

            return RedirectToRoute(new { Controller = "Developer", Action = "Category",hasError,message });
        }


        public async Task<IActionResult> UpdateCategory(int id)
        {
            CategoryViewModel category = await _categoryService.GetById(id);
            SaveCategoryViewModel viewModel = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            ViewBag.HasError = false;

            return View("Category", viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(SaveCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HasError = false;
                return View("Category", viewModel);
            }

            await _categoryService.UpdateAsync(viewModel, viewModel.Id);

            bool hasError = true;
            string message = "Cambios guardados correctamente!";

            return RedirectToRoute(new { Controller = "Developer", Action = "Category",hasError,message });

        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);

            bool hasError = true;
            string message = "Categoria eliminada correctamente!";

            return RedirectToRoute(new { Controller = "Developer", Action = "Category",hasError,message });

        }

        [HttpPost]
        public async Task<IActionResult> ProductResult(string serchWord) {
            var result = await _productService.GetAllWithInclude();
            ViewBag.HasError = false;
            ViewBag.Message = "";
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Products = result.Where(p => p.Name == serchWord).ToList();

            return View(new SaveDefaultProductViewModel() { ModalState = 0 });
        }


        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/img/products/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }
    }
}
