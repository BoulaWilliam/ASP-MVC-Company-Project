using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
	[Authorize]

	public class EmployeeController : Controller
    {
       
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            
            this.unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        // GET: Employee
        public async Task< IActionResult >Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if(string.IsNullOrEmpty(SearchValue))
                employees = await unitOfWork.EmployeeRepository.GetAll();
            else
                employees = unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            //employees = _repository.GetAll();
            var mappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel1>>(employees);

            return View(mappedEmployee);
        }

        // GET: Employee/Details/5
        public async Task< IActionResult >Details(int id)
        {
            var employee = await unitOfWork.EmployeeRepository.GetById(id);

            if (employee is null)
            {
                return NotFound();
            }
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel1>(employee);
            return View(mappedEmployee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
           // ViewBag.Departments = unitOfWork.EmployeeRepository.GetAll();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Create(EmployeeViewModel1 employeeViewModel)
        {

            var employee = _mapper.Map<EmployeeViewModel1, Employee>(employeeViewModel);
            employee.ImageName = DocumentSettings.UploadImage(employeeViewModel.Image,"Images");
            await unitOfWork.EmployeeRepository.Add(employee);
            int Result = await unitOfWork.Complete();
            if (Result > 0)
            {
                TempData["Message"] = "Employee has been created!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await unitOfWork.EmployeeRepository.GetById(id);
            ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();
            var employeeViewModel = _mapper.Map<Employee, EmployeeViewModel1>(employee);

            return View(employeeViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult >Edit(int id, EmployeeViewModel1 employeeViewModel)
        {

            if (id != employeeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<EmployeeViewModel1, Employee>(employeeViewModel);
                    unitOfWork.EmployeeRepository.Update(employee);
                    await unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                }
            }
            return View(employeeViewModel);
        }

        public async Task< IActionResult >Delete(int id)
        {

            var employee = await unitOfWork.EmployeeRepository.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }
            var employeeViewModel = _mapper.Map<Employee, EmployeeViewModel1>(employee);

            return View(employeeViewModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult >DeleteConfirmed(int id, EmployeeViewModel1 employeeViewModel)
        {
            if (id != employeeViewModel.Id)
                return BadRequest();
            try
            {
                var employee = _mapper.Map<EmployeeViewModel1, Employee>(employeeViewModel);
                if(employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName,"Images");
                }
                unitOfWork.EmployeeRepository.Delete(employee);
                await unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            return View(employeeViewModel);
        }

   
    }

}
