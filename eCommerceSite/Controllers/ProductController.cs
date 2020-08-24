using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //Get sll producrs from databse
            //List<Product> products = _context.Products.ToList();
            List<Product> products = (from p in _context.Products
                                      select p).ToList();

            //Send list of products to view to be diplayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //Add to DB
                _context.Products.Add(p); // Creates insert querey
                _context.SaveChanges(); // executes query

                TempData["Message"] = $"{p.ProductId} {p.Title} was added successfully"; // populate success message

                //Rederict back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
