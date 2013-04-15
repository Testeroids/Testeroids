namespace Testeroids.Aspects
{
    using System;

    public class FaultedTaskExpectedExceptionAttribute : FaultedTaskExceptionAttribute
    {
        public Type ExpectedException { get; set; }

        public FaultedTaskExpectedExceptionAttribute(Type expectedException)
        {
            ExpectedException = expectedException;
            if (!expectedException.IsSubclassOf(typeof(Exception)))
            {
                throw new NotSupportedException("Only types inherited from Exception are supported");
            }
        }
    }
}