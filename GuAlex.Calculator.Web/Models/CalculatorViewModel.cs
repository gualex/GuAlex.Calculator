namespace GuAlex.Calculator.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CalculatorViewModel
    {
        [Required]
        [Display(Name = "Выражение")]
        public string Expression { get; set; }
    }
}