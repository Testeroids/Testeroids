namespace Testeroids.Aspects
{
    using System;

    internal class FaultedTaskExceptionResilientAttribute : FaultedTaskExceptionAttribute
    {
        public Type Exception { get; set; }

        public FaultedTaskExceptionResilientAttribute(Type exception)
        {
            this.Exception = exception;
        }
    }
}