namespace GuAlex.Calculator.Web.Tests.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using GuAlex.Calculator.Web.Models;
    using NUnit.Framework;

    [TestFixture]
    public class CalculatorViewModelTests
    {
        [Test]
        public void GoodModel()
        {
            var model = new CalculatorViewModel() { Expression = "123" };
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(model, context, results, true);
            Assert.IsTrue(isModelStateValid);
        }

        [Test]
        public void BadModel()
        {
            var model = new CalculatorViewModel() { Expression = new string('1', 201) };
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(model, context, results, true);
            Assert.IsFalse(isModelStateValid);
        }
    }
}