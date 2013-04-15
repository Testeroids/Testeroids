namespace Testeroids.Aspects
{
    using System;

    public class FaultedTaskExceptionResilientAttribute : FaultedTaskExceptionAttribute
    {
        public Type Exception { get; set; }

        public FaultedTaskExceptionResilientAttribute(Type exception)
        {
            this.Exception = exception;
        }
    }
}