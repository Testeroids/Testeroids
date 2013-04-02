namespace Testeroids.Mocking
{
    using System;

    public class SetupWasNeverUsedException : Exception
    {
        public SetupWasNeverUsedException(string message, Exception innerexception) 
            : base(message, innerexception)
        {            
        }
    }
}