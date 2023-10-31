using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        //Index
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var users = await userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = userManager.GetRolesAsync(U).Result
                }).ToListAsync(); 
                return View(users);
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                var MappedUser = new UserViewModel
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel>(){ MappedUser });
            }
        }

        //Details
        public async Task<IActionResult> Details(string id,string ViewName = "Details")
        {
            if(id is null)
                return BadRequest();
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();
            
            var mappedUser = mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(mappedUser);
        }

        //Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel UserVM)
        {

            if (id != UserVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var User = await userManager.FindByIdAsync(id);
                    User.FName = UserVM.FName;
                    User.LName = UserVM.LName;
                    User.PhoneNumber = UserVM.PhoneNumber;
                    await userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(UserVM);
        }

        //Delete

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var User = await userManager.FindByIdAsync(id);
                    await userManager.DeleteAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(UserVM);
        }

    }
}
