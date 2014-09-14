namespace GuAlex.Calculator.Tests
{
    using Irony.Parsing;

    public class EvaluatorTestBase
    {
        public ParseTreeNode CreateNodeNotTerm(string name)
        {
            return new ParseTreeNode(new NonTerminal(name), new SourceSpan());
        }

        public ParseTreeNode CreateNodeToken(string oper)
        {
            return
                new ParseTreeNode(new Token(new Terminal(oper, TokenCategory.Content), new SourceLocation(), oper, oper));
        }

        public ParseTreeNode CreateNumberNode(int value)
        {
            return new ParseTreeNode(new Token(new NumberLiteral("num"), new SourceLocation(), value.ToString(), value));
        }

        public ParseTreeNode CreateStringNode(string value)
        {
            return new ParseTreeNode(new Token(new StringLiteral("str"), new SourceLocation(), value, value));
        }
    }
}