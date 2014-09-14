namespace GuAlex.Calculator.Tests
{
    using Calculator;
    using Irony.Parsing;
    using NUnit.Framework;

    [TestFixture]
    public class SimpleEvaluatorGrammarTests
    {
        private static SimpleEvaluatorGrammar _grammar;

        [SetUp]
        public void Setupfixture()
        {
            _grammar = new SimpleEvaluatorGrammar();
        }

        [Test]
        public void SingleNumber()
        {
            var result = new Parser(_grammar).Parse("123");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.Number, result.Root.Term);
            Assert.AreEqual(123, result.Root.Token.Value);
        }

        [Test]
        public void MultiNumber()
        {
            var result = new Parser(_grammar).Parse("123 456");
            Assert.AreEqual(ParseTreeStatus.Error, result.Status);
        }

        [Test]
        public void CorrectParentheses()
        {
            var result = new Parser(_grammar).Parse("((1+(2))+3)");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.BinExpr, result.Root.Term);
            Assert.AreEqual(_grammar.BinExpr, result.Root.ChildNodes[0].Term);
        }

        [Test]
        public void InCorrectParentheses()
        {
            var result = new Parser(_grammar).Parse("((1+(2))+3");
            Assert.AreEqual(ParseTreeStatus.Error, result.Status);
        }

        [Test]
        public void Operation_Mul()
        {
            var result = new Parser(_grammar).Parse("1*2");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.BinExpr, result.Root.Term);
            Assert.AreEqual("*", result.Root.ChildNodes[1].Token.Value);
        }

        [Test]
        public void Operation_Div()
        {
            var result = new Parser(_grammar).Parse("6/3/2");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.BinExpr, result.Root.Term);
            Assert.AreEqual("/", result.Root.ChildNodes[1].Token.Value);
            Assert.AreEqual(_grammar.BinExpr, result.Root.ChildNodes[0].Term, "Div is left associative");
        }

        [Test]
        public void Operation_Add()
        {
            var result = new Parser(_grammar).Parse("1+2+3");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.BinExpr, result.Root.Term);
            Assert.AreEqual("+", result.Root.ChildNodes[1].Token.Value);
        }

        [Test]
        public void Operation_Sub()
        {
            var result = new Parser(_grammar).Parse("1-2");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.BinExpr, result.Root.Term);
            Assert.AreEqual("-", result.Root.ChildNodes[1].Token.Value);
        }

        [Test]
        public void Operation_UnMinus()
        {
            var result = new Parser(_grammar).Parse("-(1)");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.UnaryExpr, result.Root.Term);
            Assert.AreEqual("-", result.Root.ChildNodes[0].Token.Value);
        }

        [Test]
        public void Operation_UnPlus()
        {
            var result = new Parser(_grammar).Parse("+(1)");
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.UnaryExpr, result.Root.Term);
            Assert.AreEqual("+", result.Root.ChildNodes[0].Token.Value);
            Assert.AreEqual(_grammar.Number, result.Root.ChildNodes[1].Term);
        }

        [Test]
        public void Operation_Unknown()
        {
            var result = new Parser(_grammar).Parse("1->2");
            Assert.AreEqual(ParseTreeStatus.Error, result.Status);
        }

        [Test]
        public void Number_MaxInt()
        {
            var result = new Parser(_grammar).Parse(int.MaxValue.ToString());
            Assert.AreEqual(ParseTreeStatus.Parsed, result.Status);
            Assert.AreEqual(_grammar.Number, result.Root.Term);
            Assert.AreEqual(int.MaxValue, result.Root.Token.Value);
        }

        [Test]
        public void Number_MaxIntPlusOne()
        {
            var result = new Parser(_grammar).Parse(((long) (int.MaxValue) + 1).ToString());
            Assert.AreEqual(ParseTreeStatus.Error, result.Status);
        }

        [Test]
        public void Number_FloatIsNoValid()
        {
            var result = new Parser(_grammar).Parse("1.0");
            Assert.AreEqual(ParseTreeStatus.Error, result.Status);
        }
    }
}