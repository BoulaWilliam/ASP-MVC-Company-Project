using Company.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Company.PL.ViewModels
{
    public class EmployeeViewModel1
    {
        public int Id { get; set; } //PK

        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "MaxLength Is 50 Chars")]
        [MinLength(5, ErrorMessage = "MinLength Is 5 Chars")]
        public string Name { get; set; }


        [Range(22, 35, ErrorMessage = "Age Must Be In Range From 22 to 35")]
        public int Age { get; set; }

        [RegularExpression("City,Country",
            ErrorMessage = "Address must be Like City,Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public Decimal Salary { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
