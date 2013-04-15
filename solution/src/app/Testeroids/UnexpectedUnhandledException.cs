namespace Testeroids.Aspects
{
    using System;

    public class UnexpectedUnhandledException : Exception
    {
        public UnexpectedUnhandledException(Exception unhandledException)
            : base("An exception which was not expected was thrown and not handled.\r\n\r\n" + unhandledException.Message, unhandledException)
        {
        }
    }
}