using GuAlex.Calculator.Web.Models;

namespace GuAlex.Calculator.Web.Tests.Controllers
{
    using Moq;
    using NUnit.Framework;
    using System.Web.Mvc;
    using Web.Controllers;

    [TestFixture]
    public class CalculatorControllerTests
    {
        private CalculatorController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new CalculatorController();
        }

        [Test]
        public void FormatEvaluationErrorMessage()
        {
            Assert.AreEqual("QWE", _controller.FormatEvaluationErrorMessage(new EvaluationResult("QWE", 0)));
            Assert.AreEqual("QWE (позиция 3)", _controller.FormatEvaluationErrorMessage(new EvaluationResult("QWE", 2)));
        }

        [Test]
        public void Index()
        {
            var result = _controller.Index();
            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public void GetResult_Correct()
        {
            var mock = new Mock<IExpressionEvaluator>();
            mock.Setup(ld => ld.Evaluate("1+2")).Returns(new EvaluationResult(3));
            _controller.ExpressionEvaluator = mock.Object;
            var jsonResult = _controller.GetResult(new CalculatorViewModel { Expression = "1+2" });
            var wrapper = new System.Web.Routing.RouteValueDictionary(jsonResult.Data);
            Assert.AreEqual("3", wrapper["Result"]);
        }

        [Test]
        public void GetResult_Error()
        {
            var mock = new Mock<IExpressionEvaluator>();
            mock.Setup(ld => ld.Evaluate("1/0")).Returns(new EvaluationResult("ERR", 1));
            _controller.ExpressionEvaluator = mock.Object;
            var jsonResult = _controller.GetResult(new CalculatorViewModel { Expression = "1/0" });
            var wrapper = new System.Web.Routing.RouteValueDictionary(jsonResult.Data);
            Assert.AreEqual(true, wrapper["HasError"]);
            Assert.IsTrue(((string)wrapper["Result"]).StartsWith("ERR"));
        }

        [Test]
        public void GetResult_LongExpression()
        {
            _controller.ModelState.AddModelError("LongExpr", "Выражение слишком длинное");
            var jsonResult = _controller.GetResult(new CalculatorViewModel { Expression = new string('1', 201) });
            var wrapper = new System.Web.Routing.RouteValueDictionary(jsonResult.Data);
            Assert.AreEqual(true, wrapper["HasError"]);
        }
    }
}
