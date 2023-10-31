using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {//Get All
            var Departments =  await _unitOfWork.DepartmentRepository.GetAll();
            return View(Departments);
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if(ModelState.IsValid) // Server Side Validation
            {
                await _unitOfWork.DepartmentRepository.Add(department);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(department); // To The Same Input He Just Did
        }

        //Details
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
                var Department = await _unitOfWork.DepartmentRepository.GetById(id.Value);

                if (Department is null)
                    return NotFound();
            
            return View(ViewName,Department);
        }

        //Update
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id,"Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id ,Department department)
        {
            if (id != department.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
        }


        [HttpGet]
        public async Task<IActionResult>  Delete(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
            {
                return NotFound();
            }

            return View(department);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  DeleteConfirmed(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
            {
                return NotFound();
            }

            _unitOfWork.DepartmentRepository.Delete(department);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}
