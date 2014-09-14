namespace GuAlex.Calculator
{
    /// <summary>
    /// Интерфейс вычислителя выражений
    /// </summary>
    public interface IExpressionEvaluator
    {
        /// <summary>
        /// Вычисление значения выражения
        /// </summary>
        /// <param name="expression">Выражение</param>
        EvaluationResult Evaluate(string expression);
    }

    /// <summary>
    /// Результат вычисления выражения
    /// </summary>
    public class EvaluationResult
    {
        public EvaluationResult(object result)
        {
            Result = result;
        }

        public EvaluationResult(string errorMessage, int location)
        {
            HasError = true;
            ErrorMessage = errorMessage;
            Location = location;
        }

        /// <summary>
        /// Результат вычисления выражения, при ошибках null.
        /// </summary>
        public object Result { get; private set; }

        /// <summary>
        /// Признак наличия ошибок
        /// </summary>       
        public bool HasError { get; private set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Позиция ошибки в выражении. Начальная позиция 0.
        /// </summary>
        public int Location { get; private set; }
    }
}