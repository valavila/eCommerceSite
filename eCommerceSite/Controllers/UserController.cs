using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        //FIELDS
        private readonly ProductContext _context;

        //CONSTRUCTOR
        public UserController(ProductContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                //Map data to user account instance
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    UserName = reg.UserName
                };

                //Add to databse
                _context.UserAccounts.Add(acc);
                await _context.SaveChangesAsync();
                //Rederict to homepage
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }
        
        public IActionResult Login()
        {
            return View();
        }
    }
}
