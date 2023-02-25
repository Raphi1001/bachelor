﻿using System.Runtime.Serialization;

namespace tourPlanner.DAL.Exceptions
{
    [Serializable]
    internal class DatabaseErrorException : Exception
    {
        public DatabaseErrorException()
        {
        }

        public DatabaseErrorException(string? message) : base(message)
        {
        }

        public DatabaseErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DatabaseErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}