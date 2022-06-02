using System.ComponentModel.DataAnnotations;

namespace Site.Models
{
    public class AuthenticationRequest
    {
        [Required]
        [Display(Name = "Логин")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
