using System;

namespace reactAzure.Services
{
    public class UserNotInDatabaseException : Exception
    {
        public UserNotInDatabaseException(string message) : base(message)
        {
        }
    }
}
