namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class RetrieveUserException : Exception
    {
        public RetrieveUserException()
        {
        }
        public RetrieveUserException(string? message) : base(message)
        {
        }
        public RetrieveUserException(string? message, Exception innerException) : base(message, innerException)
        {
        }
        protected RetrieveUserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}
