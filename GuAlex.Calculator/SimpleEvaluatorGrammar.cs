namespace GuAlex.Calculator
{
    using System;
    using Irony.Parsing;

    /// <summary>
    /// Грамматика простого калькулятора.
    /// Поддерживаются операций '+', '-' , '*', '/' и скобки.
    /// </summary>
    [Language("Простой калькулятор", "1.0", "")]
    public sealed class SimpleEvaluatorGrammar : Grammar
    {
        public SimpleEvaluatorGrammar()
        {
            // Терминальные символы
            Number = new NumberLiteral("number", NumberOptions.IntOnly | NumberOptions.AllowSign);
            Number.DefaultIntTypes = new[] {TypeCode.Int32};

            // Нетерминальные символы
            BinExpr = new NonTerminal("BinExpr");
            UnaryExpr = new NonTerminal("UnExpr");
            var expr = new NonTerminal("Expr");
            var term = new NonTerminal("Term");
            var parExpr = new NonTerminal("ParExpr");
            var unOp = new NonTerminal("UnOp");
            var binOp = new NonTerminal("BinOp");

            // БНФ правила
            expr.Rule = term | UnaryExpr | BinExpr;
            term.Rule = Number | parExpr;
            parExpr.Rule = "(" + expr + ")";
            UnaryExpr.Rule = unOp + term + ReduceHere();
            unOp.Rule = ToTerm("+") | "-";
            BinExpr.Rule = expr + binOp + expr;
            binOp.Rule = ToTerm("+") | "-" | "*" | "/";

            // Корень грамматики
            Root = expr;

            // Приоритеты операций
            RegisterOperators(30, "+", "-");
            RegisterOperators(40, "*", "/");

            // Пунктуация
            MarkPunctuation("(", ")");
            RegisterBracePair("(", ")");

            MarkTransient(term, expr, binOp, unOp, parExpr);
        }

        public NumberLiteral Number { get; private set; }
        public NonTerminal BinExpr { get; private set; }
        public NonTerminal UnaryExpr { get; private set; }
    }
}