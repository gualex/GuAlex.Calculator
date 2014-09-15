namespace GuAlex.Calculator.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Models;

    public class CalculatorController : Controller
    {
        public IExpressionEvaluator ExpressionEvaluator { get; set; }

        // GET: Calculator
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetResult(CalculatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var expression = model.Expression;
                var result = ExpressionEvaluator.Evaluate(expression);

                var msg = result.HasError
                    ? FormatEvaluationErrorMessage(result)
                    : result.Result.ToString();

                return Json(new {Result = msg, HasError = result.HasError}, JsonRequestBehavior.AllowGet);
            }

            string messages = string.Join("; ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            return Json(new {Result = messages, HasError = true}, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public string FormatEvaluationErrorMessage(EvaluationResult evaluationResult)
        {
            if (evaluationResult.Location == 0)
                return evaluationResult.ErrorMessage;
            return string.Format("{0} (позиция {1})", evaluationResult.ErrorMessage, evaluationResult.Location + 1);
        }
    }
}