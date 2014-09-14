namespace GuAlex.Calculator.Tests
{
    using System;
    using Calculator;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class BinNodeEvaluatorTests : EvaluatorTestBase
    {
        [Test]
        public void SimpleOperation()
        {
            var node = CreateNodeNotTerm("BinExpr");
            var arg1 = CreateNumberNode(2);
            var arg2 = CreateNumberNode(3);
            node.ChildNodes.Add(arg1);
            node.ChildNodes.Add(CreateNodeToken("+"));
            node.ChildNodes.Add(arg2);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg1)).Returns(new EvaluationResult((int) arg1.Token.Value));
            mock.Setup(ld => ld.Evaluate(arg2)).Returns(new EvaluationResult((int) arg2.Token.Value));
            var result = BinNodeEvaluator.Evaluate(node, mock.Object);
            Assert.IsFalse(result.HasError);
            Assert.AreEqual(5, result.Result);
        }

        [Test]
        public void NumericOverflow()
        {
            var node = CreateNodeNotTerm("BinExpr");
            var arg1 = CreateNumberNode(int.MaxValue);
            var arg2 = CreateNumberNode(int.MaxValue);
            node.ChildNodes.Add(arg1);
            node.ChildNodes.Add(CreateNodeToken("+"));
            node.ChildNodes.Add(arg2);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg1)).Returns(new EvaluationResult((int) arg1.Token.Value));
            mock.Setup(ld => ld.Evaluate(arg2)).Returns(new EvaluationResult((int) arg2.Token.Value));
            var result = BinNodeEvaluator.Evaluate(node, mock.Object);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(SimpleExpressionEvaluator.OverflowMessage, result.ErrorMessage);
        }

        [Test]
        public void DivideByZero()
        {
            var node = CreateNodeNotTerm("BinExpr");
            var arg1 = CreateNumberNode(5);
            var arg2 = CreateNumberNode(0);
            node.ChildNodes.Add(arg1);
            node.ChildNodes.Add(CreateNodeToken("/"));
            node.ChildNodes.Add(arg2);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg1)).Returns(new EvaluationResult((int) arg1.Token.Value));
            mock.Setup(ld => ld.Evaluate(arg2)).Returns(new EvaluationResult((int) arg2.Token.Value));
            var result = BinNodeEvaluator.Evaluate(node, mock.Object);
            Assert.IsTrue(result.HasError);
            Assert.AreEqual(SimpleExpressionEvaluator.DivideByZero, result.ErrorMessage);
        }

        [Test]
        [ExpectedException(typeof (NotSupportedException))]
        public void Operation_Invalid()
        {
            var node = CreateNodeNotTerm("BinExpr");
            var arg1 = CreateNumberNode(5);
            var arg2 = CreateNumberNode(0);
            node.ChildNodes.Add(arg1);
            node.ChildNodes.Add(CreateNodeToken("invalid"));
            node.ChildNodes.Add(arg2);
            var mock = new Mock<IRootEvaluator>();
            mock.Setup(ld => ld.Evaluate(arg1)).Returns(new EvaluationResult((int) arg1.Token.Value));
            mock.Setup(ld => ld.Evaluate(arg2)).Returns(new EvaluationResult((int) arg2.Token.Value));
            BinNodeEvaluator.Evaluate(node, mock.Object);
        }

        [Test]
        public void Operation_Add()
        {
            Assert.AreEqual(9, BinNodeEvaluator.DoOperAdd(4, 5).Result);
        }

        [Test]
        public void Operation_Minus()
        {
            Assert.AreEqual(2, BinNodeEvaluator.DoOperMinus(6, 4).Result);
        }

        [Test]
        public void Operation_Div()
        {
            var node = CreateNodeNotTerm("BinExpr");
            Assert.AreEqual(2, BinNodeEvaluator.DoOperDiv(10, 4, node).Result);
        }

        [Test]
        public void Operation_Mul()
        {
            Assert.AreEqual(12, BinNodeEvaluator.DoOperMul(3, 4).Result);
        }
    }
}