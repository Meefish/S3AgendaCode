namespace Smart_Agenda_Logic.Exceptions
{

    [Serializable]
    public class UserException : Exception
    {

        public UserException()
        {
        }
        public UserException(string? message) : base(message)
        {
        }

        public UserException(string? message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}
