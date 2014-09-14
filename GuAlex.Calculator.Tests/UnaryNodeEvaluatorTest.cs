namespace GuAlex.Calculator.Tests
{
    using System;
    using Calculator;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnaryNodeEvaluatorTest : EvaluatorTestBase
    {
        [Test]
        public void UnMinus()
        {
            var node = CreateNodeNotTerm("UnExpr");
            var arg = CreateNumberNode(23);
            node.ChildNodes.Add(CreateNodeToken("-"));
            node.ChildNodes.Add(arg);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg)).Returns(new EvaluationResult((int) arg.Token.Value));
            var result = UnaryNodeEvaluator.Evaluate(node, mock.Object);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(-23, result.Result);
        }

        [Test]
        public void UnPlus()
        {
            var node = CreateNodeNotTerm("UnExpr");
            var arg = CreateNumberNode(23);
            node.ChildNodes.Add(CreateNodeToken("+"));
            node.ChildNodes.Add(arg);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg)).Returns(new EvaluationResult((int) arg.Token.Value));
            var result = UnaryNodeEvaluator.Evaluate(node, mock.Object);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(23, result.Result);
        }

        [Test]
        [ExpectedException(typeof (NotSupportedException))]
        public void NotSupported()
        {
            var node = CreateNodeNotTerm("UnExpr");
            var arg = CreateNumberNode(23);
            node.ChildNodes.Add(CreateNodeToken("@"));
            node.ChildNodes.Add(arg);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg)).Returns(new EvaluationResult((int) arg.Token.Value));
            UnaryNodeEvaluator.Evaluate(node, mock.Object);
        }
    }
}