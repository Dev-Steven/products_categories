using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using products_categories.Models;

namespace products_categories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Product> allProds = dbContext.Products.ToList();
            ViewBag.allProds = allProds;
            return View("Index");
        }

        [HttpGet("category")]
        public IActionResult Category()
        {
            List<Category> allCats = dbContext.Categories.ToList();
            ViewBag.allCats = allCats;
            return View("Category");
        }

        [HttpPost("addproduct")]
        public IActionResult AddProduct(Product newProd)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newProd);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<Product> allProds = dbContext.Products.ToList();
                ViewBag.allCats = allProds;
                return View("Index");
            }
        }

        [HttpPost("addcategory")]
        public IActionResult AddCategory(Category newCat)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newCat);
                dbContext.SaveChanges();
                return RedirectToAction("Category");
            }
            else
            {
                List<Category> allCats = dbContext.Categories.ToList();
                ViewBag.allCats = allCats;
                return View("Category");
            }
        }

        [HttpGet("product/{id}")]
        public IActionResult ProductDetail(int id)
        {
            // Getting the name of the product
            Product prod = dbContext.Products
                .Include(p => p.Groups)
                    .ThenInclude(c => c.Category)
                .FirstOrDefault(p => p.ProductId == id);

            // List of all categories
            List<Category> allCats = dbContext.Categories
                .ToList();
            // List of non associated categories
            List<Category> catsNotAdded = new List<Category>();

            // Want to find all categories that are not in the groups list
            // Compare groups with allCats
            // If item in group matches item in allCats don't add to catsNotAdded
            // If item doesn't match then add to catsNotAdded.

            foreach(var cat in allCats)
            {
                int i=0;
                bool found = false;
                while( i < prod.Groups.Count )
                {
                    if(cat.CategoryId == prod.Groups[i].CategoryId)
                    {
                        found = true;
                        break;
                    }
                    i++;
                }
                if(!found)
                {
                    catsNotAdded.Add(cat);
                }
            }

            ViewBag.catsNotAdded = catsNotAdded;

            return View("ProductDetail", prod);
        }

        [HttpPost("addcategorytoproduct")]
        public IActionResult AddCategoryToProduct(Association newAss)
        {
            Product product = dbContext.Products.FirstOrDefault(p => p.ProductId == newAss.ProductId);
            if(ModelState.IsValid)
            {
                dbContext.Associations.Add(newAss);
                dbContext.SaveChanges();
                return RedirectToAction("ProductDetail", new {id = product.ProductId});
            }
            else
            {
                return View("ProductDetail", new {id = product.ProductId});
            }
        }

        [HttpGet("category/{id}")]
        public IActionResult CategoryDetail(int id)
        {
            // Getting the name of the product
            Category cat = dbContext.Categories
                .Include(c => c.Items)
                    .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.CategoryId == id);

            // List of all categories
            List<Product> allProds = dbContext.Products
                .ToList();
            // List of non associated categories
            List<Product> prodsNotAdded = new List<Product>();

            // Want to find all categories that are not in the groups list
            // Compare groups with allCats
            // If item in group matches item in allCats don't add to catsNotAdded
            // If item doesn't match then add to catsNotAdded.

            foreach(var prod in allProds)
            {
                int i=0;
                bool found = false;
                while( i < cat.Items.Count )
                {
                    if(prod.ProductId == cat.Items[i].ProductId)
                    {
                        found = true;
                        break;
                    }
                    i++;
                }
                if(!found)
                {
                    prodsNotAdded.Add(prod);
                }
            }

            ViewBag.prodsNotAdded = prodsNotAdded;

            return View("CategoryDetail", cat);
        }

        [HttpPost("addproducttocategory")]
        public IActionResult AddProductToCategory(Association newAss)
        {
            Category category = dbContext.Categories.FirstOrDefault(c=> c.CategoryId == newAss.CategoryId);
            if(ModelState.IsValid)
            {
                dbContext.Associations.Add(newAss);
                dbContext.SaveChanges();
                return RedirectToAction("CategoryDetail", new {id = category.CategoryId});
            }
            else
            {
                return View("CategoryDetail", new {id = category.CategoryId});
            }
        }

    }
}
