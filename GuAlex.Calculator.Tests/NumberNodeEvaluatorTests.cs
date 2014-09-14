namespace GuAlex.Calculator.Tests
{
    using Calculator;
    using NUnit.Framework;

    [TestFixture]
    public class NumberNodeEvaluatorTests : EvaluatorTestBase
    {
        [Test]
        public void NodeIsValidInteger()
        {
            var node = CreateNumberNode(123);
            var result = NumberNodeEvaluator.Evaluate(node);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(123, result.Result);
        }

        [Test]
        public void NodeIsInvalidTerm()
        {
            var node = CreateStringNode("123");
            var result = NumberNodeEvaluator.Evaluate(node);
            Assert.IsTrue(result.HasError);
        }

        [Test]
        public void NodeIsNonTerm()
        {
            var node = CreateNodeNotTerm("!@#");
            var result = NumberNodeEvaluator.Evaluate(node);
            Assert.IsTrue(result.HasError);
        }
    }
}