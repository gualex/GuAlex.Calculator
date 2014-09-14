namespace GuAlex.Calculator.Tests
{
    using Calculator;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleExpressionEvaluatorTests
    {
        [Test]
        public void IntegrationTest()
        {
            var evaluator = new SimpleExpressionEvaluator();
            var result = evaluator.Evaluate("4+14/ ( 2 *4-1)");
            Assert.AreEqual(6, result.Result);
        }

        [Test]
        public void ErrorPosition()
        {
            var evaluator = new SimpleExpressionEvaluator();
            var result = evaluator.Evaluate("1 + 1000000*1000000");
            Assert.AreEqual(true, result.HasError);
            Assert.AreEqual(11, result.Location);
        }

        [Test]
        public void UnariExpression()
        {
            var evaluator = new SimpleExpressionEvaluator();
            var result = evaluator.Evaluate("-(3)");
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(-3, result.Result);
        }

        [Test]
        public void IncorectExpression()
        {
            var evaluator = new SimpleExpressionEvaluator();
            var result = evaluator.Evaluate("1 + 3/)7");
            Assert.AreEqual(true, result.HasError);
        }

        [Test]
        public void NullArgument()
        {
            var evaluator = new SimpleExpressionEvaluator();
            var result = evaluator.Evaluate((string)null);
            Assert.IsTrue(result.HasError);
        }

        [Test]
        public void EmptyArgument()
        {
            var evaluator = new SimpleExpressionEvaluator();
            Assert.IsTrue(evaluator.Evaluate("").HasError);
            Assert.IsTrue(evaluator.Evaluate("  ").HasError);
        }
    }
}