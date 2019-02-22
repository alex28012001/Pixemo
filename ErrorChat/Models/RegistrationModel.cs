using System.ComponentModel.DataAnnotations;

namespace ErrorChat.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(20,MinimumLength=3,ErrorMessage = "Никнейм должен состовлять минимум 3 символа")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Пароль должен состовлять минимум 4 символа")]
        public string Password { get; set; }
    }
}