namespace GuAlex.Calculator.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CalculatorViewModel
    {
        [Required]
        [MaxLength(200, ErrorMessage = "Длина поля '{0}' должна быть не более {1} символов.")]
        [Display(Name = "Выражение")]
        public string Expression { get; set; }
    }
}