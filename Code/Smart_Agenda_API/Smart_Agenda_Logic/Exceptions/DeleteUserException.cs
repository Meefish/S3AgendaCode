namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class DeleteUserException : Exception
    {

        public DeleteUserException()
        {
        }

        public DeleteUserException(string? message) : base(message)
        {
        }

        public DeleteUserException(string? message, Exception innerException) : base(message, innerException)
        {
        }
        protected DeleteUserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {

        }
    }
}
