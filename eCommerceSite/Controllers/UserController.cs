using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check if username/email is in use
                bool isEmailTaken = await (from account in _context.UserAccounts
                                           where account.Email == reg.Email
                                           select account).AnyAsync();

                // if so, add custom error and send back to view
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That email is already in use");
                }

                bool isUsernameTaken = await (from account in _context.UserAccounts
                                              where account.UserName == reg.UserName
                                              select account).AnyAsync();
                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.UserName), "That username is taken");
                }

                if (isEmailTaken || isUsernameTaken)
                {
                    return View(reg);
                }


                // Map data to user account instance
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    UserName = reg.UserName
                };
                // add to database
                _context.UserAccounts.Add(acc);
                await _context.SaveChangesAsync();

                LogUserIn(acc.UserId);
                // redirect to home page
                return RedirectToAction("Index", "Home");
            }
            return View(reg);
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            } 
            
            UserAccount account = await (from u in _context.UserAccounts
                                   where (u.UserName == model.UsernameOrEmail || u.Email == model.UsernameOrEmail)
                                   && u.Password == model.Password
                                   select u).SingleOrDefaultAsync();

            if (account == null)
            {
                //Credentials didnt match
                //Custom error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");
                
                return View(model);
            }

            //Log user int owebsite
            HttpContext.Session.SetInt32("UserId", account.UserId);

            LogUserIn(account.UserId);


            return RedirectToAction("Index", "Home");
        }
        private void LogUserIn(int accountId)
        {
            // Log user into website
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // Removes all current session data
            HttpContext.Session.Clear();

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
