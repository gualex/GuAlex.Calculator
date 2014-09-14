namespace GuAlex.Calculator
{
    using System;
    using Irony.Parsing;

    /// <summary>
    /// Интерфейс для рекурсивного вычисления значений узлов 
    /// </summary>
    public interface IRootEvaluator
    {
        /// <summary>
        /// Рекурсивное вычисление значения для узла
        /// </summary>
        /// <param name="node">Узел в дереве разбора</param>
        EvaluationResult Evaluate(ParseTreeNode node);
    }

    /// <summary>
    /// Вычислитель простых выражений с грамматикой <see cref="SimpleEvaluatorGrammar"/>.
    /// Вычисления выполняются с типом <see cref="int"/>
    /// </summary>
    public class SimpleExpressionEvaluator : IExpressionEvaluator, IRootEvaluator
    {
        public static readonly string OverflowMessage = "Арифметическое переполнение.";
        public static readonly string DivideByZero = "Деление на ноль.";
        public static readonly string IncorrectExpression = "Некорректное выражение.";
        public static readonly string EmptyExpression = "Пустое выражение.";
        public static readonly string OperationNotSupported = "Операция '{0}' не поддерживается.";

        private static readonly SimpleEvaluatorGrammar Grammar = new SimpleEvaluatorGrammar();
        private static readonly LanguageData Language = new LanguageData(Grammar);

        /// <summary>
        /// Вычисление выражения
        /// </summary>
        /// <param name="expression">Текстовое представление выражения</param>
        public EvaluationResult Evaluate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return new EvaluationResult(EmptyExpression, 0);

            var parser = new Parser(Language);
            var parseResult = parser.Parse(expression);
            if (parseResult.HasErrors())
            {
                return new EvaluationResult(IncorrectExpression,
                    parseResult.ParserMessages[0].Location.Position);
            }

            return Evaluate(parseResult.Root);
        }

        public EvaluationResult Evaluate(ParseTreeNode node)
        {
            if (node.Term == Grammar.Number)
                return NumberNodeEvaluator.Evaluate(node);

            if (node.Term == Grammar.UnaryExpr)
                return UnaryNodeEvaluator.Evaluate(node, this);

            if (node.Term == Grammar.BinExpr)
                return BinNodeEvaluator.Evaluate(node, this);

            return new EvaluationResult(IncorrectExpression, node.Span.Location.Position);
        }
    }

    /// <summary>
    /// Вычилитель для числовых терминалов
    /// </summary>
    public static class NumberNodeEvaluator
    {
        public static EvaluationResult Evaluate(ParseTreeNode node)
        {
            if (node.Token != null && node.Token.Value is int)
                return new EvaluationResult((int)node.Token.Value);

            return new EvaluationResult(SimpleExpressionEvaluator.IncorrectExpression,
                node.Span.Location.Position);
        }
    }

    /// <summary>
    /// Вычислитель для унарных операций
    /// </summary>
    public static class BinNodeEvaluator
    {
        public static EvaluationResult Evaluate(ParseTreeNode node, IRootEvaluator rootEvaluator)
        {
            var arg1 = rootEvaluator.Evaluate(node.ChildNodes[0]);
            var operNode = node.ChildNodes[1];
            var oper = operNode.Token.Value as string;
            var arg2 = rootEvaluator.Evaluate(node.ChildNodes[2]);
            if (arg1.HasError)
                return arg1;

            if (arg2.HasError)
                return arg2;

            try
            {
                if (oper == "+" && arg1.Result is int && arg2.Result is int)
                    return DoOperAdd((int)arg1.Result, (int)arg2.Result);

                if (oper == "-" && arg1.Result is int && arg2.Result is int)
                    return DoOperMinus((int)arg1.Result, (int)arg2.Result);

                if (oper == "*" && arg1.Result is int && arg2.Result is int)
                    return DoOperMul((int)arg1.Result, (int)arg2.Result);

                if (oper == "/" && arg1.Result is int && arg2.Result is int)
                    return DoOperDiv((int)arg1.Result, (int)arg2.Result, operNode);
            }
            catch (ArithmeticException)
            {
                return new EvaluationResult(SimpleExpressionEvaluator.OverflowMessage,
                    operNode.Span.Location.Position);
            }
            throw new NotSupportedException(string.Format(SimpleExpressionEvaluator.OperationNotSupported, oper));
        }

        public static EvaluationResult DoOperAdd(int arg1Val, int arg2Val)
        {
            checked
            {
                return new EvaluationResult(arg1Val + arg2Val);
            }
        }

        public static EvaluationResult DoOperMinus(int arg1Val, int arg2Val)
        {
            checked
            {
                return new EvaluationResult(arg1Val - arg2Val);
            }
        }

        public static EvaluationResult DoOperMul(int arg1Val, int arg2Val)
        {
            checked
            {
                return new EvaluationResult(arg1Val * arg2Val);
            }
        }

        public static EvaluationResult DoOperDiv(int arg1Val, int arg2Val, ParseTreeNode node)
        {
            if (arg2Val == 0)
            {
                return new EvaluationResult(SimpleExpressionEvaluator.DivideByZero,
                    node.Span.Location.Position);
            }

            return new EvaluationResult(arg1Val / arg2Val);
        }
    }

    /// <summary>
    /// Вычислитель для бинарных операций
    /// </summary>
    public static class UnaryNodeEvaluator
    {
        public static EvaluationResult Evaluate(ParseTreeNode node, IRootEvaluator rootEvaluator)
        {
            var sing = node.ChildNodes[0].Token.Value as string;
            var arg = rootEvaluator.Evaluate(node.ChildNodes[1]);

            if (arg.HasError)
                return arg;

            if (sing == "-")
                return new EvaluationResult(-(int)arg.Result);

            if (sing == "+")
                return arg;

            throw new NotSupportedException(string.Format(SimpleExpressionEvaluator.OperationNotSupported, sing));
        }
    }
}