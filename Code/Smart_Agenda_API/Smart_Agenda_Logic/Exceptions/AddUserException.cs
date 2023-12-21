namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class AddUserException : Exception
    {
        public AddUserException() { }
        public AddUserException(string? message) : base(message) { }
        public AddUserException(string? message, Exception inner) : base(message, inner) { }

        protected AddUserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}
