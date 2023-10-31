using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; } //PK

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Age { get; set; }

        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public Decimal Salary { get; set; }

        public bool IsActive { get; set; }
        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageName { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
