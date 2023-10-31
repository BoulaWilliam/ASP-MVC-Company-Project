using AutoMapper;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        //Index
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var roles = await roleManager.FindByNameAsync(name);
                var MappedRole = new RoleViewModel
                {
                    Id = roles.Id,
                    RoleName = roles.Name,
                    
                };
                return View(new List<RoleViewModel>() { MappedRole });
            }
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel RoleVm)
        {
            if (ModelState.IsValid)
            {
                var MappedRole = mapper.Map<RoleViewModel, IdentityRole>(RoleVm);
                await roleManager.CreateAsync(MappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(RoleVm);
        }

        //Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var mappedRole = mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(mappedRole);
        }

        //Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleViewModel RoleVm)
        {

            if (id != RoleVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await roleManager   .FindByIdAsync(id);
                    Role.Name = RoleVm.RoleName;
                    await roleManager.UpdateAsync(Role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(RoleVm);
        }

        //Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel RoleVM)
        {
            if (id != RoleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await roleManager.FindByIdAsync(id);
                    await roleManager.DeleteAsync(Role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(RoleVM);
        }

    }
}
