namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class UpdateUserException : Exception
    {
        public UpdateUserException() { }
        public UpdateUserException(string? message) : base(message) { }
        public UpdateUserException(string? message, Exception inner) : base(message, inner) { }
        protected UpdateUserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}
