using System.ComponentModel.DataAnnotations;

namespace ErrorChat.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Password { get; set; }
    }
}