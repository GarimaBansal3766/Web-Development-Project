using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Entity.Online_Shoppng.Models.Entity;
using Online_Shoppng.Models.Repository;
using Online_Shoppng.Models.Services;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Online_Shoppng.Controllers
{
    public class OnlineController : Controller
    {
        private IfCategory _ifCategory;
        private IfProducts _ifProducts;

        public OnlineController(IfCategory ifCategory, IfProducts ifProducts)
        {
            _ifCategory = ifCategory;
            _ifProducts = ifProducts;
        }

        public IActionResult New_Product()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> AddCategory([FromForm] Category category, IFormFile categoryimage)
        {
            //if (categoryimage != null && categoryimage.Length > 0)
            //{
            //    var fileName = Path.GetFileName(categoryimage.FileName);
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await categoryimage.CopyToAsync(stream);
            //    }

            //    category.Categoryimage = "/images/" + fileName;
            //}
            if (categoryimage != null && categoryimage.Length > 0)
            {
                var extension = Path.GetExtension(categoryimage.FileName); // Get file extension
                var uniqueFileName = Guid.NewGuid().ToString() + extension; // Unique file name
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await categoryimage.CopyToAsync(stream);
                }

                category.Categoryimage = "/images/" + uniqueFileName;
            }

            int i = _ifCategory.AddCategory(category);
            if (i > 0)
            {
                return Json("successful");
            }
            else
            {
                return Json("failed");
            }
        }
       
        [HttpGet]
        public JsonResult DeleteCategory(int id)
        {
            int i = _ifCategory.DeleteCategory(id);
            if (i > 0)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }



        [HttpPost]
        public async Task<JsonResult> AddProduct([FromForm] Product product, IFormFile productimage1, IFormFile productimage2, IFormFile productimage3)
        {
            if (productimage1 != null && productimage1.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productimage1.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage1.CopyToAsync(stream);
                }
                product.Productimage1 = "/images/" + uniqueFileName;
            }

            if (productimage2 != null && productimage2.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productimage2.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage2.CopyToAsync(stream);
                }
                product.Productimage2 = "/images/" + uniqueFileName;
            }

            if (productimage3 != null && productimage3.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productimage3.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage3.CopyToAsync(stream);
                }

                product.Productimage3 = "/images/" + uniqueFileName;
            }

            int i = _ifProducts.AddProduct(product);
            if (i > 0)
            {
                return Json("product add successful");
            }
            else
            {
                return Json("product add failed");
            }
        }

        [HttpGet]
        public JsonResult GetAllCategories()
        {
            var categories = _ifCategory.GetAllCategories();
            return Json(categories);
        }
        [HttpGet]

        public JsonResult SearchCategory(int id)
        {
            Category CatObj = _ifCategory.SearchCategory(id);
            return Json(CatObj);
        }

        
        [HttpPost]
        public async Task<JsonResult> UpdateCategory([FromForm] Category category, IFormFile categoryImage)
        {
            if (categoryImage != null && categoryImage.Length > 0)
            {
                // Save the uploaded file
                var fileName = Path.GetFileName(categoryImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await categoryImage.CopyToAsync(stream);
                }

                // Update the image path in the model
                category.Categoryimage = "/Images/" + fileName;
            }

            // Call repository to update category
            int result = _ifCategory.UpdateCategory(category);

            if (result > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [HttpGet]
        public JsonResult GetProducts()
        {
            var Products = _ifProducts.GetProducts();
            return Json(Products);
        }

        [HttpGet]
        public JsonResult DeleteProduct(int id)
        {
            bool result = _ifProducts.DeleteProduct(id);
            return Json(new { success = result });
        }
        [HttpGet]

        public JsonResult SearchProduct(int id)
        {
            Product ProObj = _ifProducts.SearchProduct(id);
            return Json(ProObj);
        }
        [HttpPost]

        public async Task<JsonResult> UpdateProduct([FromForm] Product product, IFormFile productimage1,IFormFile productimage2,IFormFile productimage3)
        {
            if (productimage1 != null && productimage1.Length > 0)
            {
                // Save the uploaded file
                var fileName = Path.GetFileName(productimage1.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage1.CopyToAsync(stream);
                }

                // Update the image path in the model
                product.Productimage1 = "/Images/" + fileName;
            }
            if (productimage2 != null && productimage2.Length > 0)
            {
                // Save the uploaded file
                var fileName = Path.GetFileName(productimage2.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage2.CopyToAsync(stream);
                }

                // Update the image path in the model
                product.Productimage2 = "/Images/" + fileName;
            }
            if (productimage3 != null && productimage3.Length > 0)
            {
                // Save the uploaded file
                var fileName = Path.GetFileName(productimage3.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productimage3.CopyToAsync(stream);
                }

                // Update the image path in the model
                product.Productimage3 = "/Images/" + fileName;
            }

            // Call repository to update category
            int result = _ifProducts.UpdateProduct(product);

            if (result > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
