namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class UpdateTaskException : Exception
    {
        public UpdateTaskException()
        {
        }

        public UpdateTaskException(string? message) : base(message)
        {
        }

        public UpdateTaskException(string? message, Exception innerException) : base(message, innerException)
        {
        }
        protected UpdateTaskException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {

        }
    }
}
