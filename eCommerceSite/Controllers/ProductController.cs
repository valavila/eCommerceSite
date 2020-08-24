using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            //Get sll producrs from databse
            //List<Product> products = _context.Products.ToList();
            List<Product> products = await (from p in _context.Products
                                      select p).ToListAsync();

            //Send list of products to view to be diplayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product p)
        {
            if (ModelState.IsValid)
            {
                //Add to DB
                _context.Products.Add(p); // Creates insert querey
                await _context.SaveChangesAsync(); // executes query

                TempData["Message"] = $"{p.Title} was added successfully"; // populate success message

                //Rederict back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
