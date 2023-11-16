using System;

namespace reactAzure.Services
{
    public class InternalApplicationException : Exception
    {
        public InternalApplicationException()
        {

        }
        public InternalApplicationException(string sMessage) : base(sMessage)
        {

        }
        public InternalApplicationException(Exception oException) : base(oException.Message, oException)
        {

        }
    }
}
