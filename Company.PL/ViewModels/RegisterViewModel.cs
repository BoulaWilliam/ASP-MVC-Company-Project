using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "First Name Is Required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "Last Name Is Required")]

		public string LName { get; set; }
		[Required(ErrorMessage = "Email  Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password  Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Password  Is Required")]
		[Compare("Password",ErrorMessage = "Confirm Password Does not Match The Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }

	}
}
