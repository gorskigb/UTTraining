using System.Collections.Generic;

namespace UTTraining.Infrastructure
{
    public class LogicResult<T>
    {
        public LogicResult()
        {
            ErrorMessages = new List<string>();
        }

        public LogicResult(T result) : this()
        {
            Result = result;
            Succeded = true;
        }

        public LogicResult(IList<string> errorMessages) : this()
        {
            if (errorMessages != null)
                ErrorMessages.AddRange(errorMessages);
        }

        public T Result { get; private set; }

        public bool Succeded { get; set; }

        public List<string> ErrorMessages { get; private set; }
    }
}
